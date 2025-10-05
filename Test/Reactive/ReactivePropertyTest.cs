using System;
using NUnit.Framework;
using QbGameLib_Utils.Reactive;

namespace QbGameLib.QbGameLib_Utils.Test.Reactive
{
    [TestFixture]
    public class ReactivePropertyTest
    {
        private class ValueClass
        {
            public int val = 0;
        }
        [Test]
        public void TestReactiveProperty()
        {
            ValueClass value = new ValueClass();
            ReactiveProperty<int> reactiveProperty = new ReactiveProperty<int>(0);
            IDisposable disposable = reactiveProperty.Subscribe((v) => value.val = v);
            //
            reactiveProperty.Value = 1;
            Assert.AreEqual(1, value.val);
            
            reactiveProperty.Value = 2;
            Assert.AreEqual(2, value.val);
            
            disposable.Dispose();
            reactiveProperty.Value = 3;
            Assert.AreEqual(2, value.val);
            
            disposable = reactiveProperty.Subscribe((v) => value.val = v);
            reactiveProperty.Value = 4;
            Assert.AreEqual(4, value.val);
            reactiveProperty.Dispose();
            reactiveProperty.Value = 5;
            Assert.AreEqual(4, value.val);
        }

        [Test]
        public void TestReactiveValidateProperty()
        {
            ValueClass value = new ValueClass();
            ReactiveProperty<int> reactiveProperty = new ReactiveProperty<int>(0);
            reactiveProperty.Validate((v)=>v<5);
            IDisposable disposable = reactiveProperty.Subscribe((v) => value.val = v);
            //
            reactiveProperty.Value = 1;
            Assert.AreEqual(1, value.val);
            reactiveProperty.Value = 10;
            Assert.AreEqual(1, value.val);
        }
        
        [Test]
        public void TestReactiveEqualsProperty()
        {
            ValueClass value = new ValueClass();
            IReactiveProperty<int> reactiveProperty = new ReactiveProperty<int>(0)
                .Subscribe((v) => value.val++, out IDisposable disposable);
            
            reactiveProperty.Value = 10;
            Assert.AreEqual(1, value.val);
            reactiveProperty.Value = 10;
            Assert.AreEqual(1, value.val);
            reactiveProperty.Value = 3;
            Assert.AreEqual(2, value.val);
        }
        
        [Test]
        public void TestReactiveExceptionProperty()
        {
            ValueClass value = new ValueClass();
            IReactiveProperty<int> reactiveProperty = new ReactiveProperty<int>(0)
                .Subscribe((v) =>
                {
                    if(v==-1) throw new Exception();
                    value.val = v;
                }, out IDisposable disposable)
                .AutoUnsubscribeOnException();
            
            reactiveProperty.Value = 1;
            Assert.AreEqual(1, value.val);
            reactiveProperty.Value = -1;
            Assert.AreEqual(1, value.val);
            reactiveProperty.Value = 3;
            Assert.AreEqual(1, value.val);
            
            
            value = new ValueClass();
            IReactiveProperty<int> reactiveProperty2 = new ReactiveProperty<int>(0)
                .Subscribe((v) =>
                {
                    if(v==-1) throw new Exception();
                    value.val = v;
                }, out IDisposable disposable2);
            
            reactiveProperty2.Value = 1;
            Assert.AreEqual(1, value.val);
            reactiveProperty2.Value = -1;
            Assert.AreEqual(1, value.val);
            reactiveProperty2.Value = 3;
            Assert.AreEqual(3, value.val);
        }
    }
    
}