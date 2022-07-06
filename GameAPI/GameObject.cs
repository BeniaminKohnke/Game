namespace GameAPI
{
    public class GameObject
    {
        private static uint _lastId = 0;
        public readonly uint Id = _lastId++;
        private readonly Queue<Directions> _movement = new();
        private readonly Dictionary<Animations, States[]> _animations = new();

        public ushort Weight { get; set; } = 0;
        public int MovementSpeed { get; set; } = 0;
        public int MovementSpeedMultipiler { get; set; } = 1;
        public States State { get; set; }
        public Types ObjectType { get; set; }
        public Shapes Shape { get; set; }
        
        public byte[][] ShapeData { get; set; } = Array.Empty<byte[]>();

        public long Y { get; set; }
        public long X { get; set; }

        public long RelativeY => Y + ShapeData.Length;

        public void Initialize(ShapeLoader loader)
        {
            var states = loader.Shapes[Shape];
            ShapeData = states[States.NoAction1];

            foreach (var animation in Parameters.Animations)
            {
                var animationStates = new List<States>();
                foreach (var state in animation.Value)
                {
                    if (states.ContainsKey(state))
                    {
                        animationStates.Add(state);
                    }
                }

                if (animationStates.Any())
                {
                    _animations[animation.Key] = animationStates.ToArray();
                }
            }
        }

        public void EnqueueMovement(Directions direction) => _movement.Enqueue(direction);

        public Directions DequeueMovement(ShapeLoader shapeLoader)
        {
            if(_movement.TryDequeue(out var direction))
            {
                var changeAnimation = false;
                switch(direction)
                {
                    case Directions.Up:
                        TrySetNextState(Animations.MovingRight);
                        break;
                    case Directions.Down:
                        TrySetNextState(Animations.MovingLeft);
                        break;
                    case Directions.Left:
                        TrySetNextState(Animations.MovingLeft);
                        break;
                    case Directions.Right:
                        TrySetNextState(Animations.MovingRight);
                        break;
                }
                
                if(changeAnimation)
                {
                    ShapeData = shapeLoader.Shapes[Shape][State];
                }

                return direction;
            }
            return Directions.None;
        }

        private bool TrySetNextState(Animations animation)
        {
            if(_animations.TryGetValue(animation, out var states))
            {
                var index = Array.IndexOf(states, State) + 1;
                State = index < states.Length ? states[index] : states[0];
                return true;
            }
            return false;
        }
    }
}