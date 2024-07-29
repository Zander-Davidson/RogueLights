using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using System;

namespace RogueLights
{
    public class GameEntity : AnimatedSprite
    {
        public Vector2 Position;
        public Rectangle HitBox;
        public bool IsAsleep = false;

        public GameEntity(Texture2D texture, int rows, int columns, float frameRate, Vector2 initialPosition, int hitBoxWidth, int hitBoxHeight) : base(texture, rows, columns, frameRate)
        {
            Position = initialPosition;
            HitBox = new Rectangle((int)initialPosition.X, (int)initialPosition.Y, hitBoxWidth, hitBoxHeight);
        }

        public bool CollidesWithEntity(GameEntity otherGameEntity)
        {
            if (IsAsleep || otherGameEntity.IsAsleep)
            {
                return false;
            }

            return HitBox.Intersects(otherGameEntity.HitBox);
        }

        public void Teardown()
        {
            IsAsleep = true;
        }

        public delegate void UpdateHandler();

        public override void Update(GameTime gameTime)
        {
            Update(gameTime, () => { });
        }

        // handler lets us surround child update logic with if(!IsAsleep) to avoid unnecessary computation
        public void Update(GameTime gameTime, UpdateHandler handler)
        {
            if (!IsAsleep)
            {
                // update location of hit box
                HitBox.X = (int)Position.X + ((frameWidth - HitBox.Width) / 2);
                HitBox.Y = (int)Position.Y + ((frameHeight - HitBox.Height) / 2);

                handler();

                base.Update(gameTime);
            }
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
