namespace AgencyOasis.MediaFramework.Brightcove.Serialization
{
    public class JsonNetSerializer : Sitecore.RestSharp.Serialization.JsonNetSerializer
    {
        public JsonNetSerializer()
        {
            ContentType = "application/json";
        }
    }
}