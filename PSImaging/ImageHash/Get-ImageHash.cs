using LibSimilarImageDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PSImaging.ImageHash
{
    [Cmdlet("Get", "ImageHash", SupportsShouldProcess = true)]
    [Description("Generates a hash for a given image file.")]
    public class Get_ImageHash : BaseCmdlet
    {
        [Parameter(
            ParameterSetName = "Default",
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The path of an image to extract data from.")]
        public string[] Path;

        [Parameter(
            Position = 1,
            Mandatory = false,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Set hash levels (default = 2)")]
        public int Level = 2;

        [STAThread]
        protected override void ProcessRecord()
        {
            foreach (string path in this.Path)
                WriteObject(GetImageHash(FixRelativePath(path), this.Level));
        }

        public static string GetImageHash(string path, int level)
        {
            Bitmap bitmap = new Bitmap(Image.FromFile(path));
            return SimilarImage.GetCompressedImageHashAsString(bitmap, level);
        }
    }
}
