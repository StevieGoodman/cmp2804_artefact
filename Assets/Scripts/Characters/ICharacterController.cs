namespace cmp2804.Characters
{
    public interface ICharacterController
    {
        /// <summary>
        /// Increments the movement of the character for the next frame.
        /// </summary>
        public void IncrementMovement();
        
        /// <summary>
        /// Increments the rotation of the character for the next frame.
        /// </summary>
        public void IncrementRotation();
    }
}