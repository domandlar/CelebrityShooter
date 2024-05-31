using Godot;

public partial class Player : CharacterBody2D
{
	public const float Speed = 100.0f;

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	private AnimatedSprite2D _animatedSprite2D;

	private Orientation _orientation;

	public override void _Ready()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		base._Ready();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			direction = GetDirection();
			_orientation = GetOrientation(direction);
			PlayAnimation(_orientation, true);
			velocity.X = direction.X * Speed;
			velocity.Y = direction.Y * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
			PlayAnimation(_orientation, false);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void PlayAnimation(Orientation orientation, bool isMoving)
	{
		switch (orientation)
		{
			case Orientation.Up:
				var backAnimation = isMoving ? CharacterAnimation.BackWalk : CharacterAnimation.BackIdle;
				_animatedSprite2D.Play(backAnimation.ToString());
				break;
			case Orientation.Down:
				var frontAnimation = isMoving ? CharacterAnimation.FrontWalk : CharacterAnimation.FrontIdle;
				_animatedSprite2D.Play(frontAnimation.ToString());
				break;
			case Orientation.UpLeft:
			case Orientation.DownLeft:
			case Orientation.Left:
				var leftAnimation = isMoving ? CharacterAnimation.LeftWalk : CharacterAnimation.LeftIdle;
				_animatedSprite2D.Play(leftAnimation.ToString());
				break;
			case Orientation.UpRight:
			case Orientation.DownRight:
			case Orientation.Right:
				var rightAnimation = isMoving ? CharacterAnimation.RightWalk : CharacterAnimation.RightIdle;
				_animatedSprite2D.Play(rightAnimation.ToString());
				break;
		}
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

	public Orientation GetOrientation(Vector2 direction)
	{
		var isXZero = IsZeroWithTolerance(direction.X);
		var isYZero = IsZeroWithTolerance(direction.Y);

		if (isXZero)
		{
			return direction.Y < 0 ? Orientation.Up : Orientation.Down;
		}

		if (direction.X > 0)
		{
			if (isYZero)
			{
				return Orientation.Right;
			}
			if (direction.Y > 0)
			{
				return Orientation.DownRight;
			}
			return Orientation.UpRight;
		}

		// if x < 0
		if (isYZero)
		{
			return Orientation.Left;
		}
		if (direction.Y > 0)
		{
			return Orientation.DownLeft;
		}
		return Orientation.UpLeft;
	}

	private static bool IsZeroWithTolerance(float value)
	{
		var tolerance = 0.5;
		return value >= -tolerance && value <= tolerance;
	}
}

public enum CharacterAnimation
{
	BackIdle,
	FrontIdle,
	LeftIdle,
	RightIdle,
	BackWalk,
	FrontWalk,
	LeftWalk,
	RightWalk
}

public enum Orientation
{
	Up,
	Down,
	Left,
	Right,
	UpLeft,
	UpRight,
	DownLeft,
	DownRight
}
