using Colonia.Engine.Managers;
using Colonia.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Colonia.Engine.Entities.Components
{
    internal class Creature : Component
    {
        public CreatureType Type { get; set; } = CreatureType.Human;
        public CreatureHeading Heading { get; set; } = CreatureHeading.Right;
        public CreatureFacing Facing { get; set; } = CreatureFacing.Down;
        public float MovementSpeed { get; set; } = 100.0f;
        public bool IsMoving { get; private set; } = false;

        private Vector2 _targetPosition;
        private const float _arrivalThreshold = 2.0f;

        public override void Update()
        {
            if (App.Instance.Input.IsKeyPressed(Keys.Q)) CycleType(1);
            if (App.Instance.Input.IsKeyPressed(Keys.E)) CycleType(-1);

            if (App.Instance.Input.IsMouseButtonPressed(MouseButton.Left))
            {
                _targetPosition = App.Instance.SceneManager.Current.Camera.ScreenToWorld(App.Instance.Input.MousePosition);
                IsMoving = true;
            }

            Vector2 currentPos = Entity.Transform.Position;
            Vector2 toTarget = _targetPosition - currentPos;

            if (IsMoving && toTarget.LengthSquared() > _arrivalThreshold * _arrivalThreshold)
            {
                Vector2 movement = Vector2.Normalize(toTarget);
                UpdateFacingAndHeading(movement);
                Entity.Transform.Position += movement * MovementSpeed * Time.Delta;
            }
            else IsMoving = false;

            string state = IsMoving ? "Walk" : "Idle";
            string dirString = (Heading == CreatureHeading.Left, Facing == CreatureFacing.Up) switch
            {
                (true, true) => "UpLeft",
                (true, false) => "DownLeft",
                (false, true) => "UpRight",
                _ => "DownRight"
            };
            string animKey = $"{Type}{state}_{dirString}";
            Entity.GetComponent<SpriteRenderer>().Sprite = animKey;
        }

        private void CycleType(int delta)
        {
            Type = (CreatureType)(((int)Type + delta + 6) % 6);
        }

        private void UpdateFacingAndHeading(Vector2 movement)
        {
            //if (Math.Abs(movement.Y) > Math.Abs(movement.X)) Facing = movement.Y < 0 ? CreatureFacing.Up : CreatureFacing.Down;
            Facing = CreatureFacing.Down;
            Heading = movement.X < 0 ? CreatureHeading.Left : CreatureHeading.Right;
        }
    }

    internal enum CreatureType { Dwarf, Elf, Goblin, Halfling, Human, Orc }
    internal enum CreatureHeading { Left, Right }
    internal enum CreatureFacing { Up, Down }
}