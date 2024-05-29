using Godot;

public partial class Player : CharacterBody2D
{
    public const float Speed = 300.0f;

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;

        // Get the input direction and handle the movement/deceleration.
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        if (direction != Vector2.Zero)
        {
            direction = GetDirection();
            velocity.X = direction.X * Speed;
            velocity.Y = direction.Y * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private Vector2 GetDirection()
    {
        Vector2 mousePosition = GetGlobalMousePosition();
        Vector2 playerPosition = Position;
        Vector2 direction = default;
        Vector2 toMouse = (mousePosition - playerPosition).Normalized();
        if (Input.IsActionPressed("up"))
        {
            direction = toMouse;
        }
        else if (Input.IsActionPressed("down"))
        {
            direction = -toMouse;
        }
        else if (Input.IsActionPressed("left"))
        {
            direction = toMouse.Rotated(-Mathf.Pi / 2);
        }
        else if (Input.IsActionPressed("right"))
        {
            direction = toMouse.Rotated(Mathf.Pi / 2);
        }
        return direction;
    }
}
