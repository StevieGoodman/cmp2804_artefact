using System.Threading.Tasks;

namespace cmp2804.Characters.States
{
    public interface IEnemyState
    {
        /// <summary>
        /// Updates the character's state.
        /// </summary>
        void UpdateState();
        
        /// <summary>
        /// Performs actions for this state.
        /// </summary>
        Task TickState();
    }
}