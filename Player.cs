using Godot;

public partial class Player : Area2D
{
  //player will emit this signal when it collides with an enemy
  [Signal]
  public delegate void HitEventHandler();
  private void OnBodyEntered(Node2D body)
  {
	//player disappears and emits signal to show that it has been hit
	Hide();
	EmitSignal(SignalName.Hit);
	GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
  }

  [Export]
  //how fast the player moves (pixels/sec)
  public int Speed { get; set; } = 400;
  //size of game window
  public Vector2 ScreenSize;

  //called when node enters the scren tree, use to find size of game window
  public override void _Ready()
  {
	ScreenSize = GetViewportRect().Size;
	Hide();
  }

  //called every frame, use to update elements that change often (check for input, move in direction, play animation)
  public override void _Process(double delta)
  {
	//player's movement vector, set to zero at the start of each frame
	var velocity = Vector2.Zero;

	//check each input and add/subtract from velocity to obtain a total direction
	if (Input.IsActionPressed("move_right"))
	{
	  velocity.X += 1;
	}
	if (Input.IsActionPressed("move_left"))
	{
	  velocity.X -= 1;
	}
	if (Input.IsActionPressed("move_up"))
	{
	  velocity.Y -= 1;
	}
	if (Input.IsActionPressed("move_down"))
	{
	  velocity.Y += 1;
	}

	var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

	//normalize velocity so diagonal movement isn't faster, then multiply by speed to get pixels/sec
	if (velocity.Length() > 0)
	{
	  velocity = velocity.Normalized() * Speed;
	  animatedSprite2D.Play();
	}
	else
	{
	  animatedSprite2D.Stop();
	}

	//update player position, making sure player can't move off screen
	Position += velocity * (float)delta;
	Position = new Vector2(
	  x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
	  y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
	);

	//flip sprite horizontally if moving left
	if( velocity.X != 0)
	{
	  animatedSprite2D.Animation = "walk";
	  animatedSprite2D.FlipV = false;
	  animatedSprite2D.FlipH = velocity.X < 0;
	}
	//flip sprite vertically if moving down
	else if(velocity.Y != 0)
	{
	  animatedSprite2D.Animation = "up";
	  animatedSprite2D.FlipV = velocity.Y > 0;
	}
  }

  public void Start(Vector2 position)
  {
	Position = position;
	Show();
	GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
  }
}
