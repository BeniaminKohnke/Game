using System.Collections.Concurrent;

namespace GameAPI
{
    public class GameObject : Rectangle
    {
        private static uint _lastId = 0;
        public readonly uint Id = _lastId++;

        public Dictionary<ObjectsParameters, object> ObjectParameters { get; set; } = new();
        private readonly ConcurrentQueue<Directions> _movement = new();
        private readonly ConcurrentDictionary<Animations, States[]> _animations = new();

        public Grids Grid { get; private set; }
        public States State { get; private set; } = States.NoAction1;
        public Types ObjectType { get; private set; }
        public bool IsActive { get; set; } = true;

        public GameObject(GridLoader loader, int x, int y, Types type, Grids grid) : base(loader.GetGrid(grid, States.NoAction1), x, y)
        {
            ObjectType = type;
            Grid = grid;
            
            var states = loader.GetStates(grid);
            if(states != null)
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

        public void EnqueueMovement(Directions direction) => _movement.Enqueue(direction);

        public Directions DequeueMovement(GridLoader shapeLoader)
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
                    SetGrid(shapeLoader.GetGrid(Grid, State));
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