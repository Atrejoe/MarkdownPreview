using MarkdownPreview.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace MarkdownPreview.Tests
{
	/// <summary>
	/// Contains all tests for <see cref="Engine"/>
	/// </summary>
	[TestClass]
	public class EngineTests
	{
		/// <summary>
		/// Tests <see cref="Engine.GetCss"/>
		/// </summary>
		[TestMethod]
		public void GetCssTest()
		{
			//Arrange
			//Act
			var actual = Engine.GetCss();

			//Assert
			Assert.IsFalse(string.IsNullOrEmpty(actual), "result of getting CSS was null or empty");

			Trace.Write(actual);
		}

		/// <summary>
		/// Tests <see cref="Engine.MarkDownToHtml(string)"/>
		/// </summary>
		[TestMethod()]
		public void MarkDownToHtmlTest()
		{
			//arrange
			var markdown = @"# Hello
this is a paragraph with a [link](https://github.com/Atrejoe/MarkdownPreview)";
			//act

			var html = Engine.MarkDownToHtml(markdown);

			//assert
			Assert.IsFalse(string.IsNullOrWhiteSpace(html));

			Trace.Write(html);
		}
	}
}
