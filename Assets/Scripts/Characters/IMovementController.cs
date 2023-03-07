namespace cmp2804.Characters
{
    public interface IMovementController
    {
        /// <summary>
        ///     Increments the movement of the character for the next frame.
        /// </summary>
        public void IncrementMovement();

        /// <summary>
        ///     Increments the rotation of the character for the next frame.
        /// </summary>
        public void IncrementRotation();
    }
}