namespace GameAPI
{
    public abstract class PlayerScript
    {
        public bool IsActive { get; private set; }
        private Thread? t_script;

        protected abstract void Do(GameWorld gameWorld, Parameters parameters);

        public void Invoke(GameWorld gameWorld, Parameters parameters)
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
