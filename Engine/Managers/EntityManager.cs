using Colonia.Engine.Entities;
using Colonia.Engine.Utils;
using System;

namespace Colonia.Engine.Managers
{
    internal class EntityManager : IDisposable
    {
        public bool IsOverlayManager => _isOverlayManager;

        private Entity[] _entities;
        private readonly bool _isOverlayManager = false;

        public EntityManager(bool isOverlayManager)
        {
            _isOverlayManager = isOverlayManager;
            Initialize(1000);
        }

        public EntityManager(bool isOverlayManager, int count)
        {
            _isOverlayManager = isOverlayManager;
            Initialize(count);
        }

        public void Initialize(int count)
        {
            _entities = new Entity[count];
            for (int i = 0; i < count; i++)
            {
                _entities[i] = new();
            }
        }

        public Entity[] GetAllActive() => Array.FindAll(_entities, e => e != null && e.IsActive);
        public Entity[] GetAllInactive() => Array.FindAll(_entities, e => e != null && !e.IsActive);

        public Entity Get(string name)
        {
            Entity[] activeEntities = GetAllActive();
            for (int i = 0; i < activeEntities.Length; i++)
            {
                if (activeEntities[i].Name == name) return activeEntities[i];
            }
            return null;
        }

        public bool TryGet(string name, out Entity entity)
        {
            entity = Get(name);
            return entity != null;
        }

        public bool Exists(string name)
        {
            Entity[] activeEntities = GetAllActive();
            for (int i = 0; i < activeEntities.Length; i++)
            {
                if (activeEntities[i].Name == name) return true;
            }
            return false;
        }

        public Entity Create(string name)
        {
            Entity entity = GetInactive();
            if (entity == null)
            {
                Log.WriteLine(LogLevel.Error, $"Unable to create entity '{name}' as no inactive entities available in pool.");
                return null;
            }
            entity.Name = name;
            entity.Initialize(this);
            return entity;
        }

        public Entity GetInactive() => Array.Find(_entities, e => e != null && !e.IsActive);

        public void Update()
        {
            Entity[] activeEntities = GetAllActive();
            for (int i = 0; i < activeEntities.Length; i++)
            {
                activeEntities[i].Update();
            }
        }

        public void Draw()
        {
            Entity[] activeEntities = GetAllActive();
            for (int i = 0; i < activeEntities.Length; i++)
            {
                activeEntities[i].Draw();
            }
        }

        public void Dispose()
        {
            Entity[] activeEntities = GetAllActive();
            for (int i = 0; i < activeEntities.Length; i++)
            {
                activeEntities[i].Dispose();
            }
            _entities = null;
        }
    }
}
