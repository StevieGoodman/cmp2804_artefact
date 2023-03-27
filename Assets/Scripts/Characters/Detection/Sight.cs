using cmp2804.Characters.States;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace cmp2804.Characters.Detection
{
    [HideMonoScript]
    public class Sight : SerializedMonoBehaviour
    {
        [HideLabel, Title("Distance", "The distance the character can see.")]
        [OdinSerialize, PropertyRange(1, 10)] public float Distance { get; private set; } = 5f;
        
        [HideLabel, Title("Ray Count", "The number of rays to cast.")]
        [OdinSerialize, Range(1, 50)] private int _rayCount = 10;
        [HideLabel, Title("Field of View", "The angle the character can see.")]
        [OdinSerialize, Range(1, 360)] private float _fov = 90;

        public bool PlayerInSight()
        {
            var transformVector = transform.forward;
            var angleStep = _fov / (_rayCount - 1);
            for (var rayIndex = 1; rayIndex <= _rayCount; rayIndex++)
            {
                var angle = (rayIndex - 1) * angleStep - _fov / 2;
                var rayDirection = Quaternion.AngleAxis(angle, transform.up) * transformVector;
                var ray = new Ray(transform.position, rayDirection);
                Physics.Raycast(ray, out var hit, Distance);
                if (hit.transform is null) continue;
                var hitGameObject = hit.transform.gameObject;
                if (!hitGameObject.CompareTag("Player")) continue;
                return true;
            }
            return false;
        }
    }
}