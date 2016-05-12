using System;
using System.Threading;
using Common.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Common.Caching
{
    /// <summary>
    /// The implementation of ICache should have exactlly same basic object caching behavior.
    /// The base class contains the basic object caching behavior testing cases.
    /// </summary>
    public abstract class UnitTestCacheBase
    {
        protected abstract ICache CreateCacheInstance(string name);
        protected abstract ICache CreateCacheInstance();

        protected ICache CacheInstance1;
        protected ICache CacheInstance2;

        [TestInitialize]
        public virtual void Initialize()
        {
            CacheInstance1 = CreateCacheInstance("cache1");
            CacheInstance2 = CreateCacheInstance("cache2");
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
            CacheInstance1.Dispose();
            CacheInstance2.Dispose();
        }

        protected static string GenerateKey()
        {
            return Guid.NewGuid().ToString();
        }

        [TestMethod]
        public void Name_Default()
        {
            var defaultCache = CreateCacheInstance();
            var name = defaultCache.Name;
            Assert.IsTrue(name != null && name.Contains("default"));
        }

        [TestMethod]
        public virtual void Cache_Add_ArgumentException()
        {
            try
            {
                CacheInstance1.Add("", "a");
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            try
            {
                CacheInstance1.Add<string>(GenerateKey(), null);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public virtual void Cache_Add_AbsoluteExpiration_ArgumentException()
        {
            try
            {
                CacheInstance1.Add("", "", DateTimeOffset.Now.AddSeconds(10));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            try
            {
                CacheInstance1.Add<string>(GenerateKey(), null, DateTimeOffset.Now.AddSeconds(10));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public virtual void Cache_Add_SlidingExpiration_ArgumentException()
        {
            try
            {
                CacheInstance1.Add("", "", TimeSpan.FromMilliseconds(1000));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            try
            {
                CacheInstance1.Add<string>(GenerateKey(), null, TimeSpan.FromMilliseconds(1000));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public virtual void Cache_Set_ArgumentException()
        {
            try
            {
                CacheInstance1.Set("", "a");
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            try
            {
                CacheInstance1.Set<string>(GenerateKey(), null);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public virtual void Cache_Set_AbsoluteExpiration_ArgumentException()
        {
            try
            {
                CacheInstance1.Set("", "", DateTimeOffset.Now.AddSeconds(10));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            try
            {
                CacheInstance1.Set<string>(GenerateKey(), null, DateTimeOffset.Now.AddSeconds(10));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public virtual void Cache_Set_SlidingExpiration_ArgumentException()
        {
            try
            {
                CacheInstance1.Set("", "", TimeSpan.FromMilliseconds(1000));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }

            try
            {
                CacheInstance1.Set<string>(GenerateKey(), null, TimeSpan.FromMilliseconds(1000));
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
        }

        [TestMethod]
        public virtual void Cache_Name()
        {
            var type = CacheInstance1.GetType().Name;
            Assert.AreEqual($"{type}-cache1", CacheInstance1.Name);
            Assert.AreEqual($"{type}-cache2", CacheInstance2.Name);
        }

        [TestMethod]
        public void Cache_Add_Success()
        {
            var key = GenerateKey();
            var key2 = GenerateKey();
            var success = CacheInstance1.Add(key, 1, DateTimeOffset.Now.AddSeconds(2));
            Assert.IsTrue(success);

            var obj = CacheInstance1.Get(key);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(1, (int)obj);

            success = CacheInstance1.Add(key2, 11);
            Assert.IsTrue(success);

            obj = CacheInstance1.Get(key2);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(11, (int)obj);
        }

        [TestMethod]
        public virtual void Cache_Add_SlidingExpiration_Success()
        {
            var key = GenerateKey();
            var success = CacheInstance1.Add(key, 1, new TimeSpan(0, 0, 2));
            Assert.IsTrue(success);

            Thread.Sleep(1100);

            var obj = CacheInstance1.Get(key);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(1, (int)obj);

            Thread.Sleep(1100);

            obj = CacheInstance1.Get(key);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(1, (int)obj);

            Thread.Sleep(2500);

            obj = CacheInstance1.Get(key);
            Assert.IsNull(obj);
        }

        [TestMethod]
        public void Cache_Add_MultipleAddShouldReturnFalse()
        {
            var key = GenerateKey();
            var success = CacheInstance1.Add(key, 1, DateTimeOffset.Now.AddSeconds(100));
            Assert.IsTrue(success);

            success = CacheInstance1.Add(key, 2, DateTimeOffset.Now.AddSeconds(100));
            Assert.IsFalse(success);

            var obj = CacheInstance1.Get(key);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(1, (int)obj);

            var key2 = GenerateKey();
            var success2 = CacheInstance1.Add(key2, 1);
            Assert.IsTrue(success2);

            success2 = CacheInstance1.Add(key2, 2);
            Assert.IsFalse(success2);

            var obj2 = CacheInstance1.Get(key2);
            Assert.IsNotNull(obj2);
            Assert.IsInstanceOfType(obj2, typeof(int));
            Assert.AreEqual(1, (int)obj2);
        }

        [TestMethod]
        public void Cache_Add_CachedItemExpired()
        {
            var key = GenerateKey();
            var success = CacheInstance1.Add(key, 1, DateTimeOffset.Now.AddMilliseconds(2000));

            Assert.IsTrue(success);
            var obj = CacheInstance1.Get(key);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(1, (int)obj);

            Thread.Sleep(2000);

            var containsKey = CacheInstance1.Contains(key);
            Assert.IsFalse(containsKey);

            obj = CacheInstance1.Get(key);
            Assert.IsNull(obj);
        }

        [TestMethod]
        public virtual void Cache_Set_AbsoluteExpiration_Success()
        {
            var key = GenerateKey();
            CacheInstance1.Set(key, 5, DateTimeOffset.Now.AddSeconds(1));

            var obj = CacheInstance1.Get(key);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(5, (int)obj);

            CacheInstance1.Set(key, 6, DateTimeOffset.Now.AddSeconds(1));

            obj = CacheInstance1.Get(key);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(6, (int)obj);
        }

        [TestMethod]
        public virtual void Cache_Set_SlidingExpiration_Success()
        {
            var key = GenerateKey();
            CacheInstance1.Set(key, 7, new TimeSpan(0, 0, 2));

            Thread.Sleep(1000);
            Assert.IsTrue(CacheInstance1.Contains(key));

            Thread.Sleep(1000);
            Assert.IsTrue(CacheInstance1.Contains(key));

            Thread.Sleep(1000);
            Assert.IsTrue(CacheInstance1.Contains(key));

            Thread.Sleep(2500);
            Assert.IsFalse(CacheInstance1.Contains(key));
        }

        [TestMethod]
        public virtual void Cache_Remove_Success()
        {
            var key = GenerateKey();
            CacheInstance1.Set(key, 6, DateTimeOffset.Now.AddMilliseconds(1000));

            var obj = CacheInstance1.Get(key);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(6, (int)obj);

            obj = CacheInstance1.Remove(key);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(int));
            Assert.AreEqual(6, (int)obj);

            CacheInstance1.Set(key, 6);
            var six = CacheInstance1.Remove<int>(key);
            Assert.AreEqual(6, six);

            obj = CacheInstance1.Get(key);
            Assert.IsNull(obj);

            Assert.IsFalse(CacheInstance1.Contains(key));

            obj = CacheInstance1.Remove("1000");
            Assert.IsNull(obj);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public virtual void Cache_Remove_CastException()
        {
            var key = GenerateKey();
            const int one = 1;
            CacheInstance1.Set(key, one);
            CacheInstance1.Remove<string>(key);
        }

        [TestMethod]
        public virtual void Cache_Contains_Success()
        {
            var key = GenerateKey();
            CacheInstance1.Set(key, 7, DateTimeOffset.Now.AddMilliseconds(1000));

            Assert.IsTrue(CacheInstance1.Contains(key));

            Assert.IsFalse(CacheInstance1.Contains("abcdasfd"));
        }

        [TestMethod]
        public virtual void Cache_Get_Success()
        {
            var key = GenerateKey();
            const string text = "abcdefg";
            CacheInstance1.Set(key, text);

            var obj = CacheInstance1.Get(key);
            Assert.IsNotNull(obj);
            Assert.IsInstanceOfType(obj, typeof(string));

            var str = CacheInstance1.Get<string>(key);
            Assert.AreEqual(text, str);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public virtual void Cache_Get_Exception()
        {
            var key = GenerateKey();
            const string text = "abcdefg";
            CacheInstance1.Set(key, text);

            CacheInstance1.Get<int>(key);
        }
    }
}
