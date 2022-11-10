using GameAPI;
using SFML.Graphics;
using SFML.Window;

namespace Game.Graphics.GUI
{
    internal abstract class Page
    {
        internal abstract void Draw(RenderWindow window, GameWorld? world);
        internal abstract bool HandleInput(KeyEventArgs args);
        internal abstract void Reset();
    }
}
