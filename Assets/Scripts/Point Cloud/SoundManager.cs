using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cmp2804.Point_Cloud
{
    public class SoundManager : SerializedMonoBehaviour
    {
        private static readonly Dictionary<Transform, Color> ObjectColours = new();

        private static readonly Dictionary<Transform, Color> HighlightObjectColours = new();

        private static readonly Queue<MakerRay> RaysToCast = new();
        private static bool _highlightEnabled = true;

        [OdinSerialize] private Transform _playerHead;

        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        private void Update()
        {
            var raysCast = 0;
            var target = Mathf.Min(200, Mathf.Max(4, Mathf.RoundToInt(RaysToCast.Count / 2f)));
            while (raysCast < target && RaysToCast.Count > 0)
            {
                var ray = RaysToCast.Dequeue();
                if (Physics.Raycast(ray.Origin, ray.Direction, out var hit, ray.Length, ray.LayerMask))
                {
                    if (!hit.transform.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))) //Somehow more performant than
                                                                                                 //simply checking distance to player!
                    {
                        if (!Physics.Raycast(hit.point, _playerHead.position - hit.point, out var playerCheck)) continue;
                        //If the raycast hits something else before the player, ignore it
                        if (!playerCheck.transform.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))) continue; 
                    }
                    if (_highlightEnabled && HighlightObjectColours.TryGetValue(hit.transform, out var colour))
                        PointCloudRenderer.Instance.CreatePoint(hit.transform.InverseTransformPoint(hit.point), hit.transform, hit.normal, colour,
                            ray.Lifespan * (1 - hit.distance / ray.Length));
                    else if (ObjectColours.TryGetValue(hit.transform, out colour))
                        PointCloudRenderer.Instance.CreatePoint(hit.transform.InverseTransformPoint(hit.point), hit.transform, hit.normal, colour,
                            ray.Lifespan * (1 - hit.distance / ray.Length));
                }
                raysCast++;
            }
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

        public static void OnSceneChange(Scene arg0, Scene scene)
        {
            CacheAllColours();
        }

        public static (Dictionary<Transform, Color>, Dictionary<Transform, Color>) CacheAllColours()
        {
            foreach (var renderer in FindObjectsOfType<Renderer>())
            {
                if (renderer.transform.TryGetComponent(out ObjectHighlighter highlight))
                    HighlightObjectColours.Add(renderer.transform, highlight.highlightColour);

                ObjectColours.Add(renderer.transform, renderer.material.color);
                renderer.enabled = false;
            }
            return (HighlightObjectColours, ObjectColours);
        }

        /// <summary>
        /// Updates the colour of an object for the point cloud.
        /// </summary>
        /// <param name="transform">Object to update the colour of.</param>
        /// <param name="newColour">Colour to replace the old.</param>
        public static void UpdateObjectColour(Transform transform, Color newColour)
        {
            if (ObjectColours.ContainsKey(transform))
                ObjectColours[transform] = newColour;
            else
                ObjectColours.Add(transform, transform.GetComponent<Renderer>().material.color);
            PointCloudRenderer.Instance.RefreshPointColourForTransform(transform, newColour);
        }

        /// <summary>
        /// Deletes all points associated with a transform.
        /// </summary>
        /// <param name="transform">Object to hide.</param>
        public static void DisableObjectPoints(Transform transform)
        {
            PointCloudRenderer.Instance.DisablePointsParentedToTransform(transform);
        }

        /// <summary>
        ///     Makes a sound in a sphere at origin provided.
        /// </summary>
        /// <param name="origin">Centre of the sphere projected from.</param>
        /// <param name="numberOfRays">
        ///     The number of rays sent out from the origin, note this does not equal number of points
        ///     generated.
        /// </param>
        /// <param name="rayLength">The distance each ray should travel.</param>
        /// <param name="lifespan">
        ///     How long the point generated should remain for. This is affected by the distance the ray
        ///     travels.
        /// </param>
        /// <param name="inverted">If the rays should be cast from the surface of the sphere towards to origin.</param>
        /// <returns>The a copy of the produced queue for testing.</returns>
        public static Queue<MakerRay> MakeSound(Vector3 origin, int numberOfRays, float rayLength, float lifespan,
            LayerMask layerMask, bool inverted = false)
        {
            var testQueue = new Queue<MakerRay>();
            for (var i = 0; i < numberOfRays; i++)
            {
                var randDirection = Random.onUnitSphere;
                if (inverted)
                {
                    var newOrigin = origin + randDirection * rayLength;
                    RaysToCast.Enqueue(
                        new MakerRay(newOrigin, -randDirection, rayLength, lifespan, layerMask));
                    testQueue.Enqueue(
                        new MakerRay(newOrigin, -randDirection, rayLength, lifespan, layerMask));
                }
                else
                {
                    RaysToCast.Enqueue(
                        new MakerRay(origin, randDirection, rayLength, lifespan, layerMask));
                    testQueue.Enqueue(
                       new MakerRay(origin, randDirection, rayLength, lifespan, layerMask));
                }
            }
            return testQueue;
        }

        /// <summary>
        ///     Makes a sound in a sector of a sphere at the provided origin.
        /// </summary>
        /// <param name="origin">Centre of the sphere projected from.</param>
        /// <param name="direction">The direction of the sound in global 3D space</param>
        /// <param name="angle">The angle of sphere projected from, for example 180 would be a hemisphere.</param>
        /// <param name="numberOfRays">
        ///     The number of rays sent out from the origin, note this does not equal number of points
        ///     generated.
        /// </param>
        /// <param name="rayLength">The distance each ray should travel.</param>
        /// <param name="lifespan">
        ///     How long the point generated should remain for. This is affected by the distance the ray
        ///     travels.
        /// </param>
        /// <param name="inverted">If the rays should be cast from the surface of the sphere towards to origin.</param>
        /// <returns>The a copy of the produced queue for testing.</returns>
        public static Queue<MakerRay> MakeSound(Vector3 origin, Vector3 direction, float angle, int numberOfRays,
           LayerMask layerMask, float rayLength, float lifespan, bool inverted = false)
        {
            var testQueue = new Queue<MakerRay>();
            for (var i = 0; i < numberOfRays; i++)
            {
                var randDirection = GetRandomDirection(direction, angle);
                if (inverted)
                {
                    var newOrigin = origin + randDirection * rayLength;
                    RaysToCast.Enqueue(
                        new MakerRay(newOrigin, -randDirection, rayLength, lifespan, layerMask));
                    testQueue.Enqueue(
                        new MakerRay(newOrigin, -randDirection, rayLength, lifespan, layerMask));
                }
                else
                {
                    RaysToCast.Enqueue(
                        new MakerRay(origin, randDirection, rayLength, lifespan, layerMask));
                    testQueue.Enqueue(
                        new MakerRay(origin, randDirection, rayLength, lifespan, layerMask));
                }
            }
            return testQueue;
        }

        public static void DisableHighlightColours()
        {
            _highlightEnabled = false;
        }

        public static void EnableHighlightColours()
        {
            _highlightEnabled = true;
        }

        public struct MakerRay
        {
            public readonly Vector3 Origin;
            public readonly Vector3 Direction;
            public readonly float Length;
            public readonly float Lifespan;
            public readonly LayerMask LayerMask;

            public MakerRay(Vector3 origin, Vector3 direction, float length, float lifespan, LayerMask layerMask)
            {
                this.Origin = origin;
                this.Direction = direction;
                this.Length = length;
                this.Lifespan = lifespan;
                this.LayerMask = layerMask;
            }
        }
    }
}