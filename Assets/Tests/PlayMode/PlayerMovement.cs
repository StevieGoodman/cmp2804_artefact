using System.Collections;
using cmp2804.Characters;
using cmp2804.Math;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

namespace Tests.PlayMode
{
    public class PlayerMovement
    {
        [UnityTest]
        public IEnumerator IncrementMovement()
        {
            SetupPlayerCharacter(out var movement, out var movementState, out var transform);
            var oldPosition = transform.position;
            yield return null;
            var newPosition = transform.position;
            var targetPosition = oldPosition + movement.MoveTarget.Origin * (Time.deltaTime * movementState.moveSpeed);
            Assert.AreEqual(
                newPosition, 
                targetPosition);
        }
        
        [UnityTest]
        public IEnumerator IncrementRotation()
        {
            SetupPlayerCharacter(out var movement, out var movementState, out var rigidBody);
            movement.MoveTarget = new Target(Vector3.left);
            var oldRotation = rigidBody.rotation;
            yield return null;
            var newRotation = rigidBody.rotation;
            var maximumAngle = 360 * Time.deltaTime / movementState.rotationSpeed;
            var targetRotation = Quaternion.RotateTowards(
                oldRotation, 
                newRotation, 
                maximumAngle);
            Assert.AreEqual(newRotation, targetRotation);
        }
        
        private static void SetupPlayerCharacter(
            out Movement movement, out MovementState movementState, out Transform transform)
        {
            var player = new GameObject("Player");
            movement = player.AddComponent<Movement>();
            movementState = ScriptableObject.CreateInstance<MovementState>();
            transform = player.transform;
            movementState.moveSpeed = 100f;
            movementState.rotationSpeed = 2f;
            movement.MovementState = movementState;
            movement.MoveTarget = new Target(Vector3.forward);
        }
    }
}
