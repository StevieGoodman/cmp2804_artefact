using cmp2804.Math;

namespace cmp2804.Characters
{
    public interface IMovement
    {
        /// <summary>
        /// Whether the character can move.
        /// </summary>
        bool CanMove { get; set; }
        
        /// <summary>
        /// The target to move in the direction of.
        /// </summary>
        Target MoveTarget { get; set; }
        
        /// <summary>
        /// The target to look in the direction of.
        /// </summary>
        Target LookTarget { get; set; }
        
        /// <summary>
        /// The state that defines the movement characteristics of the character.
        /// </summary>
        MovementState MovementState { set; }

        /// <summary>
        /// Increments the movement of the character by 1 frame.
        /// </summary>
        void IncrementMovement();

        /// <summary>
        /// Increments the rotation of the character by 1 frame.
        /// </summary>
        void IncrementRotation();
    }
}