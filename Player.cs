using System;
using Godot;

namespace DodgeTheCreeps;

public partial class Player : Area2D
{
    [Signal]
    public delegate void HitEventHandler();
    
    [Export]
    public int Speed { get; set; } = 400;

    public Vector2 ScreenSize;

    private AnimatedSprite2D _animatedSprite2D;
    private CollisionShape2D _collisionShape2D;

    public override void _Ready()
    {
        base._Ready();
        
        ScreenSize = GetViewportRect().Size;
        
        _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        
        Hide();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        var velocity = Vector2.Zero;

        if (Input.IsActionPressed("move_up"))
        {
            velocity.Y -= 1.0;
        }

        if (Input.IsActionPressed("move_down"))
        {
            velocity.Y += 1.0;
        }

        if (Input.IsActionPressed("move_left"))
        {
            velocity.X -= 1.0;
        }
        
        if (Input.IsActionPressed("move_right"))
        {
            velocity.X += 1.0;
        }
        
        if (velocity.Length() > 0)
        {
            _animatedSprite2D.Play();
            
            velocity = velocity.Normalized() * Speed;
            Position += velocity * (float)delta;
            Position = new Vector2(
                x: Math.Clamp(Position.X, 0, ScreenSize.X),
                y: Math.Clamp(Position.Y, 0, ScreenSize.Y)
            );

            if (velocity.X != 0)
            {
                _animatedSprite2D.Animation = "walk";
                _animatedSprite2D.FlipV = false;
                _animatedSprite2D.FlipH = velocity.X < 0;
            }
            else if (velocity.Y != 0)
            {
                _animatedSprite2D.Animation = "up";
                _animatedSprite2D.FlipV = velocity.Y > 0;
                _animatedSprite2D.FlipH = false;
            }
        }
        else
        {
            _animatedSprite2D.Stop();
        }
    }
    
    public void Start(Vector2 position)
    {
        Position = position;
        Show();
        _collisionShape2D.Disabled = false;
    }
    
    private void OnBodyEntered(Node2D body)
    {
        Hide();
        EmitSignal(SignalName.Hit);
        _collisionShape2D.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }
}