using Godot;
using System;

public class KickerStateAiming : KickerState
{
    private const float NEW_MAX_SPEED = 96;

    public override void Enter()
    {
        MoveArrow(kicker.GlobalPosition, kicker.FacingDirection);
        kickerPck.ArrowA.Show();
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
            Motioning(kicker.MotionAxis, delta, NEW_MAX_SPEED);
            return KickerStates.AimMoving;
        }
        StillMotioning(delta);
        if (!kicker.Attacking2)
        {
            return KickerStates.Attack2Recovering;
        }
        FlippingSprite(kicker.FacingDirection.x);
        MoveArrow(kicker.GlobalPosition, kicker.FacingDirection);
        return KickerStates.None;
    }

    public override void Exit()
    {
        kickerPck.ArrowA.Hide();
    }

    private void FlippingSprite(float x)
    {
        kicker.Sprite.FlipH = x < 0;
    }

    private void MoveArrow(Vector2 pos, Vector2 dir)
    {
        kickerPck.ArrowA.GlobalPosition = pos;
        kickerPck.ArrowA.Rotation = dir.Angle();
    }
}
