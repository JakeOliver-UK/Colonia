using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Colonia.Engine
{
    internal abstract class Scene : IDisposable
    {
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;
        public bool IsActive => _isActive;

        private bool _isActive = false;

        public virtual void Initialize()
        {
            _isActive = true;
        }

        public virtual void Update() { }
        
        public virtual void Draw()
        {
            App.Instance.GraphicsDevice.Clear(BackgroundColor);

            App.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            DrawWorld();
            App.Instance.SpriteBatch.End();

            App.Instance.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);
            DrawOverlay();
            App.Instance.SpriteBatch.End();
        }

        public virtual void DrawWorld()
        {

        }
        
        public virtual void DrawOverlay()
        {
            
        }
        
        public virtual void Dispose()
        {
            if (_isActive)
            {
                _isActive = false;
            }
        }
    }
}
