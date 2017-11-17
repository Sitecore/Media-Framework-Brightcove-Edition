var SMFRTECommandLine = window.RadEditorCommandList;
if ( typeof(SMFRTECommandLine)==="undefined" ) {
	SMFRTECommandLine = window.Telerik.Web.UI.Editor.CommandList;
}
SMFRTECommandLine["EmbedMedia"] = function (commandName, editor, args) {
    var id;

    // inserted media in form of <img src="~/media/CC2393E7CA004EADB4A155BE4761086B.ashx" />
    // if (!id) {
    //id = GetMediaID(html);
    //}

    scEditor = editor;

    editor.showExternalDialog(
      "/sitecore/shell/default.aspx?xmlcontrol=MediaFramework.EmbedMedia&la=" + scLanguage + (id ? "&fo=" + id : "") + (scDatabase ? "&databasename=" + scDatabase : ""),
      null, //argument
      1000,
      600,
      scInsertSitecoreMedia,
      null,
      "Insert Media",
      true, //modal
      Telerik.Web.UI.WindowBehaviors.Close, // behaviors
      false, //showStatusBar
      false //showTitleBar
    );
};

SMFRTECommandLine["EmbedLink"] = function (commandName, editor, args) {
  var link = encodeURIComponent(editor.getSelectionHtml());
  var id;

  scEditor = editor;

  editor.showExternalDialog(
    "/sitecore/shell/default.aspx?xmlcontrol=MediaFramework.EmbedLink&la=" + scLanguage + (id ? "&fo=" + id : "") + (scDatabase ? "&databasename=" + scDatabase : "") + "&link=" + link,
    null, //argument
   1000,
    600,
    scInsertSitecoreMedia,
    null,
    "Insert Link",
    true, //modal
    Telerik.Web.UI.WindowBehaviors.Close, // behaviors
    false, //showStatusBar
    false //showTitleBar
  );
};