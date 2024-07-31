using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace RogueLights
{
    public class Game1 : Game
    {
        Texture2D BulletTexture;
        RecycleableAssetCollection<Projectile> Projectiles;
        TimeSpan fireRate = new TimeSpan(3500000);
        TimeSpan lastProjectileTime = TimeSpan.Zero;
        float projectileSpeedConstant = 400f;

        Texture2D BadGuyTexture;
        List<BadGuy> BadGuys = new List<BadGuy>();
        TimeSpan lastBadGuySpawnTime = TimeSpan.Zero;
        float BadGuySpawnRate = 1.7f; // per second

        Texture2D BasicExplostionTexture;
        List<GameEffect> BasicExplosions = new List<GameEffect>();
        int nextBasicExplosion = 0;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private int _score = 0;

        private GameEntity Player;

        private Texture2D arrow;
        private Vector2 arrowPosition;
        private float arrowPositionConstant = 50f;
        private Vector2 rightThumbUnitVector = new Vector2(1, 0);

        public static int SceneWidth;
        public static int SceneHeight;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            GameUtility.Initialize(Content, _spriteBatch);

            SceneWidth = _graphics.PreferredBackBufferWidth;
            SceneHeight = _graphics.PreferredBackBufferHeight;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            // sprite font
            _font = Content.Load<SpriteFont>("Score");

            // sprite animation

            Texture2D texture = Content.Load<Texture2D>("goodguy");
            Player = new GameEntity(texture, 2, 2, 12, new Vector2(SceneWidth / 2, SceneHeight / 2), 32, 48);

            BadGuyTexture = Content.Load<Texture2D>("badguy");

            BasicExplostionTexture = Content.Load<Texture2D>("BasicExplosion");
            foreach (int i in Enumerable.Range(0, 1000))
            {
                BasicExplosions.Add(new GameEffect(BasicExplostionTexture, 2, 2, 12, Vector2.Zero, true, true));
            }

            arrow = Content.Load<Texture2D>("arrow");

            BulletTexture = Content.Load<Texture2D>("BombShot");
            Projectiles = new RecycleableAssetCollection<Projectile>(
                () => new Projectile(BulletTexture, 2, 2, 24, Vector2.Zero, 24, 24),
                1000
            );
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here

            HandleGamePadInput(gameTime);

            arrowPosition.X = (Player.Position.X + Player.frameWidth / 2) + (rightThumbUnitVector.X * arrowPositionConstant);
            arrowPosition.Y = (Player.Position.Y + Player.frameHeight / 2) + (rightThumbUnitVector.Y * arrowPositionConstant);

            Player.Update(gameTime);
            
            foreach (var bg in BadGuys)
            {
                bg.Update(gameTime, Player.Position);

                if (Player.CollidesWithEntity(bg))
                {
                    Player.Teardown();
                    arrow.Dispose();
                }
            }            
            
            foreach (var p in Projectiles.Instances)
            {
                p.Update(gameTime);

                foreach (var bg in BadGuys)
                {
                    if (p.CollidesWithEntity(bg))
                    {
                        p.Teardown();
                        bg.Teardown();

                        if (nextBasicExplosion >= BasicExplosions.Count)
                        {
                            nextBasicExplosion = 0;
                        }

                        BasicExplosions[nextBasicExplosion].Initialize(false, bg.Position, Vector2.Zero);

                        nextBasicExplosion++;

                        // TODO: figure out how to properly remove bad guys from the list

                        _score++;
                    }
                }
            }

            foreach (var be in BasicExplosions)
            {
                be.Update(gameTime);
            }

            SpawnBadGuys(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            foreach (var p in Projectiles.Instances)
            {
                p.Draw(_spriteBatch);
            }

            foreach (var bg in BadGuys)
            {
                bg.Draw(_spriteBatch);
            }

            foreach (var be in BasicExplosions)
            {
                be.Draw(_spriteBatch);
            }

            Player.Draw(_spriteBatch);

            _spriteBatch.Begin();
            _spriteBatch.Draw(
                arrow,
                arrowPosition,
                null,
                Color.White,
                (float)(Math.Atan2(rightThumbUnitVector.Y, rightThumbUnitVector.X) + Math.PI / 2),
                MyMath.GetTextureRelativeCenter(arrow),
                1f,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.DrawString(_font, $"{_score}", new Vector2(10, 10), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void HandleGamePadInput(GameTime gameTime)
        {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);

            if (capabilities.IsConnected)
            {
                GamePadState state = OneShotXboxButton.GetState();

                // left thumb stick
                if (capabilities.HasLeftXThumbStick)
                {
                    Player.Position.X += state.ThumbSticks.Left.X * 10.0f;
                }

                if (capabilities.HasLeftYThumbStick)
                {
                    Player.Position.Y -= state.ThumbSticks.Left.Y * 10.0f;
                }

                // right thimb stick
                if (capabilities.HasRightXThumbStick && capabilities.HasRightYThumbStick
                    && (state.ThumbSticks.Right.X != 0 || state.ThumbSticks.Right.Y != 0)) 
                {
                    rightThumbUnitVector = MyMath.GetUnitVector(new Vector2(state.ThumbSticks.Right.X, -1 * state.ThumbSticks.Right.Y));    
                }

                // right trigger

                // 0.1 is threshold to prevent drift
                if (capabilities.HasRightTrigger && state.Triggers.Right > 0.1 && fireRate + lastProjectileTime < gameTime.TotalGameTime)
                {
                    Projectiles.InitializeNextInstance(
                        false,
                        new Vector2(Player.Position.X + Player.frameWidth / 2 - BulletTexture.Width / 2, Player.Position.Y + Player.frameHeight / 2 - BulletTexture.Height / 2),
                        new Vector2(rightThumbUnitVector.X * projectileSpeedConstant, rightThumbUnitVector.Y * projectileSpeedConstant)
                    );

                    lastProjectileTime = gameTime.TotalGameTime;
                }
            }
        }

        private void SpawnBadGuys(GameTime gameTime)
        {
            if (lastBadGuySpawnTime.Ticks + TimeSpan.TicksPerSecond / BadGuySpawnRate < gameTime.TotalGameTime.Ticks)
            {
                Random rnd = new Random();

                int randomSceneEdge = rnd.Next(0, 4);
                Vector2 newBadGuyPosition;

                switch (randomSceneEdge)
                {
                    case 0:
                        newBadGuyPosition = new Vector2(0, rnd.Next(0, SceneHeight));
                        break;

                    case 1:
                        newBadGuyPosition = new Vector2(rnd.Next(0, SceneWidth), 0);
                        break;

                    case 2:
                        newBadGuyPosition = new Vector2(SceneWidth, rnd.Next(0, SceneHeight));
                        break;

                    case 3:
                        newBadGuyPosition = new Vector2(rnd.Next(0, SceneWidth), SceneHeight);
                        break;

                    default:
                        newBadGuyPosition = Vector2.Zero;
                        break;
                }

                BadGuys.Add(new BadGuy(BadGuyTexture, 2, 2, 12, newBadGuyPosition, 32, 48));

                lastBadGuySpawnTime = gameTime.TotalGameTime;
            }
        }
    }
}
