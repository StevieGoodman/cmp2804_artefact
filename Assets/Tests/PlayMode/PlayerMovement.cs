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
            SetupPlayerCharacter(out var movement, out var movementState, out var rigidbody);
            var oldPosition = rigidbody.position;
            yield return null;
            var newPosition = rigidbody.position;
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
            out BasicMovement basicMovement, out MovementState movementState, out Rigidbody rigidbody)
        {
            var player = new GameObject("Player");
            basicMovement = player.AddComponent<BasicMovement>();
            movementState = ScriptableObject.CreateInstance<MovementState>();
            rigidbody = player.GetComponent<Rigidbody>();
            movementState.moveSpeed = 100f;
            movementState.rotationSpeed = 2f;
            basicMovement.MovementState = movementState;
            basicMovement.MoveTarget = new Target(Vector3.forward);
        }
    }
}
