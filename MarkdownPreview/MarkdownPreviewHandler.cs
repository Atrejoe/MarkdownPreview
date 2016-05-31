using SharpShell.Attributes;
using SharpShell.SharpPreviewHandler;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MarkdownPreview
{
    /// <summary>
    /// Handler that treats text content of a file a MarkDown
    /// </summary>
    /// <seealso cref="SharpPreviewHandler" />
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".md")]
    [DisplayName("MarkDown Preview Handler")]
    [PreviewHandler]
    public class MarkdownPreviewHandler : SharpPreviewHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownPreviewHandler"/> class, enabling visual styles
        /// </summary>
        public MarkdownPreviewHandler() {
            Application.EnableVisualStyles();
        }

        /// <summary>
        /// DoPreview must create the preview handler user interface and initialize it with data
        /// provided by the shell.
        /// </summary>
        /// <returns>
        /// The preview handler user interface.
        /// </returns>
        protected override PreviewHandlerControl DoPreview()
        {
            //  Create the handler control.
            var handler = new MarkdownHandlerPreviewControl();

            //  Do we have a file path? If so, we can do a preview.
            if (!string.IsNullOrEmpty(SelectedFilePath))
                handler.DoPreview(SelectedFilePath);

            //  Return the handler control.
            return handler;
        }
    }
}
