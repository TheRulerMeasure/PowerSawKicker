using Godot;
using System;

public class CpuKickerStateDodging : CpuKickerState
{
    private const float MAX_DODGING_TIME = 0.6f;

    private float _dodgingTime = 0;
    private Vector2 _axis = Vector2.Right;

    public override void Enter()
    {
        var vel = cpuKickerPck.DetectedDiscVel;
        if (Mathf.IsZeroApprox(vel.Length()))
        {
            _axis = Vector2.Right;
            return;
        }
        _axis = cpuKickerPck.DetectedDiscVel.Normalized().Rotated(Mathf.Pi * 0.5f);
        var dist1 = cpuKickerPck.DetectedDiscPos.DistanceTo(kicker.GlobalPosition + _axis);
        var dist2 = cpuKickerPck.DetectedDiscPos.DistanceTo(kicker.GlobalPosition + _axis.Rotated(Mathf.Pi));
        var dodgeLeft = dist1 < dist2;
        if (dodgeLeft)
        {
            _axis = _axis.Rotated(Mathf.Pi);
        }
    }

    public override CpuKickerStates Tick(float delta)
    {
        EmitSignal(nameof(MotionAxisChanged), _axis);
        _dodgingTime += delta;
        if (_dodgingTime >= MAX_DODGING_TIME)
        {
            EmitSignal(nameof(MotionAxisChanged), Vector2.Zero);
            return CpuKickerStates.LookingForDisc;
        }
        return CpuKickerStates.None;
    }

    public override void Exit()
    {
        _dodgingTime = 0;
    }
}
