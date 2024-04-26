using Godot;
using System;

public class CpuKickerStateAimingAtDisc : CpuKickerState
{
    private const float MAX_AIMING_TIME = 0.75f;
    private const float MIN_AIMING_TIME = 0.5f;

    private RandomNumberGenerator _rng = new RandomNumberGenerator();
    private float _aimingTime = 0f;
    private float _maxAimingTime = 0f;
    private float _leadAmount = 0f;

    public override void Enter()
    {
        _rng.Randomize();
        _rng.Seed += 15;
        _leadAmount = _rng.Randf() * 40;
        _maxAimingTime = _rng.RandfRange(MIN_AIMING_TIME, MAX_AIMING_TIME);
        EmitSignal(nameof(Attack2High));
    }

    public override CpuKickerStates Tick(float delta)
    {
        MovingAwayFromDisc();
        Aiming();
        _aimingTime += delta;
        if (_aimingTime >= _maxAimingTime)
        {
            EmitSignal(nameof(Attack2Low));
            EmitSignal(nameof(MotionAxisChanged), Vector2.Zero);
            return CpuKickerStates.Dodging;
        }
        return CpuKickerStates.None;
    }

    public override void Exit()
    {
        _aimingTime = 0;
    }

    private void MovingAwayFromDisc()
    {
        Vector2 dir = kicker.GlobalPosition.DirectionTo(cpuKickerPck.DetectedDiscPos);
        dir *= -1;
        float ang = _rng.Randf() * Mathf.Pi * 0.5f;
        EmitSignal(nameof(MotionAxisChanged), dir.Rotated(ang));
    }

    private void Aiming()
    {
        Vector2 targetPos = cpuKickerPck.DetectedDiscPos + (cpuKickerPck.DetectedDiscVel * _leadAmount);
        Vector2 faceDir = kicker.GlobalPosition.DirectionTo(targetPos);
        EmitSignal(nameof(FacingDirectionChanged), faceDir);
    }
}
