using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace RogueLights
{
    public class BadGuy : GameEntity
    {
        float SpeedConstant = 100f;

        public BadGuy(Texture2D texture, int rows, int columns, float frameRate, Vector2 initialPosition, int hitBoxWidth, int hitBoxHeight) : base(texture, rows, columns, frameRate, initialPosition, hitBoxWidth, hitBoxHeight)
        {

        }

        public void Update(GameTime gameTime, Vector2 playerPosition)
        {
            base.Update(gameTime, () =>
            {
                Vector2 unitVectorToPlayer = MyMath.GetUnitVector(playerPosition - Position);

                Position.X += unitVectorToPlayer.X * SpeedConstant * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Position.Y += unitVectorToPlayer.Y * SpeedConstant * (float)gameTime.ElapsedGameTime.TotalSeconds;
            });
        }
    }
}
