using Markdig;
using System;
using System.IO;
using System.Reflection;

namespace MarkdownPreview.Core
{
	public static class Engine
	{
		/// <summary>
		/// Converts MarkDown into Html
		/// </summary>
		/// <param name="markdown">MarkDown content</param>
		/// <returns>Html</returns>
		public static string MarkDownToHtml(string markdown)
		{
			// Run parser
			string text = Markdown.ToHtml(markdown);

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
