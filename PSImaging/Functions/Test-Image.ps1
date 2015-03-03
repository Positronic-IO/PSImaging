	function Test-Image {
	    [CmdletBinding()]
	    [OutputType([System.Boolean])]
	    param(
	        [Parameter(Mandatory=$true, Position=0, ValueFromPipeline=$true)] 
	        [ValidateNotNullOrEmpty()]
	        [Alias('PSPath')]
	        [string] $Path
	    )
	
	    PROCESS {
	        $knownHeaders = @{
	            jpg = @( "FF", "D8" );
	            bmp = @( "42", "4D" );
	            gif = @( "47", "49", "46" );
	            tif = @( "49", "49", "2A" );
	            png = @( "89", "50", "4E", "47", "0D", "0A", "1A", "0A" );
	            pdf = @( "25", "50", "44", "46" );
	        }
	        
	        # coerce relative paths from the pipeline into full paths
	        if($_ -ne $null) {
	            $Path = $_.FullName
	        }
	
	        # read in the first 8 bits
	        $bytes = Get-Content -LiteralPath $Path -Encoding Byte -ReadCount 1 -TotalCount 8 -ErrorAction Ignore
	
	        $retval = $false
	        foreach($key in $knownHeaders.Keys) {
	
	            # make the file header data the same length and format as the known header
	            $fileHeader = $bytes | 
	                Select-Object -First $knownHeaders[$key].Length | 
	                ForEach-Object { $_.ToString("X2") }
	            if($fileHeader.Length -eq 0) {
	                continue
	            }
	
	            # compare the two headers
	            $diff = Compare-Object -ReferenceObject $knownHeaders[$key] -DifferenceObject $fileHeader
	            if(($diff | Measure-Object).Count -eq 0) {
	                $retval = $true
	            }
	        }
	
	        return $retval
	    }
	}
