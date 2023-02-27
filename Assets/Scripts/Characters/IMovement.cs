namespace Characters
{
    public interface IMovement
    {
        /// <summary>
        /// Increments the movement of the character for the next frame.
        /// </summary>
        void IncrementMovement();
        
        /// <summary>
        /// Increments the rotation of the character for the next frame.
        /// </summary>
        void IncrementRotation();
    }
}