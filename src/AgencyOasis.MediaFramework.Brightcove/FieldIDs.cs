using Sitecore.Data;

namespace Brightcove.MediaFramework.Brightcove
{
    public static class FieldIDs
    {
        public static class Account
        {
            public static readonly ID ClientId = new ID("{E7FCF624-4A36-4BE8-B2F5-D5C30BE677C4}");
            public static readonly ID ClientSecret = new ID("{F537C3A0-152C-4AE1-A21B-EBF69B8EE51B}");
        }

        public static class Video
        {
            public static readonly ID Duration = new ID("{25F40EE3-E453-4899-A728-E9DB0ED6E6E3}");
            public static readonly ID IngestJobId = new ID("{82A9CD93-9937-4765-8854-9F03F26B2094}");
            public static readonly ID IngestStatus = new ID("{8FEAFD50-9082-4F77-A9E7-97963220D50F}");

            // Video sharing section fields
            public static readonly ID ByExternalAcct = new ID("{34E7A4EA-3AB4-46A8-8EFD-FE269820382B}");
            public static readonly ID ById = new ID("{643248F5-49FE-4CF4-935B-212B66B24400}");
            public static readonly ID SourceId = new ID("{A74DB86B-296A-4562-B2F2-22DB9CDD68B1}");
            public static readonly ID ToExternalAcct = new ID("{DD2B605D-5493-477E-AC11-34D39DB96015}");
            public static readonly ID ByReference = new ID("{DB5AE464-3523-444E-94F4-398C01BEAE6B}");
        }

        public static class Playlist
        {
            public static readonly ID CreationDate = new ID("{5A33E84E-9453-4ED4-8109-473FEAECE931}");
            public static readonly ID LastModifiedDate = new ID("{00D9ED39-D12C-48E7-A998-B5976E9C4F52}");
            public static readonly ID Favorite = new ID("{1D354E71-8722-4096-9C02-0C5AE1BEC4AB}");
        }

        public static class Player
        {
            public static readonly ID Class = new ID("{BEACD02A-B365-468E-83C7-3ADA3C58DB4F}");
            public static readonly ID ShowPlaylist = new ID("{E09287AB-76D7-4F11-A759-58BE886F85A1}");
        }
    }
}