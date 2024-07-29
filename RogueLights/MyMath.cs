using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace RogueLights
{
    public class MyMath
    {
        public static Vector2 GetTextureAbsoluteCenter(Texture2D texture, Vector2 position)
        {
            return GetTextureAbsoluteCenter(texture.Width, texture.Height, position);
        }

        public static Vector2 GetTextureAbsoluteCenter(float textureWidth, float textureHeight, Vector2 position)
        {
            return new Vector2(position.X + textureWidth / 2, position.Y + textureHeight / 2);
        }

        public static Vector2 GetTextureRelativeCenter(Texture2D texture)
        {
            return new Vector2(texture.Width / 2, texture.Height / 2);
        }

        public static float Magnitude(Vector2 v)
        {
            return (float) Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2));
        }

        public static Vector2 GetUnitVector(Vector2 v)
        {
            return new Vector2(v.X / Magnitude(v), v.Y / Magnitude(v));
        }

    }
}
