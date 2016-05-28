using SharpShell.Attributes;
using SharpShell.SharpPreviewHandler;
using System.Runtime.InteropServices;

namespace MarkDownPreview
{
    /// <summary>
    /// Handler that treats text content of a file a MarkDown
    /// </summary>
    /// <seealso cref="SharpShell.SharpPreviewHandler.SharpPreviewHandler" />
    [ComVisible(true)]
    [COMServerAssociation(AssociationType.ClassOfExtension, ".md")]
    [DisplayName("MarkDown Preview Handler")]
    [PreviewHandler]
    public class MarkDownPreviewHandler : SharpPreviewHandler
    {
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
            var handler = new MarkDownHandlerPreviewControl();

            //  Do we have a file path? If so, we can do a preview.
            if (!string.IsNullOrEmpty(SelectedFilePath))
                handler.DoPreview(SelectedFilePath);

            //  Return the handler control.
            return handler;
        }
    }
}
