using Godot;
using GDCollections = Godot.Collections;
using System;

public class AreaDiscDetector : Area2D
{
    [Signal] public delegate void DiscPosChanged(Vector2 pos, Vector2 vel);

    private float _maxDelayDetect = 0.25f;

    [Export]
    public float MaxDelayDetect
    {
        get { return _maxDelayDetect; }
        set { _maxDelayDetect = value; }
    }

    private float _delayDetect = 0;

    public override void _Ready()
    {
        
    }

    public override void _Process(float delta)
    {
        _delayDetect += delta;
        if (_delayDetect >= MaxDelayDetect)
        {
            _delayDetect = 0;
            Disc disc = GetClosestDisc(GetOverlappingBodies());
            if (disc != null)
            {
                EmitSignal(nameof(DiscPosChanged), disc.GlobalPosition, disc.Velocity);
            }
            else
            {
                EmitSignal(nameof(DiscPosChanged), Vector2.One * -1, Vector2.Zero);
            }
        }
    }

    private Disc GetClosestDisc(GDCollections.Array bodies)
    {
        Disc disc = null;
        float dist = Mathf.Inf;
        for (int i = 0; i < bodies.Count; i++)
        {
            Disc b = bodies[i] as Disc;
            float newDist = GlobalPosition.DistanceTo(b.GlobalPosition);
            if (newDist < dist)
            {
                dist = newDist;
                disc = b;
            }
        }
        return disc;
    }
}
