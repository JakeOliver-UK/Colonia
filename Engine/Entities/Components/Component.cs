using System;

namespace Colonia.Engine.Entities.Components
{
    internal abstract class Component : IDisposable
    {
        public Entity Entity => _entity;

        private Entity _entity;

        public virtual void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public virtual void Update() { }
        public virtual void Draw() { }
        
        public virtual void Dispose()
        {
            _entity = null;
        }
    }
}
