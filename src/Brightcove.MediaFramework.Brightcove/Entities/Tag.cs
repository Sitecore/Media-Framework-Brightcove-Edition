using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brightcove.MediaFramework.Brightcove.Entities
{
    public class Tag : Asset
    {
        public override string ToString()
        {
            return string.Format("(type:{0},name:{1})", (object)this.GetType().Name, (object)this.Name);
        }
    }
}
