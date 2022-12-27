namespace GameAPI.GameObjects
{
    public class Projectile : GameObject
    {
        private float _movementUpdate = 0f;
        public Projectile(int x, int y, Grids grid, Directions direction) : base(x, y, Types.Projectile, grid)
        {
            LastDirection = direction;
        }

        public override void EnqueueMovement(Directions direction)
        {
            if (_movementUpdate > 0.02f)
            {
                base.EnqueueMovement(direction);
                _movementUpdate = 0f;
            }
        }

        public override void Update(float deltaTime)
        {
            _movementUpdate += deltaTime;
            EnqueueMovement(LastDirection);
        }
    }
}
