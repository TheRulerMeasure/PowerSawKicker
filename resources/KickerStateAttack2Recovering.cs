using Godot;
using System;

public class KickerStateAttack2Recovering : KickerState
{
    private const float MAX_RECOVER_TIME = 0.15f;

    private float recoverTime = 0;

    public override void Enter()
    {
        kicker.AnimPlayer.Play("idle");
        float shootForce = 15;
        kickerPck.ShootBulletA(kicker.GlobalPosition, kicker.FacingDirection * shootForce);
    }

    public override KickerStates TickPhysics(float delta)
    {
        if (kicker.Health <= 0)
        {
            return KickerStates.Dying;
        }
        StillMotioning(delta);
        recoverTime += delta;
        if (recoverTime > MAX_RECOVER_TIME)
        {
            return KickerStates.Idling;
        }
        return KickerStates.None;
    }

    public override void Exit()
    {
        recoverTime = 0;
    }
}
