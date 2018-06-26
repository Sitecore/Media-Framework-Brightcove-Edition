<%@ Page Language="C#" AutoEventWireup="true" Inherits="Sitecore.MediaFramework.UI.Sublayouts.Upload,Sitecore.MediaFramework" %>
<!DOCTYPE HTML>
<html lang="en">
<head>
<meta charset="utf-8">
<meta http-equiv="x-ua-compatible" content="IE=edge,chrome=1" >
<title>MediaFramework Uploader</title>
<link rel="stylesheet" href="/sitecore modules/Web/MediaFramework/CSS/Uploader/bootstrap.min.css">
<link rel="stylesheet" href="/sitecore modules/Web/MediaFramework/CSS/Uploader/bootstrap-ie6.min.css">
<link rel="stylesheet" href="/sitecore modules/Web/MediaFramework/CSS/Uploader/bootstrap-responsive.min.css">
 
<!-- Bootstrap CSS fixes for IE6 -->
<!--[if lt IE 7]>
<link rel="stylesheet" href="http://blueimp.github.io/cdn/css/bootstrap-ie6.min.css">
<![endif]-->

<link rel="stylesheet" href="/sitecore modules/Web/MediaFramework/CSS/Uploader/jquery.fileupload-ui.css"> 
<link rel="stylesheet" href="/sitecore modules/Web/MediaFramework/CSS/Uploader/mfUploader.css"> 
<!-- Demo styles -->
<style>

    body
    {
        background:white;
    }

</style>
     
</head>

<body>
   <div class="uploader">
    <div class="container">
    <!-- The file upload form used as target for the file upload widget -->
    <form id="fileupload" action="/" method="POST" runat="server" enctype="multipart/form-data">
        <!-- Redirect browsers with JavaScript disabled to the origin page -->
        <noscript><input type="hidden" name="redirect" value="http://blueimp.github.io/jQuery-File-Upload/"></noscript>
        <!-- The fileupload-buttonbar contains buttons to add/delete files and start/cancel the upload -->
        <asp:HiddenField ID="wrongExtension" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="TextGoTo" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="TextSelectItem" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="AccountMenuWarn" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="UploadingCanceled" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="EmptyResult" ClientIDMode="Static" runat="server" />
        <asp:HiddenField ID="Buffered" ClientIDMode="Static" runat="server" />
        <table>
            <tr>
                <td class="left-menu">
                    <h4 id="LeftMenuTitle"><asp:Literal runat="server" ID="ltrAccounts"/></h4>
                    <ul class="row fileupload-accounts"></ul>
                </td>
                <td class="upload-box">
                    <div class="row fileupload-buttonbar">
                        <div class="span7">
                            <!-- The fileinput-button span is used to style the file input field as button -->
                            <span class="btn btn-success fileinput-button">
                            
                                <span><asp:Literal runat="server" ID="ltrAddFiles"/></span>
                                <input type="file" name="files[]" multiple>
                            </span>
                            <button type="submit" class="btn btn-primary start">
                             
                                <span><asp:Literal runat="server" ID="ltrStartUpload"/></span>
                            </button>
                         </div>
                    </div>
                     
                    <div>    
                        <!-- The table listing the files available for upload/download -->
                        <table role="presentation" class="table table-striped"><tbody class="files"></tbody></table>
                    </div>
                </td>
            </tr>
        </table> 
        <asp:HiddenField ID="PageData" ClientIDMode="Static" runat="server" />
    </form>
    <br>
</div>
    
    </div>
     <label id="errorLabel" class="label-error"></label>
    
    <script id="template-accounts" type="text/template">
        
           {%for (var i=0, prov; prov=o.accounts[i]; i++) { %}
                <hr />
                {%for (var j=0, acc; acc=prov[j]; j++) { %}
        
                    <li>
                        <input type="checkbox" class="account-box" template={%= acc.accountTemplateId%} accId={%= acc.id%} {%= acc.selected?'checked="checked"':''%} />
                        <span>{%=acc.name%}</span>
                    </li>
                {%}}%} 
    
    </script>
     
             <script id="template-upload" type="text/template"> 
                 {% for (var i=0, file; file=o.files[i]; i++) { %} 
                 <tr class="template-upload fade">
                 <td class="text">
                     <p class="name">{%=file.name?file.name:''%}</p>
                         {% if (file.error) { %}
                            <div><span class="label label-important"><asp:Literal runat="server" ID="ltrError"/></span> {%=file.error%}</div>
                         {% } %}
                 </td>
                      {% if (!file.error) { %}
                             <td class="upload-process">
                                 <div class="row">
                                    <div class="span1 col">
                                         <span>Size : </span>
                                    </div>
                                    <div class="span2 col">
                                         <p class="size">{%=o.formatFileSize(file.size)%}</p>
                                    </div>
                                    <div class="span3 buttons">
                                          {% if (!o.options.autoUpload) { %}
                                             <button class="btn btn-primary start"> 
                                                 <asp:Literal runat="server" ID="ltrStart"/>
                                             </button>
                                             {% } %} 
                                    </div>
                                 </div>
                                 <div class="row buffering">
                                    <div class="span1 col">
                                       <span><asp:Literal runat="server" ID="ltrBuffering"/> : </span>
                                    </div>
                                    <div class="span2 col" >
                                       <div class="progress progress-success progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="0"><div class="bar" style="width:0%;"></div></div> 
                                    </div>
                                    <div class="span3 col buttons">
                                        <button class="btn btn-warning cancel">
                                            <asp:Literal runat="server" ID="ltrCancel"/>
                                        </button> 
                                    </div>
                                 </div>
                             </td> 
                           
                            {% } %}
                             </tr>
                            {% } %}     
             </script>
   
    <script  id="template-upload-done" type="text/template"> 
      <div class="span1 col"> <p class="name">{%=o.status.label%} </p></div> 
      <div class="span2 col"> <img width="38" class="done-image" src={%=o.status.image%} /> </div> 
      <div class="span3 col">{% if(o.status.error) { %}<span> {%=o.status.error%}</span>
          {%}if(!o.status.error && o.status.mode != 'buffered') { %}<button mode={%=o.status.mode%} mItemId={%=o.status.mediaItemId%} database={%=o.status.database%} class="btn goto">{%= (o.status.mode=='norm'? $('#TextGoTo').val() : $('#TextSelectItem').val() )%}</button>{%}%} 
      </div>  
    </script>
    
    <script  id="template-upload-start" type="text/template">
        <div class="row" accid={%=o.account.id%}>
            <div class="span1 col progress-label">{%=o.account.name%}</div>
            <div class="span2 col">
                <div class="external-progress-box">
                   <div class="percents"> <img src="/sitecore/shell/Themes/Standard/Images/MediaFramework/Common/loading.gif" width="17" alt="upload"/>  <span class="external-progress-percents">0%</span></div>
                    <div class="external-progress progress-success progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100" aria-valuenow="20">
                        <div class="bar" style="width:0%;"></div>
                    </div>
               </div> 
            </div>
            <div class="span3 buttons">
            </div>
        </div>
    </script>
      
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/jquery.js" type="text/javascript"></script> 
    
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/underscore-min.js" type="text/javascript"></script> 
    
    <!-- The jQuery UI widget factory, can be omitted if jQuery UI is already included -->  
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/jquery.ui.widget.js" type="text/javascript"></script>

    <!-- The Templates plugin is included to render the upload/download listings -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/tmpl.min.js" type="text/javascript"></script>
        
    <!-- The Load Image plugin is included for the preview images and image resizing functionality -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/load-image.min.js" type="text/javascript"></script>

    <!-- The Canvas to Blob plugin is included for image resizing functionality -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/canvas-to-blob.min.js" type="text/javascript"></script>

    <!-- Bootstrap JS is not required, but included for the responsive demo navigation -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/bootstrap.min.js"></script>

    <!-- blueimp Gallery script -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/jquery.blueimp-gallery.min.js" type="text/javascript"></script>
    
    <!-- The Iframe Transport is required for browsers without support for XHR file uploads -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/jquery.iframe-transport.js"></script>
    
        <!-- The File Upload image preview & resize plugin -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/jquery.fileupload.js" type="text/javascript"></script>

    <!-- The File Upload processing plugin -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/jquery.fileupload-process.js" type="text/javascript"></script>    

    <!-- The File Upload image preview & resize plugin -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/jquery.fileupload-image.js" type="text/javascript"></script>

        <!-- The File Upload image preview & resize plugin -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/jquery.fileupload-audio.js" type="text/javascript"></script>

    <!-- The File Upload image preview & resize plugin -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/jquery.fileupload-video.js" type="text/javascript"></script>
        <!-- The basic File Upload plugin -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/jquery.fileupload-ui.js" type="text/javascript"></script>

    <!-- The MF Upload script -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/MFUpload.js" type="text/javascript"></script>

    <!-- The main application script -->
    <script src="/sitecore modules/Web/MediaFramework/js/Uploader/main.js" type="text/javascript"></script>
  
 
 </body> 
</html>
