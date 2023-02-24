using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace cmp2804.Point_Cloud
{
    public class SoundManager : MonoBehaviour
    {
        private struct MakerRay
        {
            public Vector3 origin;
            public Vector3 direction;
            public float length;
            public float lifespan;

            public MakerRay(Vector3 origin, Vector3 direction, float length, float lifespan)
            {
                this.origin = origin;
                this.direction = direction;
                this.length = length;
                this.lifespan = lifespan;
            }
        }

        [SerializeField] private Transform playerHead;
        
        private static readonly Dictionary<Transform, Color> _objectColours = new Dictionary<Transform, Color>();

        private static readonly Dictionary<Transform, Color> _highlightObjectColours =
            new Dictionary<Transform, Color>();

        private static readonly Queue<MakerRay> _raysTocast = new Queue<MakerRay>();
        private static bool highlightEnabled = true;

        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Update()
        {
            int raysCast = 0;
            int target = Mathf.Max(40, Mathf.RoundToInt(_raysTocast.Count / 2f));
            while (raysCast < target && _raysTocast.Count > 0)
            {
                MakerRay ray = _raysTocast.Dequeue();
                if (Physics.Raycast(ray.origin, ray.direction, out RaycastHit hit, ray.length))
                {
                    if (!Physics.Raycast(hit.point, playerHead.position - hit.point, out RaycastHit playerCheck))
                    {
                        continue;
                    }

                    if (!playerCheck.transform.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
                    {
                        continue;
                        ;
                    }
                    
                    
                    if (highlightEnabled && _highlightObjectColours.TryGetValue(hit.transform, out Color colour))
                    {
                        PointCloudRenderer.Instance.CreatePoint(hit.point, hit.normal, colour,
                            ray.lifespan * (1 - (hit.distance / ray.length)));
                    }
                    else if (_objectColours.TryGetValue(hit.transform, out colour))
                    {
                        PointCloudRenderer.Instance.CreatePoint(hit.point, hit.normal, colour,
                            ray.lifespan * (1 - (hit.distance / ray.length)));
                    }
                }

                raysCast++;
            }
        }

        private static Vector3 GetRandomDirection(Vector3 direction, float angle)
        {
            int tries = 0;
            while (tries < 500)
            {
                Vector3 randomDirection = Random.onUnitSphere;

                // Check if the angle between the random direction and the sector direction is less than the sector angle
                if (Vector3.Angle(randomDirection, direction) < angle / 2)
                {
                    return randomDirection;
                }

                tries++;
            }

            return Vector3.zero;
        }

        public static void OnSceneChange(Scene arg0, Scene scene)
        {
            foreach (var renderer in FindObjectsOfType<Renderer>())
            {
                if (renderer.transform.TryGetComponent<ObjectHighlighter>(out ObjectHighlighter highlight))
                {
                    _highlightObjectColours.Add(renderer.transform, highlight.HighlightColour);
                }

                _objectColours.Add(renderer.transform, renderer.material.color);
                renderer.enabled = false;
            }
        }

        /// <summary>
        /// Makes a sound in a sphere at origin provided.
        /// </summary>
        /// <param name="origin">Centre of the sphere projected from.</param>
        /// <param name="numberOfRays">The number of rays sent out from the origin, note this does not equal number of points generated.</param>
        /// <param name="rayLength">The distance each ray should travel.</param>
        /// <param name="lifespan">How long the point generated should remain for. This is affected by the distance the ray travels.</param>
        /// <param name="inverted">If the rays should be cast from the surface of the sphere towards to origin.</param>
        public static void MakeSound(Vector3 origin, int numberOfRays, float rayLength, float lifespan,
            bool inverted = false)
        {
            for (int i = 0; i < numberOfRays; i++)
            {
                if (inverted)
                {
                    Vector3 randDirection = Random.onUnitSphere;
                    Vector3 newOrigin = origin + randDirection * rayLength;
                    _raysTocast.Enqueue(
                        new MakerRay(newOrigin, -randDirection, rayLength, lifespan));
                }
                else
                {
                    _raysTocast.Enqueue(
                        new MakerRay(origin, Vector3.one, rayLength, lifespan));
                }
            }
        }

        /// <summary>
        /// Makes a sound in a sector of a sphere at the provided origin.
        /// </summary>
        /// <param name="origin">Centre of the sphere projected from.</param>
        /// <param name="direction">The direction of the sound in global 3D space</param>
        /// <param name="angle">The angle of sphere projected from, for example 180 would be a hemisphere.</param>
        /// <param name="numberOfRays">The number of rays sent out from the origin, note this does not equal number of points generated.</param>
        /// <param name="rayLength">The distance each ray should travel.</param>
        /// <param name="lifespan">How long the point generated should remain for. This is affected by the distance the ray travels.</param>
        /// <param name="inverted">If the rays should be cast from the surface of the sphere towards to origin.</param>
        public static void MakeSound(Vector3 origin, Vector3 direction, float angle, int numberOfRays,
            float rayLength, float lifespan, bool inverted = false)
        {
            for (int i = 0; i < numberOfRays; i++)
            {
                if (inverted)
                {
                    Vector3 randDirection = GetRandomDirection(direction, angle);
                    Vector3 newOrigin = origin + randDirection * rayLength;
                    _raysTocast.Enqueue(
                        new MakerRay(newOrigin, -randDirection, rayLength, lifespan));
                }
                else
                {
                    _raysTocast.Enqueue(
                        new MakerRay(origin, GetRandomDirection(direction, angle), rayLength, lifespan));
                }
            }
        }

        public static void DisableHighlightColours()
        {
            highlightEnabled = false;
        }

        public static void EnableHighlightColours()
        {
            highlightEnabled = true;
        }
    }
}