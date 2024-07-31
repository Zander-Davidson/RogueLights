using Microsoft.Xna.Framework;

namespace RogueLights
{
    public interface IInitializeable
    {
        public void Initialize(bool isAsleep, Vector2 initialPosition, Vector2 velocity);
    }
}
