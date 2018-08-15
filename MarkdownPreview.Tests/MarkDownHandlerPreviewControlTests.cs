using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace MarkdownPreview.Tests
{
	/// <summary>
	/// Contains all tests for <see cref="MarkdownHandlerPreviewControl"/>
	/// </summary>
	[TestClass]
	public class MarkdownHandlerPreviewControlTests
	{
		/// <summary>
		/// Tests <see cref="MarkdownHandlerPreviewControl.GetCss"/>
		/// </summary>
		[TestMethod]
		public void GetCssTest()
		{
			//Arrange

			//Act
			var actual = MarkdownHandlerPreviewControl.GetCss();

			//Assert
			Assert.IsFalse(string.IsNullOrEmpty(actual), "result of getting CSS was null or empty");

			Trace.Write(actual);
		}

		/// <summary>
		/// Tests <see cref="MarkdownHandlerPreviewControl.MarkDownToHtml(string)"/>
		/// </summary>
		[TestMethod()]
		public void MarkDownToHtmlTest()
		{
			//arrange
			var markdown = @"# Hello
this is a paragraph with a [link](https://github.com/Atrejoe/MarkdownPreview)";
			//act

			var html = MarkdownHandlerPreviewControl.MarkDownToHtml(markdown);

			//assert
			Assert.IsFalse(string.IsNullOrWhiteSpace(html));

			Trace.Write(html);
		}
	}
}
