using Colonia.Engine;
using Colonia.Engine.Managers;
using Colonia.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Colonia
{
    public class App : Game
    {
        public static App Instance => _instance;
        public GraphicsDeviceManager GraphicsDeviceManager => _graphicsDeviceManager;
        public SpriteBatch SpriteBatch => _spriteBatch;
        public RenderTarget2D RenderTarget => _renderTarget;
        public Rectangle Destination => _destination;
        public const int VirtualWidth = 640;
        public const int VirtualHeight = 360;

        private static App _instance;
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;
        private Rectangle _destination;

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

            _graphicsDeviceManager.PreferredBackBufferWidth = 1920;
            _graphicsDeviceManager.PreferredBackBufferHeight = 1080;
            _graphicsDeviceManager.IsFullScreen = true;
            _graphicsDeviceManager.ApplyChanges();

            base.Initialize();

            SceneManager.Add("MainScene", new MainScene());
            SceneManager.Switch("MainScene");

            Log.WriteLine(LogLevel.Info, $"Application initialized.");
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _renderTarget = new RenderTarget2D(GraphicsDevice, VirtualWidth, VirtualHeight);
        }

        protected override void Update(GameTime gameTime)
        {
            Time.Update(gameTime);
            SceneManager.Current?.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            UpdateDestination();
            SceneManager.Current?.Draw();

            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            _renderTarget?.Dispose();
            _spriteBatch?.Dispose();
        }

        private void UpdateDestination()
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            int scale = Math.Max(1, Math.Min(
                pp.BackBufferWidth / VirtualWidth,
                pp.BackBufferHeight / VirtualHeight));

            int width = VirtualWidth * scale;
            int height = VirtualHeight * scale;

            _destination = new Rectangle(
                (pp.BackBufferWidth - width) / 2,
                (pp.BackBufferHeight - height) / 2,
                width,
                height);
        }

        public Point ScreenToVirtual(Point screen) => new(
            (screen.X - _destination.X) * VirtualWidth / _destination.Width,
            (screen.Y - _destination.Y) * VirtualHeight / _destination.Height);

        protected override void OnExiting(object sender, ExitingEventArgs args)
        {
            Log.WriteLine(LogLevel.Info, $"Exiting application...");

            SceneManager.Current?.Dispose();

            base.OnExiting(sender, args);
            
            Log.WriteLine(LogLevel.Info, $"Application exited.");
        }
    }
}
