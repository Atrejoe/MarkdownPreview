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
    }
}
