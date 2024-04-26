using Godot;
using System;

public class CpuKickerState : Resource
{
    [Signal] public delegate void MotionAxisChanged(Vector2 axis);
    [Signal] public delegate void FacingDirectionChanged(Vector2 dir);
    [Signal] public delegate void Attack2High();
    [Signal] public delegate void Attack2Low();

    protected CpuKickerPackage cpuKickerPck;
    protected Kicker kicker;

    public void InitState(Kicker kicker, CpuKickerPackage pck)
    {
        this.kicker = kicker;
        Connect(nameof(MotionAxisChanged), kicker, "OnMotionAxisChanged");
        Connect(nameof(FacingDirectionChanged), kicker, "OnFacingDirectionChanged");
        Connect(nameof(Attack2High), kicker, "OnAttack2Highed");
        Connect(nameof(Attack2Low), kicker, "OnAttack2Lowed");
        cpuKickerPck = pck;
    }

    public virtual void Enter()
    {

    }

    public virtual CpuKickerStates Tick(float delta)
    {
        return CpuKickerStates.None;
    }

    public virtual void Exit()
    {

    }
}
