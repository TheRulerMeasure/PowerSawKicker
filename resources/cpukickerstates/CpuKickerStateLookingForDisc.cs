using Godot;
using System;

public class CpuKickerStateLookingForDisc : CpuKickerState
{
    public override CpuKickerStates Tick(float delta)
    {
        if (cpuKickerPck.DetectedDiscPos.x >= 0)
        {
            EmitSignal(nameof(MotionAxisChanged), kicker.GlobalPosition.DirectionTo(cpuKickerPck.DetectedDiscPos));
            return CpuKickerStates.GoingToDisc;
        }
        EmitSignal(nameof(MotionAxisChanged), Vector2.Zero);
        return CpuKickerStates.None;
    }
}
