using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarkDownPreview;
using System.Diagnostics;

namespace MarkdownPreview.Tests
{
    /// <summary>
    /// Contains all tests for <see cref="MarkDownHandlerPreviewControl"/>
    /// </summary>
    [TestClass]
    public class MarkDownHandlerPreviewControlTests
    {
        /// <summary>
        /// Tests <see cref="MarkDownHandlerPreviewControl.GetCss"/>
        /// </summary>
        [TestMethod]
        public void GetCssTest()
        {
            //Arrange

            //Act
            var actual = MarkDownHandlerPreviewControl.GetCss();

            //Assert
            Assert.IsFalse(string.IsNullOrEmpty(actual), "result of getting CSS was null or empty");

            Trace.Write(actual);
        }
    }
}
