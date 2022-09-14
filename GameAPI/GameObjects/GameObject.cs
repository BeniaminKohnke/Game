using System.Collections.Concurrent;

namespace GameAPI.GameObjects
{
    public class GameObject : Rectangle
    {
        private static uint s_lastId = 1;
        protected readonly ConcurrentQueue<Directions> _movement = new();
        protected readonly ConcurrentDictionary<Animations, States[]> _animations = new();
        public Dictionary<ObjectsParameters, object> ObjectParameters { get; set; } = new();

        public uint Id { get; } = s_lastId++;
        public virtual Grids Grid { get; protected set; }
        public virtual States State { get; protected set; } = States.NoAction1;
        public virtual Types ObjectType { get; protected set; }
        public virtual Directions LastDirection { get; set; } = Directions.None;
        public bool IsActive { get; set; } = true;

        public GameObject(GridLoader loader, int x, int y, Types type, Grids grid) : base(loader.GetGrid(grid, States.NoAction1), x, y)
        {
            ObjectType = type;
            Grid = grid;

            var states = loader.GetStates(grid);
            if (states != null)
            {
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
        }

        public virtual void Update(double deltaTime, GridLoader loader)
        {

        }

        public virtual void EnqueueMovement(Directions direction) => _movement.Enqueue(direction);

        public Directions DequeueMovement(GridLoader loader)
        {
            if (_movement.TryDequeue(out var direction))
            {
                var changeAnimation = false;
                var lastDirection = Directions.None;
                switch (direction)
                {
                    case Directions.Up:
                        changeAnimation = TrySetNextState(Animations.MovingRight);
                        lastDirection = Directions.Up;
                        break;
                    case Directions.Down:
                        changeAnimation = TrySetNextState(Animations.MovingLeft);
                        lastDirection = Directions.Down;
                        break;
                    case Directions.Left:
                        changeAnimation = TrySetNextState(Animations.MovingLeft);
                        lastDirection = Directions.Left;
                        break;
                    case Directions.Right:
                        changeAnimation = TrySetNextState(Animations.MovingRight);
                        lastDirection = Directions.Right;
                        break;
                }

                if (changeAnimation)
                {
                    LastDirection = lastDirection;
                    SetGrid(loader.GetGrid(Grid, State));
                }

                return direction;
            }
            return Directions.None;
        }

        protected bool TrySetNextState(Animations animation)
        {
            if (_animations.TryGetValue(animation, out var states))
            {
                var index = Array.IndexOf(states, State) + 1;
                State = states[index < states.Length ? index : 0];
                return true;
            }
            return false;
        }

        protected void ResetState(Animations animation, int frame = 0)
        {
            if (_animations.TryGetValue(animation, out var states))
            {
                State = states[states.Length >= frame && frame > 0 ? frame - 1 : 0];
            }
        }
    }
}