using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace RogueLights
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;

        public int frameWidth;
        public int frameHeight;

        float FrameRate; // per second

        TimeSpan LastFrameTime = TimeSpan.Zero;

        public bool IsOneShot;
        public bool IsAsleep;

        public AnimatedSprite(Texture2D texture, int rows, int columns, float frameRate, bool isAsleep = false, bool isOneShot = false)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            frameWidth = Texture.Width / Columns;
            frameHeight = Texture.Height / Rows;
            FrameRate = frameRate;
            IsOneShot = isOneShot;
            IsAsleep = isAsleep;
        }

        public void Teardown()
        {
            IsAsleep = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsAsleep && LastFrameTime.Ticks + TimeSpan.TicksPerSecond / FrameRate < gameTime.TotalGameTime.Ticks)
            {
                currentFrame++;

                if (!IsOneShot && currentFrame == totalFrames)
                {
                    currentFrame = 0;
                }

                LastFrameTime = gameTime.TotalGameTime;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            if (currentFrame < totalFrames || !IsOneShot)
            {
                int row = currentFrame / Columns;
                int column = currentFrame % Columns;

                Rectangle sourceRectangle = new Rectangle(frameWidth * column, frameHeight * row, frameWidth, frameHeight);
                Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, frameWidth, frameHeight);

                spriteBatch.Begin();
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
                spriteBatch.End();
            }
        }
    }
}
