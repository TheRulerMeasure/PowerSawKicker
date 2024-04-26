using Godot;
using System;

public class KickerStateDying : KickerState
{
    private const float MAX_DYING_TIME = 1.5f;

    private float _dyingTime = 0;

    public override KickerStates TickPhysics(float delta)
    {
        _dyingTime += delta;
        if (_dyingTime > MAX_DYING_TIME)
        {
            if (!IsQueuedForDeletion())
            {
                BlowUp();
                kicker.QueueFree();
            }
        }
        return KickerStates.None;
    }

    private void BlowUp()
    {
        var blow = kickerPck.PackedBodyExplosion.Instance<Node2D>();
        blow.Position = kicker.GlobalPosition;
        kicker.GetParent().AddChild(blow);
    }
}
