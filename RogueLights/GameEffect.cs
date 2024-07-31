using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RogueLights
{
    public class GameEffect : AnimatedSprite, IInitializeable
    {
        public Vector2 Position;
        public Vector2 Velocity;

        public GameEffect(Texture2D texture, int rows, int columns, float frameRate, Vector2 initialPosition, bool isAsleep = false, bool isOneShot = false) : base(texture, rows, columns, frameRate, isAsleep, isOneShot) 
        {
            Position = initialPosition;
        }

        public void Initialize(bool isAsleep, Vector2 initialPosition, Vector2 velocity)
        {
            IsAsleep = isAsleep;
            Position = initialPosition;
            Velocity = velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsAsleep)
            {
                base.Draw(spriteBatch, Position);
            }
        }
    }
}
