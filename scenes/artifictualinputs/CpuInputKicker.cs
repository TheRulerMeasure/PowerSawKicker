using Godot;
using GDCollections = Godot.Collections;
using System;

public class CpuInputKicker : Node
{
    [Signal] public delegate void MotionAxisChanged(Vector2 axis);
    [Signal] public delegate void DiscPosChanged(Vector2 pos);

    private NodePath _kickerNp = "..";

    [Export]
    public NodePath KickerNp
    {
        get { return _kickerNp; }
        set { _kickerNp = value; }
    }
    [Export]
    public NodePath AreaDiscDetectNp { get; set; }

    private Node2D _enemy;
    private Node2D _node;
    private Area2D _areaDiscDetector;

    public override void _Ready()
    {
        _areaDiscDetector = GetNode<Area2D>(AreaDiscDetectNp);
        _node = GetNode<Node2D>(KickerNp);
        Connect(nameof(MotionAxisChanged), _node, "OnMotionAxisChanged");
        Timer timerDiscDetect = GetNode<Timer>("TimerDiscDetect");
        timerDiscDetect.Connect("timeout", this, nameof(OnTimerDiscDetectTimeout));
    }

    public void InitEnemy(NodePath np)
    {
        _enemy = GetNode<Node2D>(np);
    }

    private Vector2 GetClosestDisc(GDCollections.Array bodies)
    {
        Vector2 pos = Vector2.One;
        float dist = Mathf.Inf;
        for (int i = 0; i < bodies.Count; i++)
        {
            Node2D b = bodies[i] as Node2D;
            float newDist = _node.GlobalPosition.DistanceTo(b.GlobalPosition);
            if (newDist < dist)
            {
                dist = newDist;
                pos = b.GlobalPosition;
            }
        }
        return pos;
    }

    private void OnTimerDiscDetectTimeout()
    {
        var bodies = _areaDiscDetector.GetOverlappingBodies();
        if (bodies.Count <= 0)
        {
            EmitSignal(nameof(DiscPosChanged), Vector2.One * -1);
            return;
        }
        Vector2 pos = GetClosestDisc(bodies);
        EmitSignal(nameof(DiscPosChanged), pos);
    }
}
