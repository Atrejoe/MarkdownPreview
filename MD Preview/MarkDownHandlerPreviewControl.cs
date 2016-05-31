using MarkdownSharp;
using SharpShell.SharpPreviewHandler;
using System;
using System.IO;
using System.Windows.Forms;

namespace MarkDownPreview
{
    /// <summary>
    /// Displays content of a file as MarkDown
    /// </summary>
    /// <seealso cref="PreviewHandlerControl" />
    public partial class MarkDownHandlerPreviewControl : PreviewHandlerControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkDownHandlerPreviewControl"/> class.
        /// </summary>
        public MarkDownHandlerPreviewControl()
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

                var releaseRemarks = "Original version, with enable visual styles";

                //Insert the html into the browser
                var html = $@"<!DOCTYPE html>
<html>
    <head>
        <title>Preview pane rendered at {DateTime.Now}</title>
        <style>body {{
                color:black;
                background-color:whitesmoke;
            }}</style>
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
    }
}
