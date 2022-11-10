﻿namespace GameAPI.GameObjects
{
    public class GameObject : Rectangle
    {
        private static uint s_lastId = 1;
        protected readonly Queue<Directions> _movement = new();
        protected readonly Dictionary<Animations, States[]> _animations = new();
        public uint Id { get; } = s_lastId++;
        public Dictionary<ObjectsParameters, object> ObjectParameters { get; set; } = new();
        public virtual Grids Grid { get; protected set; }
        public virtual States State { get; protected set; } = States.NoAction1;
        public virtual Types ObjectType { get; protected set; }
        public virtual Directions LastDirection { get; set; } = Directions.None;
        public bool IsActive { get; set; } = true;
        public bool HasChanged { get; set; } = false;
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

        public virtual void Update(float deltaTime, GridLoader loader)
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
                    case Directions.North:
                        changeAnimation = TrySetNextState(Animations.MovingRight);
                        lastDirection = Directions.North;
                        break;
                    case Directions.South:
                        changeAnimation = TrySetNextState(Animations.MovingLeft);
                        lastDirection = Directions.South;
                        break;
                    case Directions.West:
                        changeAnimation = TrySetNextState(Animations.MovingLeft);
                        lastDirection = Directions.West;
                        break;
                    case Directions.East:
                        changeAnimation = TrySetNextState(Animations.MovingRight);
                        lastDirection = Directions.East;
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