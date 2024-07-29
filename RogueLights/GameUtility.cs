using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

public sealed class GameUtility
{
    private static GameUtility instance = null;
    private static readonly object _lock = new object();

    public static ContentManager ContentManager { get; private set; }

    public static SpriteBatch SpriteBatch { get; private set; }

    public static GameUtility Instance
    {
        get
        {
            lock (_lock)
            {
                if (instance == null)
                {
                    instance = new GameUtility();
                }    

                return instance;
            }
        }
    }

    public static void Initialize(ContentManager contentManager, SpriteBatch spriteBatch)
    {
        ContentManager = contentManager;
        SpriteBatch = spriteBatch;
    }
}