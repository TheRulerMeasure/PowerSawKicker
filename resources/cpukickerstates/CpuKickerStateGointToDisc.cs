using Godot;
using System;

public class CpuKickerStateGointToDisc : CpuKickerState
{
    public override CpuKickerStates Tick(float delta)
    {
        if (cpuKickerPck.DetectedDiscPos.x < 0)
        {
            EmitSignal(nameof(MotionAxisChanged), Vector2.Zero);
            return CpuKickerStates.LookingForDisc;
        }
        if (IsTooCloseAndApproaching())
        {
            if (!cpuKickerPck.CanShoot())
            {
                EmitSignal(nameof(MotionAxisChanged), Vector2.Zero);
                return CpuKickerStates.Dodging;
            }
            else
            {
                EmitSignal(nameof(MotionAxisChanged), Vector2.Zero);
                return CpuKickerStates.QuickShooting;
            }
        }
        if (IsTooClose())
        {
            if (cpuKickerPck.CanShoot())
            {
                EmitSignal(nameof(MotionAxisChanged), Vector2.Zero);
                return CpuKickerStates.AimingAtDisc;
            }
            else
            {
                var dir = kicker.GlobalPosition.DirectionTo(cpuKickerPck.DetectedDiscPos);
                dir *= -1;
                EmitSignal(nameof(MotionAxisChanged), dir);
                return CpuKickerStates.None;
            }
        }
        EmitSignal(nameof(MotionAxisChanged), kicker.GlobalPosition.DirectionTo(cpuKickerPck.DetectedDiscPos));
        return CpuKickerStates.None;
    }

    private bool IsTooCloseAndApproaching()
    {
        Vector2 vel = cpuKickerPck.DetectedDiscVel;
        if (Mathf.IsZeroApprox(vel.Length()))
        {
            return false;
        }
        float dist = cpuKickerPck.DetectedDiscPos.DistanceTo(kicker.GlobalPosition);
        if (dist < 199f)
        {
            var dotted = vel.Normalized().Dot(kicker.MotionAxis * -1);
            if (dotted > 0)
            {
                // GD.Print(dotted);
                if (vel.Length() > 5)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    private bool IsTooClose()
    {
        return cpuKickerPck.DetectedDiscPos.DistanceTo(kicker.GlobalPosition) < 166f;
    }
}
