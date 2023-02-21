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

        private static readonly Dictionary<Transform, Color> _objectColours = new Dictionary<Transform, Color>();
        private static readonly Queue<MakerRay> _raysTocast = new Queue<MakerRay>();

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
                    if (_objectColours.TryGetValue(hit.transform, out Color colour))
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
                _objectColours.Add(renderer.transform, renderer.material.color);
                renderer.enabled = false;
            }
        }

        public static void MakeSound(Transform origin, int numberOfRays, float rayLength, float lifespan)
        {
            for (int i = 0; i < numberOfRays; i++)
            {
                _raysTocast.Enqueue(
                    new MakerRay(origin.position, Vector3.one, rayLength, lifespan));
            }
            // Instance.StartCoroutine(RayCaster(origin, Vector3.zero, 360, numberOfRays, rayLength,
            //     lifespan));
        }

        public static void MakeSound(Transform origin, Vector3 direction, float angle, int numberOfRays,
            float rayLength, float lifespan)
        {
            // Instance.StartCoroutine(RayCaster(origin, direction, angle, numberOfRays, rayLength,
            //     lifespan));
            for (int i = 0; i < numberOfRays; i++)
            {
                _raysTocast.Enqueue(
                    new MakerRay(origin.position, GetRandomDirection(direction, angle), rayLength, lifespan));
            }
        }

        // private static IEnumerator RayCaster(Transform origin, Vector3 direction, float angle, int numberOfRays,
        //     float rayLength, float lifespan)
        // {
        //     int raysCast = 0;
        //     while (raysCast < numberOfRays)
        //     {
        //         Vector3 randomDirection = Random.onUnitSphere;
        //
        //         // Check if the angle between the random direction and the sector direction is less than the sector angle
        //         if (Vector3.Angle(randomDirection, direction) < angle / 2)
        //         {
        //             if (Physics.Raycast(origin.position, randomDirection, out RaycastHit hit, rayLength))
        //             {
        //                 PointCloudRenderer.Instance.CreatePoint(hit.point, hit.normal, _objectColours[hit.transform],
        //                     lifespan);
        //             }
        //
        //             if (++raysCast % 10 == 0)
        //             {
        //                 yield return new WaitForSeconds(0.1f);
        //             }
        //         }
        //     }
        // }
    }
}