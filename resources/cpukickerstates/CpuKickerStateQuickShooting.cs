using Godot;
using System;

public class CpuKickerStateQuickShooting : CpuKickerState
{
    private const float MAX_AIMING_TIME = 0.1f;

    private float _aimingTime = 0;

    public override void Enter()
    {
        EmitSignal(nameof(Attack2High));
    }

    public override CpuKickerStates Tick(float delta)
    {
        EmitSignal(nameof(MotionAxisChanged), Vector2.Zero);
        Aiming();
        _aimingTime += delta;
        if (_aimingTime >= MAX_AIMING_TIME)
        {
            EmitSignal(nameof(Attack2Low));
            return CpuKickerStates.Dodging;
        }
        return CpuKickerStates.None;
    }

    public override void Exit()
    {
        _aimingTime = 0;
    }

    private void Aiming()
    {
        Vector2 targetPos = cpuKickerPck.DetectedDiscPos;
        Vector2 faceDir = kicker.GlobalPosition.DirectionTo(targetPos);
        EmitSignal(nameof(FacingDirectionChanged), faceDir);
    }
}
