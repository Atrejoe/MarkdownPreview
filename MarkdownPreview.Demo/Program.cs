using System;
using System.Windows.Forms;

namespace MarkdownPreview.Demo {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			using (var frm = new Form()) {
				Application.Run(frm);
			}
		}
	}
}
