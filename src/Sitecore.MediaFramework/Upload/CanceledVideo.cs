namespace Sitecore.MediaFramework.Upload
{
  using Sitecore.Data;

  public class CanceledVideo
  {
    public string EntityId { get { return string.Empty; } }

    public string EntityName { get { return string.Empty; } }

    public virtual ID TemplateId { get { return ID.Null; } }
  }
}