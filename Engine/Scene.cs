using Colonia.Engine.Entities;
using Colonia.Engine.Entities.Components;
using Colonia.Engine.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Colonia.Engine
{
    internal abstract class Scene : IDisposable
    {
        public Color BackgroundColor { get; set; } = Color.CornflowerBlue;
        public Tilemap Tilemap { get; set; }
        public bool IsActive => _isActive;
        public EntityManager WorldEntityManager => _worldEntityManager;
        public EntityManager OverlayEntityManager => _overlayEntityManager;
        public Camera Camera => _camera;

        private bool _isActive = false;
        private EntityManager _worldEntityManager;
        private EntityManager _overlayEntityManager;
        private Camera _camera;

        public virtual void Initialize()
        {
            _isActive = true;
            _worldEntityManager = new(false);
            _overlayEntityManager = new(true);
            Entity cameraEntity = _worldEntityManager.Create("Camera");
            _camera = cameraEntity.AddComponent<Camera>();
        }

        public virtual void Update()
        {
            _worldEntityManager.Update();
            _overlayEntityManager.Update();
        }
        
        public virtual void Draw()
        {
            App.Instance.GraphicsDevice.Clear(BackgroundColor);

            App.Instance.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied, transformMatrix: _camera.Transform);
            DrawWorld();
            App.Instance.SpriteBatch.End();

            App.Instance.SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.NonPremultiplied);
            DrawOverlay();
            App.Instance.SpriteBatch.End();
        }

        public virtual void DrawWorld()
        {
            Tilemap?.Draw();
            _worldEntityManager.Draw();
        }
        
        public virtual void DrawOverlay()
        {
            _overlayEntityManager.Draw();
        }
        
        public virtual void Dispose()
        {
            if (_isActive)
            {
                _isActive = false;
                _worldEntityManager.Dispose();
                _overlayEntityManager.Dispose();
            }
        }
    }
}
