using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Colonia.Engine
{
    internal abstract class Scene : IDisposable
    {
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;
        public ContentManager ContentManager => _contentManager;
        public Dictionary<string, Texture2D> Textures => _textures;
        public Dictionary<string, Sprite> Sprites => _sprites;
        public Camera Camera => _camera;

        private ContentManager _contentManager;
        private Dictionary<string, Texture2D> _textures;
        private Dictionary<string, Sprite> _sprites;
        private Camera _camera;

        public virtual void Initialize()
        {
            _contentManager = new ContentManager(App.Instance.Services, "Content");
            _textures = [];
            _camera = new Camera
            {
                ViewSize = new Point(App.Instance.RenderTarget.Width, App.Instance.RenderTarget.Height)
            };
            _sprites = [];
            LoadContent();
        }

        public virtual void LoadContent() { }
        public virtual void Update() { }

        public virtual void Draw()
        {
            App.Instance.GraphicsDevice.SetRenderTarget(App.Instance.RenderTarget);
            App.Instance.GraphicsDevice.Clear(BackgroundColor);

            App.Instance.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, _camera.GetViewMatrix());
            DrawWorld();
            App.Instance.SpriteBatch.End();

            App.Instance.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            DrawUI();
            App.Instance.SpriteBatch.End();

            App.Instance.GraphicsDevice.SetRenderTarget(null);
            App.Instance.GraphicsDevice.Clear(Color.Black);

            App.Instance.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
            App.Instance.SpriteBatch.Draw(App.Instance.RenderTarget, App.Instance.Destination, Color.White);
            App.Instance.SpriteBatch.End();
        }

        public virtual void DrawWorld() { }
        public virtual void DrawUI() { }

        public virtual void UnloadContent()
        {
            _contentManager?.Unload();
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnloadContent();
                _contentManager?.Dispose();
            }
        }
    }
}
