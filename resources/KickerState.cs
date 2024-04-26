using Godot;
using System;

public class KickerState : Resource
{
    protected Kicker kicker;
    protected KickerPackage kickerPck;

    public void InitState(Kicker kicker, KickerPackage pck)
    {
        this.kicker = kicker;
        this.kickerPck = pck;
    }

    public virtual void Enter()
    {

    }

    public virtual KickerStates TickPhysics(float delta)
    {
        return KickerStates.None;
    }

    public virtual void Exit()
    {

    }

    protected bool HasMotion()
    {
        return !Mathf.IsZeroApprox(kicker.MotionAxis.Length());
    }

    protected void StillMotioning(float delta, float friction = -1)
    {
        kicker.Velocity = kicker.ResGndMover.ApplyFriction(kicker.Velocity, delta, friction);
        kicker.MoveAndSlide(kicker.Velocity);
    }

    protected void Motioning(Vector2 motionAxis, float delta, float maxSpeed = -1)
    {
        kicker.Velocity = kicker.ResGndMover.ApplyMotion(kicker.Velocity, motionAxis, delta, maxSpeed);
        kicker.MoveAndSlide(kicker.Velocity);
    }
}
