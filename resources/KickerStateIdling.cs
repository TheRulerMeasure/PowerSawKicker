using Godot;
using System;

public class KickerStateIdling : KickerState
{
    public override void Enter()
    {
        kicker.AnimPlayer.Play("idle");
    }

    public override KickerStates TickPhysics(float delta)
    {
        if (kicker.Health <= 0)
        {
            return KickerStates.Dying;
        }
        if (HasMotion())
        {
            Vector2 motionAxis = kicker.MotionAxis;
            FlippingSprite(motionAxis.x);
            Motioning(motionAxis, delta);
            return KickerStates.Moving;
        }
        StillMotioning(delta);
        if (kicker.Attacking2 && kickerPck.CanShootBulletA())
        {
            return KickerStates.Aiming;
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
