namespace GameAPI.GameObjects
{
    public class Enemy : GameObject
    {
        private float _movementUpdate = 0f;
        public Enemy(int x, int y, Grids grid) : base(x, y, Types.Enemy, grid)
        {
        }

        public override void EnqueueMovement(Directions direction)
        {
            if (_movementUpdate > 0.05f)
            {
                base.EnqueueMovement(direction);
                _movementUpdate = 0f;
            }
        }

        public override void Update(float deltaTime)
        {
            _movementUpdate += deltaTime;
        }
    }
}
