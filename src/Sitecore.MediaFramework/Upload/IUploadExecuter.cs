namespace Sitecore.MediaFramework.Upload
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;

  public interface IUploadExecuter
  {
    List<string> FileExtensions { get; }

    bool SupportCanceling { get; set; }

    void Upload(NameValueCollection parameters, byte[] fileBytes);

    void Cancel(Guid fileId, Guid accountId);
  }
}