using Sitecore.Data;

namespace Brightcove.MediaFramework.Brightcove
{
    public static class FieldIDs
    {
        public static class Account
        {
            public static readonly ID ClientId = new ID("{E7FCF624-4A36-4BE8-B2F5-D5C30BE677C4}");
            public static readonly ID ClientSecret = new ID("{F537C3A0-152C-4AE1-A21B-EBF69B8EE51B}");
            public static readonly ID PublisherId = new ID("{3B2A0B52-EEB1-42A9-989D-DD0F57986E67}");
            public static readonly ID ReadToken = new ID("{E7FCF624-4A36-4BE8-B2F5-D5C30BE677C4}");
            public static readonly ID WriteToken = new ID("{F537C3A0-152C-4AE1-A21B-EBF69B8EE51B}");
        }

        public static class AccountSettings
        {
            public static readonly ID DefaultVideoPlayer = new ID("{F122FBAE-16CF-4D7F-A351-55A0E80A35CA}");
            public static readonly ID DefaultPlaylistPlayer = new ID("{F0C25F85-7F6D-44E1-9DF8-86E6A7EE7039}");
        }

        public static class MediaElement
        {
            public static readonly ID Id = new ID("{A5B025C5-2F6B-45AF-BEE6-92883D9291D6}");
            public static readonly ID Name = new ID("{437A7D9F-7A50-4955-B3AE-583EBA5AD35A}");
            public static readonly ID ReferenceId = new ID("{A6D7AA5F-97DF-44C7-9AA4-4C7430E2DE2A}");
            public static readonly ID ThumbnailUrl = new ID("{2ECDE5D3-BFDE-4A5F-8D20-7C6DBA2BBFA0}");
            public static readonly ID ShortDescription = new ID("{2AF799D0-C019-4518-B166-0513C37218E8}");
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

            public static readonly ID CreationDate = new ID("{00270BAC-F30A-4B54-8AA6-614159522F5B}");
            public static readonly ID LongDescription = new ID("{42136A7A-FCA2-4F5F-836A-9EC2A0DA2534}");
            public static readonly ID PublishedDate = new ID("{4B223FFD-5972-4A8D-8CF7-08793E99BCA2}");
            public static readonly ID LastModifiedDate = new ID("{87E30397-7707-41D5-A4C9-500B01D80F5E}");
            public static readonly ID Economics = new ID("{22579C0B-2BB7-47EB-AE54-39CC8D9899C5}");
            public static readonly ID LinkUrl = new ID("{385F26A8-80DD-4408-A090-5D8A3A759775}");
            public static readonly ID LinkText = new ID("{FBB25ADD-22F8-4D4A-88A5-F648528FD299}");
            public static readonly ID Tags = new ID("{23250E7A-1548-4D0E-8870-957535235EE4}");
            public static readonly ID VideoStillUrl = new ID("{161A7C32-673B-43DA-9D1F-9D2A2226FCD6}");
            public static readonly ID Length = new ID("{EA3EE5CA-7545-4489-AE20-FF8C15EB98CB}");
            public static readonly ID PlaysTotal = new ID("{DB3D6769-ED9E-47F8-BD50-ABBA2C10AA1A}");
            public static readonly ID PlaysTrailingWeek = new ID("{54882903-2021-4C94-96DC-296CE1F8EEB0}");
            public static readonly ID CustomFields = new ID("{1CE3B5F5-FCEF-49C4-88D4-F5FB335E1C74}");
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
            public static readonly ID Id = new ID("{6A3B39AF-C59A-4E3F-86C8-EB0E846CF6E6}");
            public static readonly ID PlaylistType = new ID("{CF791C1B-A50C-422F-8369-7A1B9D823705}");
            public static readonly ID AutoStart = new ID("{31E644DE-B18A-4972-96A9-158747B8D79C}");
            public static readonly ID BackgroundColor = new ID("{5611864D-5C61-40B2-A423-F522BB85126F}");
            public static readonly ID WMode = new ID("{E5590C27-6645-4062-9072-70B2F188F23B}");
        }

        public static class PlayerList
        {
            public static readonly ID PlaylistType = new ID("{70059B46-11D1-45F3-B92E-45DF21AF8310}");
            public static readonly ID TagInclusion = new ID("{878C3F5B-303E-4F38-B42E-98F107372440}");
            public static readonly ID FilterTags = new ID("{C2B3AEB0-5CA9-4E2B-A75C-D9E5261BA0C1}");
            public static readonly ID VideoIds = new ID("{1526509E-D99A-48FB-8CCA-246B388B83FB}");
        }

        public static class Tag
        {
            public static readonly ID Name = new ID("{24FD815E-EBCE-4D16-90C1-62C66CEC95F2}");
        }
    }
}