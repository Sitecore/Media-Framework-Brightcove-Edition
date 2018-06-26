using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightcove.MediaFramework.Brightcove.Entities.ViewModels
{
    public class VideoCustomFields
    {
        public Video Video { get;set; }
        public IList<FieldInfo> Fields { get; set; }
    }
}
