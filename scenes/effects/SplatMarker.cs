using Godot;
using System;

public class SplatMarker : Node2D
{
    private const int CHILD_COUNT = 3;

    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        _rng.Randomize();
        Position += Vector2.Up * 152;
        var rot = Rotation;
        Rotation = 0;
        var index = _rng.RandiRange(0, CHILD_COUNT - 1);
        var sp = GetChild<Sprite>(index);
        sp.Rotation = rot;
        sp.Show();
        var timer = GetTree().CreateTimer(1.5f);
        timer.Connect("timeout", this, nameof(FadeAndKill));
    }

    private void FadeAndKill()
    {
        var tween = CreateTween();
        tween.TweenProperty(this, "modulate", new Color(1, 1, 1, 0), 1);
        tween.TweenCallback(this, "queue_free");
    }
}
