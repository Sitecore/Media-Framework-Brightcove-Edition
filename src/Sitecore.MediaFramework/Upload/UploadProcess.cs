namespace Sitecore.MediaFramework.Upload
{
  using System.Collections.Specialized;

  using Sitecore.Data;

  public class UploadProcess
  {
    private readonly NameValueCollection properties;

    private readonly byte[] bytes;

    public UploadProcess(NameValueCollection properties, byte[] bytes)
    {
      this.properties = properties;
      this.bytes = bytes;
    }

    public virtual void Execute()
    {
      IUploadExecuter executer = MediaFrameworkContext.GetUploadExecuter(ID.Parse(this.properties[Constants.Upload.AccountTemplateId]));
      if (executer != null)
      {
        executer.Upload(this.properties, this.bytes);
      }
    }
  }
}