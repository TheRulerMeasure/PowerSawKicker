using Godot;
using System;

public class KickerStateMoving : KickerState
{
    public override void Enter()
    {
        kicker.AnimPlayer.Play("run");
    }

    public override KickerStates TickPhysics(float delta)
    {
        if (kicker.Health <= 0)
        {
            return KickerStates.Dying;
        }
        if (!HasMotion())
        {
            StillMotioning(delta);
            return KickerStates.Idling;
        }
        Vector2 motionAxis = kicker.MotionAxis;
        FlippingSprite(motionAxis.x);
        Motioning(motionAxis, delta);
        if (kicker.Attacking2 && kickerPck.CanShootBulletA())
        {
            return KickerStates.AimMoving;
        }
        return KickerStates.None;
    }

    private void FlippingSprite(float x)
    {
        if (x <= -0.02f)
        {
            kicker.Sprite.FlipH = true;
        }
        else if (x >= 0.02f)
        {
            kicker.Sprite.FlipH = false;
        }
    }
}
