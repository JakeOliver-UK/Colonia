using Colonia.Engine;
using Colonia.Engine.Managers;
using Colonia.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Colonia
{
    public class App : Game
    {
        public static App Instance => _instance;
        public GraphicsDeviceManager GraphicsDeviceManager => _graphicsDeviceManager;
        public SpriteBatch SpriteBatch => _spriteBatch;

        private static App _instance;
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;

        public App()
        {
            _instance = this;
            _graphicsDeviceManager = new(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Log.WriteLine(LogLevel.Info, $"Initializing application...");

            base.Initialize();

            SceneManager.Add("MainScene", new MainScene());
            SceneManager.Switch("MainScene");

            Log.WriteLine(LogLevel.Info, $"Application initialized.");
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            SceneManager.Current?.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            SceneManager.Current?.Draw();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, ExitingEventArgs args)
        {
            Log.WriteLine(LogLevel.Info, $"Exiting application...");

            SceneManager.Current?.Dispose();

            base.OnExiting(sender, args);
            
            Log.WriteLine(LogLevel.Info, $"Application exited.");
        }
    }
}
