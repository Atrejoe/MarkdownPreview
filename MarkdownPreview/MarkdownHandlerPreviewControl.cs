using MarkdownSharp;
using SharpShell.SharpPreviewHandler;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MarkdownPreview
{
    /// <summary>
    /// Displays content of a file as MarkDown
    /// </summary>
    /// <seealso cref="PreviewHandlerControl" />
    public partial class MarkdownHandlerPreviewControl : PreviewHandlerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownHandlerPreviewControl"/> class.
        /// </summary>
        public MarkdownHandlerPreviewControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Performs the preview, transforming the content of <paramref name="selectedFilePath"/> into MarkDown and display it in a Browser control (in the preview pane)
        /// </summary>
        /// <param name="selectedFilePath">The selected file path.</param>
        public void DoPreview(string selectedFilePath)
        {
            try
            {
                var content = File.ReadAllText(selectedFilePath);

                // Create new markdown instance
                var mark = new Markdown();

                // Run parser
                string text = mark.Transform(content);

                var releaseRemarks = "Original version, with enable visual styles and stylesheet";

                //Insert the html into the browser
                var html = $@"<!DOCTYPE html>
<html>
    <head>
        <title>Preview pane rendered at {DateTime.Now}</title>
        <style>
{Css}
        </style>
    </head>
    <body>
    {text}
    </body>
    <!-- {releaseRemarks} -->
</html>";

                #region Display issues
                //WebBrowser component does have the proper source (rightclick > view source) but does not display anything

                #region Original

                webBrowser.DocumentText = html;
                webBrowser.Invalidate();

                #endregion

                #region Hack attempt 1
                //Fix using: http://weblogs.asp.net/gunnarpeipman/displaying-custom-html-in-webbrowser-control
                //webBrowser.Navigate("about:blank");
                //if (webBrowser.Document != null)
                //    webBrowser.Document.Write(html);
                #endregion

                #region Hack attempt 2
                //webBrowser.Navigate("about:blank");
                //while (webBrowser.Document == null || webBrowser.Document.Body == null)
                //    Application.DoEvents();
                //webBrowser.Document.OpenNew(true).Write(html);
                #endregion

                #region Hack attampt 3
                //webBrowser.Navigate("about:blank");
                //webBrowser.Document.OpenNew(false);
                //webBrowser.Document.Write(html);
                //webBrowser.Refresh();
                #endregion

                #region Hack Attempt 4 (various kinds of temp files)

                //var tempFile = new FileInfo(Path.GetTempFileName());
                //tempFile.Attributes = FileAttributes.Temporary;

                //File.WriteAllText(tempFile.FullName,html);

                //webBrowser.Url = new Uri(tempFile.FullName);

                #endregion

                #endregion

                //MessageBox.Show("Render complete");
            }
            catch (Exception ex)
            {
                //  Maybe we could show something to the user in the preview
                //  window, but for now we'll just ignore any exceptions.

                MessageBox.Show($"An error occurred while previewing a Markdown file, please report this at https://github.com/Atrejoe/MarkdownPreview/issues.{Environment.NewLine}{ex}");

            }
        }

        private static readonly Lazy<string> _Css = new Lazy<string>(GetCss);
        private static string Css
        {
            get
            {
                return _Css.Value;
            }
        }

        /// <summary>
        /// Gets the CSS.
        /// </summary>
        /// <returns></returns>
        public static string GetCss()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"{assembly.GetName().Name}.markdownpad-github.css";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }
}
