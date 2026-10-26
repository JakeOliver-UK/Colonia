using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Colonia.Engine
{
    internal abstract class Scene : IDisposable
    {
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;
        public ContentManager ContentManager => _contentManager;

        private ContentManager _contentManager;

        public virtual void Initialize()
        {
            _contentManager = new ContentManager(App.Instance.Services, "Content");
            LoadContent();
        }

        public virtual void LoadContent() { }
        public virtual void Update() { }

        public virtual void Draw()
        {
            App.Instance.GraphicsDevice.Clear(BackgroundColor);

            App.Instance.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            DrawWorld();
            App.Instance.SpriteBatch.End();

            App.Instance.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            DrawUI();
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
