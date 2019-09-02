using SharpShell.Attributes;
using SharpShell.SharpPreviewHandler;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarkdownPreview {
	/// <summary>
	/// Handler that treats text content of a file a MarkDown
	/// </summary>
	/// <seealso cref="SharpPreviewHandler" />
	[ComVisible(true)]
	[COMServerAssociation(AssociationType.ClassOfExtension, ".md", ".markdown")]
	[DisplayName("MarkDown Preview Handler")]
	[PreviewHandler]
	public class MarkdownPreviewHandler : SharpPreviewHandler {
		/// <summary>
		/// Initializes a new instance of the <see cref="MarkdownPreviewHandler"/> class, enabling visual styles
		/// </summary>
		public MarkdownPreviewHandler() {
			Log($"Constructor called");

			Application.EnableVisualStyles();
		}

		/// <summary>
		/// DoPreview must create the preview handler user interface and initialize it with data
		/// provided by the shell.
		/// </summary>
		/// <returns>
		/// The preview handler user interface.
		/// </returns>
		protected override PreviewHandlerControl DoPreview() {

			try {
				//  Create the handler control.
			var handler = new MarkdownHandlerPreviewControl();

			handler.VerticalScroll.Enabled = true;

			Action DoPreview = () => {
				Log($"Doing preview");

				//  Do we have a file path? If so, we can do a preview.
				if (!string.IsNullOrEmpty(SelectedFilePath))
					try {
						handler.DoPreview(SelectedFilePath);
					}
#pragma warning disable CA1031 // Do not catch general exception types
					catch (Exception ex) {
#pragma warning restore CA1031 // Do not catch general exception types
						LogError($"Preview failure {ex.Message}", ex);
						throw;
					}
			};

			Log($"Running preview with delay");
			Task.Delay(100).ContinueWith(t => DoPreview(), TaskScheduler.Default);
			//DoPreview();

			//  Return the handler control.

			Log($"Returning handler");
			Log($"Doing preview");

			return handler;
			}
#pragma warning disable CA1031 // Do not catch general exception types
			catch (Exception ex) {
#pragma warning restore CA1031 // Do not catch general exception types
				LogError($"Preview failure {ex.Message}", ex);
				throw;
			}
		}
	}
}
