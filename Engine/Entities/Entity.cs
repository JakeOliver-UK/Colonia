using Colonia.Engine.Entities.Components;
using Colonia.Engine.Managers;
using System;
using System.Collections.Generic;

namespace Colonia.Engine.Entities
{
    internal class Entity : IDisposable
    {
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public bool IsActive => _isActive;
        public Entity Parent => _parent;
        public EntityManager Manager => _manager;

        private bool _isActive = false;
        private Entity _parent;
        private List<Entity> _children;
        private List<Component> _components;
        private EntityManager _manager;

        public void SetParent(Entity parent)
        {
            if (parent == null || parent == this || _children.Contains(parent)) return;
            _parent = parent;
        }

        public void RemoveParent()
        {
            if (_parent == null) return;
            _parent = null;
        }

        public bool IsRoot => _parent == null;
        public bool IsChildOf(Entity parent) => _parent == parent;
        public bool IsChildOf(string parent) => _parent.Name == parent;
        public bool IsChild => _parent != null;

        public void AddChild(Entity child)
        {
            if (child == null || child == this || _parent == child || _children.Contains(child)) return;
            _children.Add(child);
        }

        public bool HasChild(Entity child) => _children.Contains(child);
        public bool HasChild(string name) => GetChild(name) != null;

        public Entity GetChildAtIndex(int index)
        {
            if (index < 0 || index >= _children.Count) return null;
            else return _children[index];
        }

        public Entity GetChild(string name)
        {
            for (int i = 0; i < _children.Count; i++)
            {
                if (_children[i].Name == name) return _children[i];
            }
            return null;
        }

        public bool TryGetChild(string name, out Entity entity)
        {
            entity = GetChild(name);
            return entity != null;
        }

        public void RemoveChild(Entity child)
        {
            if (child == null || !_children.Contains(child)) return;
            _children.Remove(child);
        }

        public void RemoveChild(string name)
        {
            Entity child = GetChild(name);
            if (child == null) return;
            _children.Remove(child);
        }

        public void RemoveAllChildren()
        {
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].Dispose();
            }
            _children.Clear();
        }

        public T AddComponent<T>() where T : Component, new()
        {
            T component = new();
            if (HasComponent<T>()) return GetComponent<T>();
            _components.Add(component);
            component.Initialize(this);
            return component;
        }

        public bool HasComponent<T>() where T : Component
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T) return true;
            }
            return false;
        }

        public T GetComponent<T>() where T : Component
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T component) return component;
            }
            return null;
        }

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            component = GetComponent<T>();
            return component != null;
        }

        public void RemoveComponent<T>() where T : Component
        {
            for (int i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T component)
                {
                    component.Dispose();
                    _components.RemoveAt(i);
                    return;
                }
            }
        }

        public void RemoveComponent(Component component)
        {
            if (component == null || !_components.Contains(component)) return;
            component.Dispose();
            _components.Remove(component);
        }

        public void RemoveAllComponents()
        {
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Dispose();
            }
            _components.Clear();
        }

        public virtual void Initialize(EntityManager manager)
        {
            IsVisible = true;
            _children = [];
            _components = [];
            _manager = manager;
            _isActive = true;
        }

        public virtual void Update()
        {
            if (!_isActive) return;
            if (_components.Count == 0) return;
            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Update();
            }
        }
        
        public virtual void Draw()
        {
            if (!_isActive) return;
            if (!IsVisible) return;
            if (_components.Count == 0) return;

            for (int i = 0; i < _components.Count; i++)
            {
                _components[i].Draw();
            }
        }
        
        public virtual void Dispose()
        {
            Name = string.Empty;
            IsVisible = false;
            _parent = null;
            RemoveAllChildren();
            _children = null;
            RemoveAllComponents();
            _components = null;
            _isActive = false;
            _manager = null;
        }
    }
}
