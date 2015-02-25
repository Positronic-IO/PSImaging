using LibSimilarImageDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PSImaging.ImageHash
{
    public enum ConfidenceEnum { High, Medium}
    public class GroupResult
    {
        public ConfidenceEnum Confidence;
        public int Count;
        public List<FileInfo> Files;
    }

    [Cmdlet("Group", "ImageFile", SupportsShouldProcess = true)]
    [Description("Groups image files by similarity.")]
    public class Group_ImageFile : BaseCmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The paths of the images that will be grouped.")]
        public string[] Path;

        private List<string> Paths = new List<string>();

        protected override void EndProcessing()
        {
            List<GroupResult> retval = new List<GroupResult>();

            List<KeyValuePair<string, string>> hashes = new List<KeyValuePair<string, string>>();
            foreach (string path in this.Paths)
            {
                string absolutePath = FixRelativePath(path, SessionState.Path.CurrentFileSystemLocation.Path);
                hashes.Add(new KeyValuePair<string, string>(absolutePath, Get_ImageHash.GetImageHash(absolutePath, 6)));
            }

            foreach (KeyValuePair<string, string> outerpair in hashes)
            {
                if (retval.Any(r => r.Files.Any(f => f.FullName == outerpair.Key)))
                    continue;

                List<string> highConfidenceMatches = new List<string>();
                List<string> mediumConfidenceMatches = new List<string>();

                foreach (KeyValuePair<string, string> innerpair in hashes)
                {
                    float similarity = SimilarImage.CompareHashes(outerpair.Value, innerpair.Value);
                    if (similarity == 1)
                        continue;
                    if (similarity > .8)
                        highConfidenceMatches.Add(innerpair.Key);
                    else if (similarity > .55)
                        mediumConfidenceMatches.Add(innerpair.Key);
                }

                if (highConfidenceMatches.Count > 0)
                {
                    highConfidenceMatches.Add(outerpair.Key);
                    AddGroupToResults(ConfidenceEnum.High, retval, highConfidenceMatches);
                }
                else if (mediumConfidenceMatches.Count > 0)
                {
                    mediumConfidenceMatches.Add(outerpair.Key);
                    AddGroupToResults(ConfidenceEnum.Medium, retval, mediumConfidenceMatches);
                }
            }

            WriteObject(retval);

            base.EndProcessing();
        }

        private void AddGroupToResults(ConfidenceEnum confidence, List<GroupResult> results, List<string> paths)
        {
            paths.Sort();

            if (results.Any(result => string.Compare(string.Join(";", result.Files.Select(fi => fi.FullName)), string.Join(";", paths)) == 0))
                return;

            results.Add(new GroupResult
            {
                Confidence = confidence,
                Count = paths.Count,
                Files = paths.Select(fi => new FileInfo(fi)).ToList()
            });

        }

        [STAThread]
        protected override void ProcessRecord()
        {
            this.Paths.AddRange(this.Path);
        }

    }
}
