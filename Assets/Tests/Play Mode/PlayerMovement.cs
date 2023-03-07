using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.Play_Mode
{
    public class PlayerMovement
    {
        [Test]
        public void PlayerMovementSimplePasses()
        {
            // Use the Assert class to test conditions.
        }

        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator PlayerMovementWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
    }
}