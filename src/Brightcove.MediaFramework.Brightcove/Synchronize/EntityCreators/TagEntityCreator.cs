using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Brightcove.MediaFramework.Brightcove.Entities;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.MediaFramework.Synchronize;
using Video = Brightcove.MediaFramework.Brightcove.Entities.Video;

namespace Brightcove.MediaFramework.Brightcove.Synchronize.EntityCreators
{
    public class TagEntityCreator : IMediaServiceEntityCreator
    {
        public virtual object CreateEntity(Item item)
        {
            return (object)new Tag()
            {
                Name = item[FieldIDs.Tag.Name]
            };
        }
    }
}
