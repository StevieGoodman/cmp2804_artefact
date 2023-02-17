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
            Vector3 direction = Vector3.down;
            float sectorAngle = 70;
            int numRays = Mathf.RoundToInt(intensity * 100);
            //for (int i = 0; i < 100; i++)
            {
                int raysCast = 0;
                while (raysCast < numRays)
                {
                    Vector3 randomDirection = Random.onUnitSphere;

                    // Check if the angle between the random direction and the sector direction is less than the sector angle
                    if (Vector3.Angle(randomDirection, direction) < sectorAngle)
                    {
                        Ray ray = new Ray(position, randomDirection);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, intensity * 10))
                        {
                            PointCloudRenderer.Instance.CreatePoint(hit.point, hit.normal, intensity);
                            //Debug.DrawLine(position, hit.point, Color.red, 2);
                        }
                        else
                        {
                            //Debug.DrawRay(position, randomDirection * intensity, Color.blue, 1);
                        }

                        raysCast++;
                    }
                }
                // Vector3 direction = Random.onUnitSphere;
                // if (Physics.Raycast(position, direction, out RaycastHit hit, intensity))
                // {
                //     Debug.DrawLine(position, hit.point, Color.black, 2); // for science
                //     PointCloudRenderer.Instance.CreatePoint(hit.point, hit.normal, 1);
                // }
                // else
                // {
                //     Debug.DrawLine(position, position + (direction * 10), Color.black, 6); // for science
                // }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}