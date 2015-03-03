using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace PSImaging.ImageText
{
    [Cmdlet("Export", "ImageText", SupportsShouldProcess = true)]
    [Description("Extracts text from a given image file.")]
    public class Export_ImageText : BaseCmdlet
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
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The rectangle within the image to extract data from.")]
        public System.Drawing.Rectangle Rect;

        [STAThread]
        protected override void ProcessRecord()
        {
            string thisAsm = (new Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath;
            string tessdataPath = System.IO.Path.Combine(new FileInfo(thisAsm).DirectoryName, "tessdata");
            
            string[] paths = this.Path;

            using (var engine = new TesseractEngine(tessdataPath, "eng", EngineMode.Default))
            {
                foreach (string path in paths)
                {
                    var fixedPath = FixRelativePath(path);
                    using (var img = Pix.LoadFromFile(fixedPath))
                    {
                        Tesseract.Rect rect = this.Rect == null || this.Rect == System.Drawing.Rectangle.Empty 
                            ? Tesseract.Rect.Empty 
                            : new Tesseract.Rect(this.Rect.X, this.Rect.Y, this.Rect.Width, this.Rect.Height);
                            using (var page = engine.Process(img, rect))
                                WriteObject(page.GetText());
                    }
                }
            }

        }
    }
}
