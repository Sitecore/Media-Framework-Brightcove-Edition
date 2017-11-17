
namespace Sitecore.MediaFramework.Pipelines.GetContentEditorWarnings
{
  using Sitecore.Diagnostics;
  using Sitecore.Globalization;
  using Sitecore.MediaFramework.Export;
  using Sitecore.Pipelines.GetContentEditorWarnings;

  public class NotExported
  {
    public void Process(GetContentEditorWarningsArgs args)
    {
      Assert.ArgumentNotNull(args, "args");

      if (args.Item != null)
      {
        IExportExecuter exportExecuter = MediaFrameworkContext.GetExportExecuter(args.Item);
        if (exportExecuter != null && exportExecuter.IsNew(args.Item))
        {
          args.Add(Translate.Text(Translations.NotExportedWarningTitle), Translate.Text(Translations.NotExportedWarningText));
        }
      }
    }
  }
}