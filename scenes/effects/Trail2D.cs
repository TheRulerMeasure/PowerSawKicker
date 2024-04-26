using Godot;
using System;

public class Trail2D : Line2D
{
    private int _maxPoints = 10;

    [Export]
    public int MaxPoints
    {
        get { return _maxPoints; }
        set { _maxPoints = value; }
    }

    private Node2D parent;

    public override void _Ready()
    {
        SetAsToplevel(true);
        parent = GetParent<Node2D>();
    }

    public override void _PhysicsProcess(float delta)
    {
        AddPoint(parent.GlobalPosition);
        if (GetPointCount() > MaxPoints)
        {
            RemovePoint(0);
        }
    }
}
