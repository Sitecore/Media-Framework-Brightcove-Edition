
namespace Sitecore.MediaFramework.Mvc.IO
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;

  using Sitecore.Diagnostics;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.MediaFramework.Mvc.Text;
  using Sitecore.Mvc.Presentation;

  public class HtmlUpdateFilter : Stream
  {
    protected readonly MemoryStream InternalStream;

    protected readonly IHtmlUpdater[] Updaters;

    public override bool CanRead { get { return true; } }

    public override bool CanSeek { get { return true; } }

    public override bool CanWrite { get { return true; } }

    public override long Length { get { return 0; } }

    public override long Position { get; set; }

    public Stream ResponseStream { get; protected set; }

    public virtual Encoding Encoding
    {
      get
      {
        PageContext currentOrNull = PageContext.CurrentOrNull;
        return currentOrNull != null ? currentOrNull.RequestContext.HttpContext.Response.ContentEncoding : Encoding.UTF8;
      }
    }

    public HtmlUpdateFilter(Stream stream, IEnumerable<IHtmlUpdater> updaters)
    {
      Assert.ArgumentNotNull(stream, "stream");
      Assert.ArgumentNotNull(updaters, "updaters");

      this.ResponseStream = stream;
      this.InternalStream = new MemoryStream();
      this.Updaters = updaters.ToArray();
    }

    public override void Flush()
    {
      byte[] data = this.InternalStream.ToArray();

      if (this.Updaters.Length > 0)
      {
        StringBuilder builder;
        string html = this.Encoding.GetString(data);
        if (this.TryUpdateHtml(html, out builder))
        {
          data = this.Encoding.GetBytes(builder.ToString());
        }
      }

      this.TransmitData(data);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
      return this.ResponseStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
      return this.ResponseStream.Seek(offset, origin);
    }

    public override void SetLength(long length)
    {
      this.ResponseStream.SetLength(length);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
      this.InternalStream.Write(buffer, offset, count);
    }

    protected virtual void TransmitData(byte[] data)
    {
      this.ResponseStream.Write(data, 0, data.Length);
      this.ResponseStream.Flush();
      this.InternalStream.SetLength(0L);
      this.Position = 0L;
    }

    protected virtual bool TryUpdateHtml(string html, out StringBuilder builder)
    {
      builder = new StringBuilder(html);

      bool result = false;

      foreach (IHtmlUpdater updater in this.Updaters)
      {
        try
        {
          result = updater.UpdateHtml(builder) | result;
        }
        catch (Exception exception)
        {
          LogHelper.Error("Error during injecting html", this, exception);
        }
      }

      return result;
    }
  }
}