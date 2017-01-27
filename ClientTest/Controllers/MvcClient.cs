using Microsoft.VisualStudio.TestTools.UnitTesting;
using Relief.Express.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relief.Express.Mvc.Controllers.Tests
{
    [TestClass()]
    public class MvcClient
    {
        [TestMethod()]
        public void IndexTest()
        {
            //Landing page
            Assert.AreEqual(1, 1);
        }

        [TestMethod()]
        public void SecureTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetLabDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void stscallbackTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void LogoutTest()
        {
            Assert.Fail();
        }
    }
}