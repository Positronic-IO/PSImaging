using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace PSImaging
{
    public class BaseCmdlet : PSCmdlet
    {
        protected string FixRelativePath(string path, string currentPath)
        {
            string retval = path;
            if (!retval.Contains(@":\") || !retval.StartsWith(@"\"))
                retval = Path.Combine(currentPath, retval);
            return retval;
        }
    }
}
