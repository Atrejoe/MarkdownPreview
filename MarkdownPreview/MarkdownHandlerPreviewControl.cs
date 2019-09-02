using SharpShell.SharpPreviewHandler;
using System;
using System.IO;
using System.Windows.Forms;
using static MarkdownPreview.Core.Engine;

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

				string html = MarkDownToHtml(content);

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

				#region Hack attempt 3
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
#pragma warning disable CA1031 // Meaning to catch all
			catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
			{
				//  Maybe we could show something to the user in the preview
				//  window, but for now we'll just ignore any exceptions.

				MessageBox.Show($"An error occurred while previewing a Markdown file, please report this at https://github.com/Atrejoe/MarkdownPreview/issues.{Environment.NewLine}{ex}");

				throw;
			}
		}

	}
}
