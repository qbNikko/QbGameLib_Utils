using NUnit.Framework;

namespace QbGameLib_Utils.Component.DI
{
    [TestFixture]
    public class DiServiceTest
    {
        public class Service1
        {
            public string service;

            public Service1(string service)
            {
                this.service = service;
            }
        }
        
        public class Service2
        {
            public string service;

            public Service2(string service)
            {
                this.service = service;
            }
        }
        
        [Test]
        public void TestSingletonService()
        {
            DIService.RegisterSingleton(new Service2("Service2"));
            using (DIService.RegisterSingleton(new Service1("Service1")))
            {
                DIService.TryGetSingleton<Service1>(out var service1);
                DIService.TryGetSingleton<Service2>(out var service2);
                Assert.AreEqual("Service1",service1.service);
                Assert.AreEqual("Service2",service2.service);
            }
            Assert.IsFalse(DIService.TryGetSingleton<Service1>(out var service1_out));
            Assert.IsNull(service1_out);
        }
        
        [Test]
        public void TestPrototypeService()
        {
            DIService.RegisterMulti(new Service1("Service2"));
            using (DIService.RegisterMulti(new Service1("Service1")))
            {
                Assert.IsTrue(DIService.TryGetAll<Service1>(out var services));
                Assert.AreEqual(2,services.Count);
                Assert.AreEqual("Service2",services[0].service);
                Assert.AreEqual("Service1",services[1].service);
            }
            Assert.IsTrue(DIService.TryGetAll<Service1>(out var services2));
            Assert.AreEqual(1,services2.Count);
            Assert.AreEqual("Service2",services2[0].service);
        }
    }
}