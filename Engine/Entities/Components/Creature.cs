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
        public bool IsMoving { get; set; } = false;

        public override void Update()
        {
            Vector2 movement = Vector2.Zero;

            if (App.Instance.Input.IsKeyPressed(Keys.Up))
            {
                Type++;
                if (Type > CreatureType.Orc) Type = CreatureType.Dwarf;
            }

            if (App.Instance.Input.IsKeyPressed(Keys.Down))
            {
                Type--;
                if (Type < CreatureType.Dwarf) Type = CreatureType.Orc;
            }

            if (App.Instance.Input.IsKeyDown(Keys.W))
            {
                Facing = CreatureFacing.Up;
                IsMoving = true;
                movement.Y -= 1.0f;
            }
            if (App.Instance.Input.IsKeyDown(Keys.S))
            {
                Facing = CreatureFacing.Down;
                IsMoving = true;
                movement.Y += 1.0f;
            }
            if (App.Instance.Input.IsKeyDown(Keys.A))
            {
                Heading = CreatureHeading.Left;
                IsMoving = true;
                movement.X -= 1.0f;
            }
            if (App.Instance.Input.IsKeyDown(Keys.D))
            {
                Heading = CreatureHeading.Right;
                IsMoving = true;
                movement.X += 1.0f;
            }
            if (App.Instance.Input.IsKeyUp(Keys.W) && App.Instance.Input.IsKeyUp(Keys.S) && App.Instance.Input.IsKeyUp(Keys.A) && App.Instance.Input.IsKeyUp(Keys.D))
            {
                IsMoving = false;
            }

            if (movement != Vector2.Zero)
            {
                movement.Normalize();
                Entity.Transform.Position += movement * MovementSpeed * Time.Delta;
            }

            string creature = Type.ToString();

            string animationName;
            if (IsMoving) animationName = "Walk";
            else animationName = "Idle";

            string direction;
            if (Heading == CreatureHeading.Left && Facing == CreatureFacing.Up) direction = "UpLeft";
            else if (Heading == CreatureHeading.Left && Facing == CreatureFacing.Down) direction = "DownLeft";
            else if (Heading == CreatureHeading.Right && Facing == CreatureFacing.Up) direction = "UpRight";
            else direction = "DownRight";

            string animationKey = $"{creature}{animationName}_{direction}";

            Entity.GetComponent<SpriteRenderer>().Sprite = animationKey;
        }
    }

    internal enum CreatureType
    {
        Dwarf,
        Elf,
        Goblin,
        Halfling,
        Human,
        Orc
    }

    internal enum CreatureHeading
    {
        Left,
        Right
    }

    internal enum CreatureFacing
    {
        Up,
        Down
    }
}
