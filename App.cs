using Colonia.Engine;
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
        public AssetManager AssetManager => _assetManager;
        public InputManager Input => _input;
        public Sprite Cursor { get; set; }

        private static App _instance;
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private SettingsManager _settingsManager;
        private SceneManager _sceneManager;
        private AssetManager _assetManager;
        private InputManager _input;

        public App()
        {
            _instance = this;
            _graphicsDeviceManager = new(this);
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            Log.WriteLine(LogLevel.Info, "Initializing application...");

            _settingsManager = new();
            _input = new();

            base.Initialize();

            _sceneManager = new();
            _sceneManager.Add("Main", new MainScene());
            _sceneManager.Display("Main");

            Cursor = _assetManager.Sprites["Cursors"];

            Log.WriteLine(LogLevel.Info, "Application initialized.");
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _assetManager = new();
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            FPS.Update();
            _input.Update();
            _sceneManager.Current.Update();
            Window.Title = _settingsManager.Settings.ShowFrameRate ? $"{AppInfo.Name} - v{AppInfo.Version} - FPS: {FPS.Current:n0} - Screen: {Input.MousePosition} - World: {SceneManager.Current.Camera.ScreenToWorld(Input.MousePosition)}" : 
                $"{AppInfo.Name} - v{AppInfo.Version}";

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _sceneManager.Current.Draw();

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
            SpriteBatch.Draw(_assetManager.Images.Get(Cursor.Images[Cursor.CurrentFrame]), Input.MousePosition, Cursor.Frames[Cursor.CurrentFrame], Color.White, 0f, Vector2.Zero, new Vector2(2.0f, 2.0f), SpriteEffects.None, 1.0f);
            SpriteBatch.End();

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

            _assetManager.Dispose();
            _assetManager = null;

            _settingsManager.Dispose();
            _settingsManager = null;

            Log.WriteLine(LogLevel.Info, "Application shut down.");
        }
    }
}
