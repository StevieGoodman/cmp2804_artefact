using cmp2804.Math;

namespace cmp2804.Characters
{
    public interface IMovement
    {
        bool CanMove { get; set; }
        Target MoveTarget { get; set; }
        Target LookTarget { get; set; }
        MovementState MovementState { set; }
        void Awake();
        void Update();
        void IncrementMovement();
        void IncrementRotation();
    }
}