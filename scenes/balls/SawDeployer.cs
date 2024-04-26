using Godot;
using System;

public class SawDeployer : Node2D
{
    private Vector2 _startVelocity = new Vector2(3.7f, 0);

    [Export]
    public PackedScene PackedDisc { get; set; }

    [Export]
    public Vector2 StartVelocity
    {
        get { return _startVelocity; }
        set { _startVelocity = value; }
    }

    private Timer _timer2;

    public override void _Ready()
    {
        var arrow = GetNode<Node2D>("ArrowContainer");
        arrow.Rotation = StartVelocity.Normalized().Angle() + Mathf.Pi * 3;
        var tween = CreateTween();
        tween.SetEase(Tween.EaseType.Out);
        tween.SetTrans(Tween.TransitionType.Sine);
        tween.TweenProperty(arrow, "rotation", StartVelocity.Normalized().Angle(), 0.9f);
        var timer = GetNode<Timer>("Timer");
        timer.Connect("timeout", this, nameof(OnTimerTimeout));
        timer.Start();
        _timer2 = GetNode<Timer>("Timer2");
        _timer2.Connect("timeout", this, nameof(OnTimer2Timeout));
    }

    private void OnTimerTimeout()
    {
        var disc = PackedDisc.Instance<Disc>();
        disc.Position = GlobalPosition;
        disc.Velocity = StartVelocity;
        GetParent().AddChild(disc);
        _timer2.Start();
    }

    private void OnTimer2Timeout()
    {
        QueueFree();
    }
}
