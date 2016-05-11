using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Utils;

namespace UnitTest.Common.Utils
{
    [TestClass]
    public class UnitTestGuard
    {
        [TestMethod]
        public void Guard_ArgumentNotNull_Success()
        {
            var obj = new object();
            Guard.ArgumentNotNull(obj, "obj");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Guard_ArgumentNotNull_Fail()
        {
            try
            {
                object obj = null;
                Guard.ArgumentNotNull(obj, "obj");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Parameter obj cannot be null.\r\nParameter name: obj", ex.Message);
                throw;
            }           
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Guard_ArgumentNotNull_FailWithMessage()
        {
            try
            {
                object obj = null;
                Guard.ArgumentNotNull(obj, "obj", "message");
                Assert.Fail();
            }
            catch(Exception ex)
            {
                Assert.AreEqual("message\r\nParameter name: obj", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void Guard_ArgumentNotNullOrEmpty_Success()
        {
            var str = "abc";
            Guard.ArgumentNotNullOrEmpty(str, "str");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Guard_ArgumentNotNullOrEmpty_Fail()
        {
            try
            {
                string str = "";
                Guard.ArgumentNotNullOrEmpty(str, "str");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Parameter str cannot be null or empty.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Guard_ArgumentNotNullOrEmpty_FailWithMessage()
        {
            try
            {
                string str = "";
                Guard.ArgumentNotNullOrEmpty(str, "str", "message");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("message", ex.Message);
                throw;
            }
        }

        [TestMethod]
        public void Guard_InstanceOfType_Success()
        {
            var obj = "str";
            Guard.InstanceOfType(obj, "obj", typeof(string));

            var objType = typeof (string);
            Guard.InstanceOfType(objType, "obj", typeof (string));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Guard_InstanceOfType_Fail()
        {
            try
            {
                string str = "1234";
                Guard.InstanceOfType(str, "str", typeof(int));
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Parameter str should be type of System.Int32.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Guard_InstanceOfType_Fail2()
        {
            try
            {
                var objType = typeof (string);
                Guard.InstanceOfType(objType, "obj", typeof(int));
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("Parameter obj should be type of System.Int32.", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Guard_InstanceOfType_FailWithMessage()
        {
            try
            {
                string str = "1234";
                Guard.InstanceOfType(str, "str", typeof(int), "message");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("message", ex.Message);
                throw;
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Guard_InstanceOfType_Fail2WithMessage()
        {
            try
            {
                var objType = typeof(string);
                Guard.InstanceOfType(objType, "obj", typeof(int), "message");
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual("message", ex.Message);
                throw;
            }
        }
    }
}
