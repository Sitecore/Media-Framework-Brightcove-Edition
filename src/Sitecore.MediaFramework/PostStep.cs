namespace Sitecore.MediaFramework
{
  using System;
  using System.Collections.Generic;
  using System.Collections.Specialized;
  using System.IO;
  using System.Linq;

  using Sitecore.Configuration;
  using Sitecore.Data.Managers;
  using Sitecore.Diagnostics;
  using Sitecore.IO;
  using Sitecore.Install.Framework;
  using Sitecore.Jobs;
  using Sitecore.MediaFramework.Diagnostics;
  using Sitecore.Shell.Applications.Globalization.ImportLanguage;

  public class PostStep : IPostStep
  {
    public void Run(ITaskOutput output, NameValueCollection metaData)
    {
      this.ImportTranslation();
    }

    protected void ImportTranslation()
    {
      List<string> languages = this.GetClientLanguages();

      foreach (string filePath in this.GetTranslationFiles())
      {
        foreach (string db in new [] { "core", "master" })
        {
          JobOptions options = new JobOptions("ImportLanguage", "ImportLanguage", "shell", new ImportLanguageForm.Importer(db, filePath, languages), "Import")
            {
              ContextUser = Context.User
            };
          Job job = JobManager.Start(options);
          job.Wait();
        }
        try
        {
          File.Delete(filePath);
        }
        catch(Exception ex)
        {
          LogHelper.Error("Error during removing processed translation file", this, ex);
        }
      }
    }

    protected string[] GetTranslationFiles()
    {
      string path = Path.Combine(FileUtil.MapPath("temp"), "MediaFramework Translations");
      
      if (Directory.Exists(path))
      {
        return Directory.GetFiles(path);
      }

      return new string[0];
    }

    protected List<string> GetClientLanguages()
    {
      return LanguageManager.GetLanguages(Factory.GetDatabase("core")).Select(lang => lang.Name).ToList();
    }
  }
}