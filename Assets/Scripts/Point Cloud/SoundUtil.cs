using System.Collections;
using UnityEngine;

namespace cmp2804.Point_Cloud
{
    public static class SoundUtil
    {
        public static void MakeSound(Vector3 position, float intensity)
        {
            PointCloudRenderer.Instance.StartCoroutine(RayCaster(position, intensity));
        }

        private static IEnumerator RayCaster(Vector3 position, float intensity)
        {
            for (int i = 0; i < 10; i++)
            {
                Ray ray = new Ray();
                ray.origin = position;
                float inverseResolution = 10f;
                Vector3 direction = Vector3.right;
                int steps = Mathf.FloorToInt(360f / inverseResolution);
                Quaternion xRotation = Quaternion.Euler(Vector3.right * inverseResolution);
                Quaternion yRotation = Quaternion.Euler(Vector3.up * inverseResolution);
                Quaternion zRotation = Quaternion.Euler(Vector3.forward * inverseResolution);
                for (int x = 0; x < steps / 2; x++)
                {
                    direction = zRotation * direction;
                    for (int y = 0; y < steps; y++)
                    {
                        direction = xRotation * direction;
                         direction = new Vector3(direction.x + (Random.value - 0.5f), direction.y +(Random.value - 0.5f),
                            direction.z +(Random.value - 0.5f));
                        ray.direction = direction;
                        if (Physics.Raycast(ray, out RaycastHit hit, intensity))
                        {
                            Debug.DrawLine(ray.origin, hit.point, Color.black, 2); // for science
                            PointCloudRenderer.Instance.CreatePoint(hit.point, hit.normal, 1);
                        }
                        else
                        {
                            Debug.DrawLine(ray.origin, ray.origin + (direction * 10), Color.black, 2); // for science
                        }
                    }
                }

                yield return new WaitForEndOfFrame();
            }
        }
    }
}