using Markdig;
using Microsoft.Win32;
using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text;

namespace MarkdownPreview.Core {
	public static class Engine {
		/// <summary>
		/// Converts MarkDown into Html
		/// </summary>
		/// <param name="markdown">MarkDown content</param>
		/// <returns>Html</returns>
		public static string MarkDownToHtml(string markdown) {
			// Run parser
			var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
			string text = Markdown.ToHtml(markdown, pipeline);

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
			return html;
		}

		private static readonly Lazy<string> _Css = new Lazy<string>(GetCss);
		private static string Css {
			get {
				return _Css.Value;
			}
		}

		/// <summary>
		/// Gets the CSS.
		/// </summary>
		/// <returns></returns>
		public static string GetCss() {
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = $"{assembly.GetName().Name}.markdownpad-github.css";

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream)) {
				var sb = new StringBuilder(reader.ReadToEnd());

				if (IsDarkMode()) {

					sb.Append(@"
/* Dark mode
=============================================================================*/

body {
  background-color: #202020
}

body, h1, h2, h3, h4, h5
  color: #fff;
");
				}

				var result = sb.ToString();
				return result;
			}

		}

		/// <summary>
		/// Determines if the Windows 10 OS is currently in dark mode
		/// </summary>
		/// <returns></returns>
		/// <remarks>
		/// Obtained from <a href="https://stackoverflow.com/a/51336913/201019">Stack Overflow - how-to-detect-windows-10-light-dark-mode-in-win32-application</a>
		/// <br/>
		/// Implemented as part of <a href="https://github.com/Atrejoe/MarkdownPreview/issues/8">#8</a></remarks>
		public static bool IsDarkMode() {

			try {

				var result = Registry.GetValue($@"{Registry.LocalMachine}\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", null);

				if (!(result is int))
					result = Registry.GetValue($@"{Registry.CurrentUser}\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", null);

				if (result is int IsLightMode)
					return IsLightMode == 0;
			}
			catch (IOException) {
				//todo: log/display, then return default
				throw;
			}
			catch (SecurityException) {
				//todo: log/display, then return default
				throw;
			}
			catch (ArgumentException) {
				//todo: log/display, then return default
				throw;
			}        

			//Assume false, so light theme
			return false;
		}
	}
}
