using Godot;
using System;

public class Kicker : KinematicBody2D
{
    [Signal] public delegate void Died();

    private Vector2 _velocity = Vector2.Zero;
    private Vector2 _motionAxis = Vector2.Zero;
    private Vector2 _facingDirection = Vector2.Right;

    private bool _attacking1 = false;
    private bool _attacking2 = false;
    private bool _invincible = false;

    private int _health = 1;

    [Export] public ResGroundMover ResGndMover { get; set; }

    [Export] public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }
    [Export] public Vector2 MotionAxis
    {
        get => _motionAxis;
        set => _motionAxis = value;
    }
    [Export] public Vector2 FacingDirection
    {
        get { return _facingDirection; }
        set { _facingDirection = value; }
    }
    [Export] public bool Attacking1
    {
        get { return _attacking1; }
        set { _attacking1 = value; }
    }
    [Export] public bool Attacking2
    {
        get { return _attacking2; }
        set { _attacking2 = value; }
    }
    [Export]
    public int Health
    {
        get { return _health; }
        set { _health = value; }
    }
    [Export]
    public bool Invincible
    {
        get { return _invincible; }
        set
        {
            _invincible = value;
            if (_invincible == false)
            {
                if (Sprite != null)
                {
                    Sprite.Modulate = new Color(Sprite.Modulate, 1);
                }
            }
        }
    }

    public Sprite Sprite { get; set; }

    public AnimationPlayer AnimPlayer { get; set; }

    public CollisionShape2D Coll { get; set; }

    private float _invincibleAlpha = 1;

    public override void _Ready()
    {
        Sprite = GetNode<Sprite>("Sprite");
        AnimPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        Coll = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public override void _Process(float delta)
    {
        if (!Invincible)
        {
            return;
        }
        _invincibleAlpha += 15f * delta;
        if (_invincibleAlpha >= Mathf.Pi)
        {
            _invincibleAlpha -= Mathf.Pi;
        }
        float alpha = Mathf.Sin(_invincibleAlpha);
        Sprite.Modulate = new Color(Sprite.Modulate, alpha);
    }

    public bool TakeDamage(int amount)
    {
        if (Invincible)
        {
            return false;
        }
        Health -= amount;
        if (Health <= 0)
        {
            EmitSignal(nameof(Died));
            Coll.SetDeferred("disabled", true);
        }
        return true;
    }

    private void OnMotionAxisChanged(Vector2 axis)
    {
        MotionAxis = axis;
    }

    private void OnFacingDirectionChanged(Vector2 dir)
    {
        FacingDirection = dir;
    }

    private void OnAttack1Lowed()
    {
        Attacking1 = false;
    }

    private void OnAttack1Highed()
    {
        Attacking1 = true;
    }

    private void OnAttack2Lowed()
    {
        Attacking2 = false;
    }

    private void OnAttack2Highed()
    {
        Attacking2 = true;
    }
}
