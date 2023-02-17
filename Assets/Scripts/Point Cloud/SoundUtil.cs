using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace cmp2804.Point_Cloud
{
    public static class SoundUtil
    {
        private static readonly Dictionary<Transform, Color> _objectColours = new Dictionary<Transform, Color>();

        public static void OnSceneChange(Scene arg0, Scene scene)
        {
            foreach (var renderer in Object.FindObjectsOfType<Renderer>())
            {
                _objectColours.Add(renderer.transform, renderer.material.color);
                //renderer.enabled = false;
            }
        }

        public static void MakeSound(Transform origin, int numberOfRays, float rayLength, float lifespan)
        {
            PointCloudRenderer.Instance.StartCoroutine(RayCaster(origin, Vector3.zero, 360, numberOfRays, rayLength,
                lifespan));
        }

        public static void MakeSound(Transform origin, Vector3 direction, float angle, int numberOfRays,
            float rayLength, float lifespan)
        {
            PointCloudRenderer.Instance.StartCoroutine(RayCaster(origin, direction, angle, numberOfRays, rayLength,
                lifespan));
        }

        private static IEnumerator RayCaster(Transform origin, Vector3 direction, float angle, int numberOfRays,
            float rayLength, float lifespan)
        {
            int raysCast = 0;
            while (raysCast < numberOfRays)
            {
                Vector3 randomDirection = Random.onUnitSphere;

                // Check if the angle between the random direction and the sector direction is less than the sector angle
                if (Vector3.Angle(randomDirection, direction) < angle/2)
                {
                    Ray ray = new Ray(origin.position, randomDirection);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, rayLength))
                    {
                        PointCloudRenderer.Instance.CreatePoint(hit.point, hit.normal, _objectColours[hit.transform],
                            lifespan);
                    }
                    if (++raysCast % numberOfRays/10 == 0)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
        }
    }
}