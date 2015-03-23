## PSImaging
PowerShell module that provides tools for automating document image management tasks.

### Overview

PSImaging is designed to surface common imaging tools to PowerShell using correct PowerShell syntax.

### Minimum requirements

- PowerShell 3.0

### License and Copyright

Copyright 2015 Positronic IO

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

### Installing the PSImaging module

You can download and install the latest versions of PSImaging using any of the following methods:

#### PowerShell 3.0 or Later

To install from PowerShell 3.0 or later, open a native PowerShell console (not ISE,
unless you want it to take longer), and invoke one of the following commands:

```powershell
# If you want to install PSImaging for all users or update a version already installed
# (recommended, requires elevation for new install for all users)
& ([scriptblock]::Create((iwr -uri http://tinyurl.com/Install-GitHubHostedModule).Content)) `
  -GitHubUserName Positronic-IO -ModuleName PSImaging -Branch 'master'

# If you want to install PSImaging for the current user
& ([scriptblock]::Create((iwr -uri http://tinyurl.com/Install-GitHubHostedModule).Content)) `
  -GitHubUserName Positronic-IO -ModuleName PSImaging -Branch 'master' -Scope CurrentUser
```

### Using the PSImaging module

The Get-ImageHash Cmdlet uses a signature schema developed by H. Chi Wong, Marshall Bern, and David Goldberg of Xerox, published in 2002, An image signature for any kind of image. The algorithm compares relative brightness levels of regions within the image. This method is resilient to some resizing, cropping, and compression. Here's an example:

```powershell
PS C:\> Get-ImageHash .\Quick-Brown-Fox.png -Level 4
000000000000000000005210004240808004525256738080008052104673808000805210425280800000525
63567777777734000880000800880908EB50CF3908B50567356735677356300000000000000008080800080
800080D00A94E0D200EB0856735252946F356300000042142100008080080800800880D20CB561F300F7085
6731042946B1463000000421421000000000000000000008008808080808008D4F18A40DA4A1CA1
```

The Export-Text Cmdlet in this module uses Tesseract by Google (https://code.google.com/p/tesseract-ocr/), the most accurate open-source OCR engine available. The Tesseract dependencies are bundled into the module. No additional files are required to use this Cmdlet. Here an exmaple:

```
PS C:\> Export-ImageText .\Quick-Brown-Fox.png
The quick brown fox jumps right
over the lazy dog. the quick brown
fox jumps right over the lazy dog.
```

The Group-ImageFile Cmdlet utilizes the Get-ImageHash function to generate signatures for all of the image files in a collection, then compares them to one another for simlarity, grouping those that it has a degree of confidence are similar. Here's an example:

```
PS C:\> $groups = dir | Group-ImageFile
PS C:\> $groups

                Confidence                      Count Files
                ----------                      ----- -----
                      High                          5 {C:\Users\ben_000\Test...
                    Medium                          5 {C:\Users\ben_000\Test...
                    Medium                          3 {C:\Users\ben_000\Test...
                    Medium                          3 {C:\Users\ben_000\Test...
                    
PS C:\> $groups | ? Confidence -eq High | select -ExpandProperty Files

Mode                LastWriteTime     Length Name
----                -------------     ------ ----
-a---         10/4/2014  10:20 AM    2816872 1.tiff
-a---         2/16/2015   9:34 PM    2736074 10.tiff
-a---         2/16/2015   9:34 PM    2753074 17.tiff
-a---         1/18/2015   2:02 PM    2753074 4.tiff
-a---         1/18/2015   2:02 PM    2753074 7.tiff

```


Your feedback is appreciated. Let us know what we missed. Thank you for encouraging more contributions to this project by leaving us a tip at https://cash.me/$Positronic.
