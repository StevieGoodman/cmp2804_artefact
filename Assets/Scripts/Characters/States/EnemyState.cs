using System.Threading.Tasks;
using Sirenix.OdinInspector;

namespace cmp2804.Characters.States
{
    public abstract class EnemyState : SerializedMonoBehaviour
    {
        /// <summary>
        /// Updates the character's state.
        /// </summary>
        public abstract void UpdateState();
        
        /// <summary>
        /// Performs actions for this state.
        /// </summary>
        public abstract Task TickState();
    }
}