using System.Collections.Concurrent;

namespace GameAPI.DSL
{
    public abstract class PlayerScript
    {
        public bool IsActive { get; private set; }
        private Thread? t_script;

        protected abstract void Do(GameWorld gameWorld, ConcurrentDictionary<string, (Types, object)> parameters);

        public void Invoke(GameWorld gameWorld, ConcurrentDictionary<string, (Types, object)> parameters)
        {
            try
            {
                IsActive = true;
                var starter = new ThreadStart(() =>
                {
                    try
                    {
                        Do(gameWorld, parameters);
                        IsActive = false;
                    }
                    catch
                    {

                    }
                });

                t_script = new(starter);
                t_script.Start();
            }
            catch
            {

            }
        }

        public void Abort()
        {
            try
            {
                t_script?.Interrupt();
                IsActive = false;
            }
            catch//(SecurityException e)
            {

            }
        }
    }
}
