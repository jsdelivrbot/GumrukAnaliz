using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gumruk.Web.Controllers;

namespace Gumruk.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var controller = new HomeController();
            int result = controller.AddModule("dana veli");

            Assert.AreNotEqual(0, result);
        }
    }
}
