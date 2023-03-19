using cmp2804.Point_Cloud;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
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

            // Act
            Random.InitState(0);
            Queue<MakerRay> actualRays = MakeSound(origin, numberOfRays, rayLength, lifespan, inverted);

            // Assert
            Assert.AreEqual(numberOfRays, actualRays.Count);
            while (actualRays.Count > 0)
            {
                MakerRay actualRay = actualRays.Dequeue();
                Assert.AreEqual(actualRay.Origin, origin, "Origin");
                Assert.AreApproximatelyEqual(actualRay.Direction.magnitude, 1, "Direction is normalised");
                Assert.AreEqual(actualRay.Length, rayLength, "Length");
                Assert.AreEqual(actualRay.Lifespan, lifespan, "Lifespan");
            }

            yield return null;
        }
        [UnityTest]
        public IEnumerator MakeSound_WithInvertedTrue_EnqueuesCorrectRays()
        {
            // Arrange
            Vector3 origin = Vector3.zero;
            int numberOfRays = 7;
            float rayLength = 2f;
            float lifespan = 6f;
            bool inverted = true;

            // Act
            Random.InitState(0);
            Queue<MakerRay> actualRays = MakeSound(origin, numberOfRays, rayLength, lifespan, inverted);

            // Assert
            Assert.AreEqual(numberOfRays, actualRays.Count);
            while (actualRays.Count > 0)
            {
                MakerRay actualRay = actualRays.Dequeue();
                Assert.AreApproximatelyEqual(Vector3.Distance(origin, actualRay.Origin), rayLength, "Origin within sphere");
                Assert.AreApproximatelyEqual(actualRay.Direction.magnitude, 1, "Direction is normalised");
                Assert.AreEqual(actualRay.Length, rayLength, "Length");
                Assert.AreEqual(actualRay.Lifespan, lifespan, "Lifespan");
            }

            yield return null;
        }
        [UnityTest]
        public IEnumerator MakeDirectionalSound_WithInvertedFalse_EnqueuesCorrectRays()
        {
            // Arrange
            Vector3 origin = Vector3.zero;
            Vector3 direction = Vector3.up;
            float angle = 30f;
            int numberOfRays = 3;
            float rayLength = 5f;
            float lifespan = 10f;
            bool inverted = false;

            // Act
            Random.InitState(0);
            Queue<MakerRay> actualRays = MakeSound(origin, direction, angle, numberOfRays, rayLength, lifespan, inverted);

            // Assert
            Assert.AreEqual(actualRays.Count, numberOfRays);
            while (actualRays.Count > 0)
            {
                MakerRay actualRay = actualRays.Dequeue();
                Assert.AreEqual(actualRay.Origin, origin, "Origin");
                Assert.IsTrue(Vector3.Angle(actualRay.Direction, direction) < angle / 2, "Direction within angle");
                Assert.AreApproximatelyEqual(actualRay.Direction.magnitude, 1, "Direction is normalised");
                Assert.AreEqual(actualRay.Length, actualRay.Length, "Length");
                Assert.AreEqual(actualRay.Lifespan, actualRay.Lifespan, "Lifespan");
            }

            yield return null;
        }

        [UnityTest]
        public IEnumerator MakeDirectionalSound_WithInvertedTrue_EnqueuesCorrectRays()
        {
            // Arrange
            Vector3 origin = Vector3.zero;
            Vector3 direction = Vector3.right;
            float angle = 80f;
            int numberOfRays = 13;
            float rayLength = 3f;
            float lifespan = 12f;
            bool inverted = true;

            // Act
            Random.InitState(0);
            Queue<MakerRay> actualRays = MakeSound(origin, direction, angle, numberOfRays, rayLength, lifespan, inverted);

            // Assert
            Assert.AreEqual(actualRays.Count, numberOfRays);
            while (actualRays.Count > 0)
            {
                MakerRay actualRay = actualRays.Dequeue();
                Assert.AreApproximatelyEqual(Vector3.Distance(origin, actualRay.Origin), rayLength, "Origin within sphere");
                Assert.IsTrue(Vector3.Angle(actualRay.Direction, direction) > angle / 2, "Direction within angle");//Reversed due to inverted
                Assert.AreApproximatelyEqual(actualRay.Direction.magnitude, 1, "Direction is normalised");
                Assert.AreEqual(actualRay.Length, actualRay.Length, "Length");
                Assert.AreEqual(actualRay.Lifespan, actualRay.Lifespan, "Lifespan");
            }
            yield return null;
        }

        [UnityTest]
        public IEnumerator CacheAllColours_DisablesRenderersAndStoresOriginalColors()
        {
            // Arrange
            var renderer1 = new GameObject("Renderer1").AddComponent<MeshRenderer>();
            var renderer2 = new GameObject("Renderer2").AddComponent<MeshRenderer>();
            var highlight1 = renderer1.gameObject.AddComponent<ObjectHighlighter>();
            highlight1.highlightColour = Color.red;
            var highlight2 = renderer2.gameObject.AddComponent<ObjectHighlighter>();
            highlight2.highlightColour = Color.blue;

            var renderer3 = new GameObject("Renderer1").AddComponent<MeshRenderer>();
            var renderer4 = new GameObject("Renderer2").AddComponent<MeshRenderer>();
            renderer3.material.color = Color.red;
            renderer4.material.color = Color.blue;

            // Act
            (var highlightObjectColours, var objectColours) = CacheAllColours();

            // Assert
            Assert.IsFalse(renderer1.enabled);
            Assert.IsFalse(renderer2.enabled);
            Assert.IsFalse(renderer3.enabled);
            Assert.IsFalse(renderer4.enabled);
            Assert.IsTrue(highlightObjectColours.ContainsKey(renderer1.transform));
            Assert.IsTrue(highlightObjectColours.ContainsKey(renderer2.transform));
            Assert.AreEqual(Color.red, highlightObjectColours[renderer1.transform]);
            Assert.AreEqual(Color.blue, highlightObjectColours[renderer2.transform]);
            Assert.IsTrue(objectColours.ContainsKey(renderer3.transform));
            Assert.IsTrue(objectColours.ContainsKey(renderer4.transform));
            Assert.AreEqual(Color.red, objectColours[renderer3.transform]);
            Assert.AreEqual(Color.blue, objectColours[renderer4.transform]);

            // Clean up
            GameObject.DestroyImmediate(renderer1.gameObject);
            GameObject.DestroyImmediate(renderer2.gameObject);

            yield return null;
        }


        private static Vector3 GetRandomDirection(Vector3 direction, float angle)
        {
            var tries = 0;
            while (tries < 500)
            {
                var randomDirection = Random.onUnitSphere;

                // Check if the angle between the random direction and the sector direction is less than the sector angle
                if (Vector3.Angle(randomDirection, direction) < angle / 2) return randomDirection;

                tries++;
            }

            return Vector3.zero;
        }
    }
}
