using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cmp2804.Point_Cloud
{
    public class SoundManager : MonoBehaviour
    {
        private static readonly Dictionary<Transform, Color> ObjectColours = new();

        private static readonly Dictionary<Transform, Color> HighlightObjectColours = new();

        private static readonly Queue<MakerRay> RaysTocast = new();
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
            var target = Mathf.Max(40, Mathf.RoundToInt(RaysTocast.Count / 2f));
            while (raysCast < target && RaysTocast.Count > 0)
            {
                var ray = RaysTocast.Dequeue();
                if (Physics.Raycast(ray.Origin, ray.Direction, out var hit, ray.Length))
                {
                    if (!Physics.Raycast(hit.point, _playerHead.position - hit.point, out var playerCheck)) continue;

                    if (!playerCheck.transform.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
                    {
                        continue;
                        ;
                    }


                    if (_highlightEnabled && HighlightObjectColours.TryGetValue(hit.transform, out var colour))
                        PointCloudRenderer.Instance.CreatePoint(hit.point, hit.normal, colour,
                            ray.Lifespan * (1 - hit.distance / ray.Length));
                    else if (ObjectColours.TryGetValue(hit.transform, out colour))
                        PointCloudRenderer.Instance.CreatePoint(hit.point, hit.normal, colour,
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
            foreach (var renderer in FindObjectsOfType<Renderer>())
            {
                if (renderer.transform.TryGetComponent(out ObjectHighlighter highlight))
                    HighlightObjectColours.Add(renderer.transform, highlight.highlightColour);

                ObjectColours.Add(renderer.transform, renderer.material.color);
                renderer.enabled = false;
            }
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
        public static void MakeSound(Vector3 origin, int numberOfRays, float rayLength, float lifespan,
            bool inverted = false)
        {
            for (var i = 0; i < numberOfRays; i++)
                if (inverted)
                {
                    var randDirection = Random.onUnitSphere;
                    var newOrigin = origin + randDirection * rayLength;
                    RaysTocast.Enqueue(
                        new MakerRay(newOrigin, -randDirection, rayLength, lifespan));
                }
                else
                {
                    RaysTocast.Enqueue(
                        new MakerRay(origin, Vector3.one, rayLength, lifespan));
                }
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
        public static void MakeSound(Vector3 origin, Vector3 direction, float angle, int numberOfRays,
            float rayLength, float lifespan, bool inverted = false)
        {
            for (var i = 0; i < numberOfRays; i++)
                if (inverted)
                {
                    var randDirection = GetRandomDirection(direction, angle);
                    var newOrigin = origin + randDirection * rayLength;
                    RaysTocast.Enqueue(
                        new MakerRay(newOrigin, -randDirection, rayLength, lifespan));
                }
                else
                {
                    RaysTocast.Enqueue(
                        new MakerRay(origin, GetRandomDirection(direction, angle), rayLength, lifespan));
                }
        }

        public static void DisableHighlightColours()
        {
            _highlightEnabled = false;
        }

        public static void EnableHighlightColours()
        {
            _highlightEnabled = true;
        }

        private struct MakerRay
        {
            public readonly Vector3 Origin;
            public readonly Vector3 Direction;
            public readonly float Length;
            public readonly float Lifespan;

            public MakerRay(Vector3 origin, Vector3 direction, float length, float lifespan)
            {
                this.Origin = origin;
                this.Direction = direction;
                this.Length = length;
                this.Lifespan = lifespan;
            }
        }
    }
}