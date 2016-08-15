using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Sitecore.Data.Items;
using RestSharp;
using RestSharp.Authenticators;

namespace Sitecore.MediaFramework.Brightcove.Security
{
  public class BrightcoveAthenticator : IAuthenticator
  {
    public readonly string PublishedId;
    public readonly string ReadToken;
    public readonly string WriteToken;

    public BrightcoveAthenticator(Item accountItem)
    {
      this.PublishedId = accountItem[FieldIDs.Account.PublisherId];
      this.ReadToken = accountItem[FieldIDs.Account.ReadToken];
      this.WriteToken = accountItem[FieldIDs.Account.WriteToken];
    }

    public virtual void Authenticate(IRestClient client, IRestRequest request)
    {
      if (request.Method == Method.GET)
      {
        request.AddUrlSegment("token", this.ReadToken);
      }
      else
      {
        var body = request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);
        if (body != null && body.Value != null)
        {
          string boundary = DateTime.Now.Ticks.ToString("x");

          body.Name = "multipart/form-data; boundary=-----------------------------" + boundary;

          body.Value = GetMultipart(boundary, body.Value.ToString());

          if (request.Files.Count == 0)
          {
            body.Value = body.Value + GetFooter(boundary);
          }
          else
          {
            using (MemoryStream ms = new MemoryStream())
            {
              WriteStringTo(ms, (string)body.Value);

              foreach (var file in request.Files)
              {
                WriteStringTo(ms, GetMultipartFileHeader(boundary, file.FileName));
                file.Writer(ms);
                WriteStringTo(ms, "\r\n\r\n");
              }

              request.Files.Clear();

              WriteStringTo(ms, GetFooter(boundary));

              body.Value = ms.ToArray();
            }
          }
        }
      }
      //client.Proxy = new WebProxy("http://127.0.0.1:8888");
    }

    protected static void WriteStringTo(Stream stream, string toWrite)
    {
      byte[] bytes = Encoding.UTF8.GetBytes(toWrite);
      stream.Write(bytes, 0, bytes.Length);
    }

    protected static string GetFooter(string boundary)
    {
      return string.Format("-------------------------------{0}--\r\n", boundary);
    }

    protected static string GetMultipart(string boundary, string value)
    {
      return string.Format("-------------------------------{0}\r\nContent-Disposition: form-data; name=\"json\"\r\n\r\n{1}\r\n\r\n", boundary, value);
    }

    protected static string GetMultipartFileHeader(string boundary,string fileName)
    {
      return string.Format("-------------------------------{0}\r\nContent-Disposition: form-data; name=\"file\"; filename=\"{1}\";\r\nContent-Type: application/octet-stream\r\n\r\n", boundary, fileName); ;
    }
  }
}