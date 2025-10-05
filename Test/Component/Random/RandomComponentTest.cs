using NUnit.Framework;
using QbGameLib_Utils.Component.Random;
using UnityEngine;

namespace QbGameLib_Utils.Test.Component.Random
{
    [TestFixture]
    public class RandomComponentTest
    {
        [Test]
        public void TestGetRandomPositionOnCircle()
        {
            RandomComponent randomComponent = new RandomComponent();
            for (int i = 0; i < 100; i++)
            {
                Vector2 randomPosition = randomComponent.GetRandomPositionOnCircle(10, 5, 3);
                Debug.Log(randomPosition);
                Assert.IsTrue(randomPosition.y>=3f);
            }
        }
        
        [Test]
        public void TestGetRandomPositionOnRectangle()
        {
            RandomComponent randomComponent = new RandomComponent();
            for (int i = 0; i < 100; i++)
            {
                Vector2 randomPosition = randomComponent.GetRandomPositionOnRectangle(new Rect(-3,0,6,3));
                Assert.IsTrue(randomPosition.y>=0f);
                Assert.IsTrue(randomPosition.y<=3f);
                Assert.IsTrue(randomPosition.x>=-3f);
                Assert.IsTrue(randomPosition.x<=6);
            }
        }
        public enum TestRandomEnum
        {
            A, B, C, D, E, F, G, H, I, J, K
        }
        [Test]
        public void TestGetRandomEnum()
        {
            
            RandomComponent randomComponent = new RandomComponent();
            TestRandomEnum randomEnum = randomComponent.GetRandomEnum<TestRandomEnum>();
            Debug.Log(randomEnum);
        }
    }
}