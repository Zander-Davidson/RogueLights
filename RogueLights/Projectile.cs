using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace RogueLights
{
    public class Projectile : GameEntity
    {
        int Width;
        int Height;

        public Projectile(Texture2D texture, int rows, int columns, float frameRate, Vector2 initialPosition, int hitBoxWidth, int hitBoxHeight) : base(texture, rows, columns, frameRate, initialPosition, hitBoxWidth, hitBoxHeight)
        {
            Texture = texture;
            Width = Texture.Width;
            Height = Texture.Height;
            IsAsleep = true;
        }

        public void Initialize(Vector2 initialPosition, Vector2 velocity)
        {
            Position = initialPosition;
            Velocity = velocity;
            IsAsleep = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime, () =>
            {
                if (!IsAsleep)
                {
                    Position.X += Velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    Position.Y += Velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (0 > Position.X + Width || Position.X > Game1.SceneWidth ||
                    0 > Position.Y + Height || Position.Y > Game1.SceneHeight)
                {
                    IsAsleep = true;
                }
            });
        }
    }
}
