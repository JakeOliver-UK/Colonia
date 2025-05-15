using Colonia.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Colonia.Engine.Entities.Components
{
    internal class Creature : Component
    {
        public CreatureHeading Heading { get; set; } = CreatureHeading.Right;
        public CreatureFacing Facing { get; set; } = CreatureFacing.Down;
        public bool IsMoving { get; set; } = false;

        public override void Update()
        {
            if (App.Instance.Input.IsKeyDown(Keys.W))
            {
                Facing = CreatureFacing.Up;
                IsMoving = true;
                Entity.Transform.Position += new Vector2(0, -1) * Time.Delta * 100.0f;
            }
            if (App.Instance.Input.IsKeyDown(Keys.S))
            {
                Facing = CreatureFacing.Down;
                IsMoving = true;
                Entity.Transform.Position += new Vector2(0, 1) * Time.Delta * 100.0f;
            }
            if (App.Instance.Input.IsKeyDown(Keys.A))
            {
                Heading = CreatureHeading.Left;
                IsMoving = true;
                Entity.Transform.Position += new Vector2(-1, 0) * Time.Delta * 100.0f;
            }
            if (App.Instance.Input.IsKeyDown(Keys.D))
            {
                Heading = CreatureHeading.Right;
                IsMoving = true;
                Entity.Transform.Position += new Vector2(1, 0) * Time.Delta * 100.0f;
            }
            if (App.Instance.Input.IsKeyUp(Keys.W) && App.Instance.Input.IsKeyUp(Keys.S) && App.Instance.Input.IsKeyUp(Keys.A) && App.Instance.Input.IsKeyUp(Keys.D))
            {
                IsMoving = false;
            }

            string creature = "Human";

            string animationName;
            
            if (IsMoving)
            {
                animationName = "Walk";
            }
            else
            {
                animationName = "Idle";
            }

            string direction;

            if (Heading == CreatureHeading.Left && Facing == CreatureFacing.Up)
            {
                direction = "UpLeft";
            }
            else if (Heading == CreatureHeading.Left && Facing == CreatureFacing.Down)
            {
                direction = "DownLeft";
            }
            else if (Heading == CreatureHeading.Right && Facing == CreatureFacing.Up)
            {
                direction = "UpRight";
            }
            else
            {
                direction = "DownRight";
            }

            string animationKey = $"{creature}{animationName}_{direction}";

            Entity.GetComponent<SpriteRenderer>().Sprite = animationKey;
        }
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
