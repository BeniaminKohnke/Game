using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.GUI
{
    internal abstract class Page
    {
        internal abstract void Draw(RenderWindow window, GameWorld world);
        internal abstract void HandleInput(KeyEventArgs args);
        internal abstract void Reset();
        internal abstract void Release();
    }
}
