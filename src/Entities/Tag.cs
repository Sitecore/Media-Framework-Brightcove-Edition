namespace Sitecore.MediaFramework.Brightcove.Entities
{
  public class Tag
  {
    public string Name { get; set; }

    public override string ToString()
    {
      return string.Format("(type:{0},name:{1})", this.GetType().Name, this.Name);
    }
  }
}