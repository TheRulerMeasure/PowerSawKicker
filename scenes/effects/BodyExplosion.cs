using Godot;
using System;

public class BodyExplosion : Node2D
{
    private const float EXPAND_RATE = 25;
    private const int MAX_CIRCLES = 4;

    private readonly Vector2[] _circlePositions =
    {
        new Vector2(-12, 0),
        new Vector2(-3, -5),
        new Vector2(11, 10),
        new Vector2(3, -15),
    };

    private float[] _radii = { 0.1f, 0.1f, 0.1f, 0.1f };

    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    private bool[] _indexes = { false, false, false, false };

    private Timer _timer;

    public override void _Ready()
    {
        SetProcess(false);
        _rng.Randomize();
        int strikesCount = 0;
        for (int i = 0; i < MAX_CIRCLES; i++)
        {
            bool isStrike = _rng.Randf() < 0.5f;
            strikesCount += isStrike ? 1 : 0;
            if (strikesCount >= 2)
            {
                _indexes[i] = true;
            }
            else
            {
                _indexes[i] = !isStrike;
            }
        }
        _timer = GetNode<Timer>("Timer");
        _timer.Connect("timeout", this, nameof(OnTimerTimeout), null, (uint) ConnectFlags.Oneshot);
        _timer.Start(2);
        CPUParticles2D particles2D = GetNode<CPUParticles2D>("CPUParticles2D");
        particles2D.Emitting = true;
    }

    public override void _Process(float delta)
    {
        ExpandCircle(delta);
    }

    public override void _Draw()
    {
        for (int i = 0; i < MAX_CIRCLES; i++)
        {
            DrawCircle(_circlePositions[i], _radii[i], Colors.Black);
        }
    }

    private void ExpandCircle(float delta)
    {
        for (int i = 0, n = MAX_CIRCLES; n > i; i++)
        {
            if (_indexes[i])
            {
                _radii[i] += EXPAND_RATE * delta;
            }
        }
        Update();
    }

    private void FadeAndFree()
    {
        var tween = CreateTween();
        tween.TweenProperty(this, "modulate", new Color(1, 1, 1, 0), 1);
        tween.TweenCallback(this, "queue_free");
    }

    private void OnTimerTimeout()
    {
        SetProcess(true);
        _timer.Connect("timeout", this, nameof(OnTimerTimeout2), null, (uint) ConnectFlags.Oneshot);
        _timer.Start(3);
    }

    private void OnTimerTimeout2()
    {
        FadeAndFree();
    }
}
