using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using static cmp2804.Point_Cloud.SoundManager;

namespace Tests.Playmode
{
    public class SoundManager
    {
        [UnityTest]
        public IEnumerator MakeSound_WithInvertedFalse_EnqueuesCorrectRays()
        {
            // Arrange
            Vector3 origin = Vector3.zero;
            int numberOfRays = 3;
            float rayLength = 5f;
            float lifespan = 10f;
            bool inverted = false;
            Queue<MakerRay> expectedRays = new Queue<MakerRay>();
            Random.InitState(0);
            for (int i = 0; i < numberOfRays; i++)
            {
                var randDirection = Random.onUnitSphere;
                expectedRays.Enqueue(new MakerRay(origin, randDirection, rayLength, lifespan));
            }
            yield return null;
            // Act
            Queue<MakerRay> actualRays = MakeSound(origin, numberOfRays, rayLength, lifespan, inverted, 0);

            // Assert
            Assert.AreEqual(expectedRays.Count, actualRays.Count);
            while (expectedRays.Count > 0)
            {
                MakerRay expectedRay = expectedRays.Dequeue();
                MakerRay actualRay = actualRays.Dequeue();
                Assert.AreEqual(expectedRay.Origin, actualRay.Origin);
                Assert.AreEqual(expectedRay.Direction, actualRay.Direction);
                Assert.AreEqual(expectedRay.Length, actualRay.Length);
                Assert.AreEqual(expectedRay.Lifespan, actualRay.Lifespan);
            }
        }
        [UnityTest]
        public IEnumerator MakeSound_WithInvertedTrue_EnqueuesCorrectRays()
        {
            // Arrange
            Vector3 origin = Vector3.zero;
            int numberOfRays = 8;
            float rayLength = 2f;
            float lifespan = 30f;
            bool inverted = true;
            Queue<MakerRay> expectedRays = new Queue<MakerRay>();
            Random.InitState(0);
            for (int i = 0; i < numberOfRays; i++)
            {
                var randDirection = Random.onUnitSphere;
                var newOrigin = origin + randDirection * rayLength;
                expectedRays.Enqueue(new MakerRay(newOrigin, -randDirection, rayLength, lifespan));
            }
            yield return null;
            // Act
            Queue<MakerRay> actualRays = MakeSound(origin, numberOfRays, rayLength, lifespan, inverted, 0);

            // Assert
            Assert.AreEqual(expectedRays.Count, actualRays.Count);
            while (expectedRays.Count > 0)
            {
                MakerRay expectedRay = expectedRays.Dequeue();
                MakerRay actualRay = actualRays.Dequeue();
                Assert.AreEqual(expectedRay.Origin, actualRay.Origin);
                Assert.AreEqual(expectedRay.Direction, actualRay.Direction);
                Assert.AreEqual(expectedRay.Length, actualRay.Length);
                Assert.AreEqual(expectedRay.Lifespan, actualRay.Lifespan);
            }
        }
    }
}
