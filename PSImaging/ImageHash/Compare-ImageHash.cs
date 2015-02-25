using LibSimilarImageDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PSImaging.ImageHash
{
    [Cmdlet("Compare", "ImageHash", SupportsShouldProcess = true)]
    [Description("Compares two image hashes and get a coefficient of similarity (0.0=Completely distinct, 1.0=Equal)")]
    public class Compare_ImageHash : PSCmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The first hash in the comparison.")]
        public string Source;

        [Parameter(
            Position = 1,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The second hash in the comparison.")]
        public string Target;

        [STAThread]
        protected override void ProcessRecord()
        {
            WriteObject(SimilarImage.CompareHashes(this.Source, this.Target));
        }
    }
}
