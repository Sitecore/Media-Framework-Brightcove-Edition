namespace Sitecore.MediaFramework.Pipelines.RenderLayout
{
    using System.Collections.Generic;
    using System.Text;
    using System.Web.UI;

    using Sitecore.Diagnostics;
    using Sitecore.Pipelines.RenderLayout;

    /// <summary>                                 
    /// Insert Head Controls
    /// </summary>
    public class InsertHeadControls : RenderLayoutBase
    {
        public InsertHeadControls()
        {
            this.JavascriptUrls = new List<string>();
            this.CssUrls = new List<string>();
        }

        /// <summary>
        /// Javascript Urls
        /// </summary>
        public List<string> JavascriptUrls { get; set; }

        public List<string> CssUrls { get; set; }

        public override void Render(RenderLayoutArgs args)
        {
            Control root = this.RootControlResolver != null ? this.RootControlResolver.GetRootControl() : null;
            if (root != null)
            {
                foreach (var u in JavascriptUrls)
                {
                    root.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), u, string.Format(@"<script language=""javascript"" type=""text/javascript"" src=""{0}"" ></script>", u), false);
                }
            }
        }

        /// <summary>
        /// Add Javascript Url
        /// </summary>
        /// <param name="fileUrl"></param>
        public virtual void AddJavascriptUrl(string fileUrl)
        {
            if (!string.IsNullOrEmpty(fileUrl) && !this.JavascriptUrls.Contains(fileUrl))
            {
                this.JavascriptUrls.Add(fileUrl);
            }
        }

        /// <summary>
        /// Add CSS Url
        /// </summary>
        /// <param name="fileUrl"></param>
        public virtual void AddCssUrl(string fileUrl)
        {
            if (!string.IsNullOrEmpty(fileUrl) && !this.CssUrls.Contains(fileUrl))
            {
                this.CssUrls.Add(fileUrl);
            }
        }

        protected override Control GetControlToRender()
        {
            var builder = new StringBuilder();

            this.AddJavascriptToOutput(builder);

            this.AddCssToOutput(builder);

            return new LiteralControl(builder.ToString());
        }

        /// <summary>
        ///  Add Javascript To Output
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AddJavascriptToOutput(StringBuilder builder)
        {
            Assert.ArgumentNotNull(builder, "builder");

            foreach (var fileUrl in this.JavascriptUrls)
            {
                builder.AppendFormat("<script language=\"javascript\" type=\"text/javascript\" src=\"{0}\" ></script>\n", fileUrl);
            }
        }

        /// <summary>
        ///  Add CSS To Output
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void AddCssToOutput(StringBuilder builder)
        {
            Assert.ArgumentNotNull(builder, "builder");

            foreach (var fileUrl in this.CssUrls)
            {
                builder.AppendFormat("<link rel=\"Stylesheet\" href=\"{0}\" />\n", fileUrl);
            }
        }
    }
}