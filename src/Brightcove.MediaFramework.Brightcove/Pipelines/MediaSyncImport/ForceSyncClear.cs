using Sitecore.MediaFramework.Pipelines.MediaSyncImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Brightcove.MediaFramework.Brightcove.Pipelines.MediaSyncImport
{
    public class ForceSyncClear : MediaSyncImportProcessorBase
    {
        public override void Process(MediaSyncImportArgs args)
        {
            HttpRuntime.Cache.Remove("BrightcoveForceSync");
        }
    }
}
