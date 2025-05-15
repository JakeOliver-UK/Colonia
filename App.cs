using Colonia.Engine.Managers;
using Colonia.Engine.Utils;
using Colonia.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Colonia
{
    internal class App : Game
    {
        public static App Instance => _instance;
        public GraphicsDeviceManager GraphicsDeviceManager => _graphicsDeviceManager;
        public SpriteBatch SpriteBatch => _spriteBatch;
        public SceneManager SceneManager => _sceneManager;
        public SettingsManager SettingsManager => _settingsManager;

        private static App _instance;
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private SettingsManager _settingsManager;
        private SceneManager _sceneManager;

        public App()
        {
            _instance = this;
            _graphicsDeviceManager = new(this);
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Log.WriteLine(LogLevel.Info, "Initializing application...");

            _settingsManager = new();

            base.Initialize();

            _sceneManager = new();
            _sceneManager.Add("Main", new MainScene());
            _sceneManager.Display("Main");

            Log.WriteLine(LogLevel.Info, "Application initialized.");
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            FPS.Update();
            _sceneManager.Current.Update();
            Window.Title = _settingsManager.Settings.ShowFrameRate ? $"{AppInfo.Name} - v{AppInfo.Version} - FPS: {FPS.Current:n0}" : $"{AppInfo.Name} - v{AppInfo.Version}";

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _sceneManager.Current.Draw();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            Log.WriteLine(LogLevel.Info, "Shutting down application...");
            
            _spriteBatch.Dispose();
            _spriteBatch = null;

            base.UnloadContent();

            _sceneManager.Dispose();
            _sceneManager = null;

            _settingsManager.Dispose();
            _settingsManager = null;

            Log.WriteLine(LogLevel.Info, "Application shut down.");
        }
    }
}
