using Godot;
using GDCollections = Godot.Collections;
using System;

public class BulletA : Node2D
{
    private uint _collisionLayer = 10;
    private float _circleRadius = 16;
    private Vector2 _velocity = new Vector2(5.5f, 0);
    private float _kickForce = 3.6f;

    [Export(PropertyHint.Layers2dPhysics)]
    public uint CollisionLayer
    {
        get => _collisionLayer;
        set => _collisionLayer = value;
    }
    [Export(PropertyHint.Range, "0.2,30.0,0.2")]
    public float CircleRadius
    {
        get => _circleRadius;
        set => _circleRadius = value;
    }
    [Export]
    public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    [Export]
    public float KickForce
    {
        get { return _kickForce; }
        set { _kickForce = value; }
    }

    public override void _Ready()
    {

    }

    public override void _PhysicsProcess(float delta)
    {
        RID circleRID = Physics2DServer.CircleShapeCreate();
        Physics2DServer.ShapeSetData(circleRID, CircleRadius);
        GDCollections.Array intersectResults = GetQueryParam(circleRID);
        Physics2DServer.FreeRid(circleRID);
        Position += Velocity;
        if (intersectResults.Count <= 0)
        {
            return;
        }
        CheckResults(intersectResults);
        QueueFree();
    }

    private GDCollections.Array GetQueryParam(RID shapeRID)
    {
        var directSpace = GetWorld2d().DirectSpaceState;
        var param = new Physics2DShapeQueryParameters();
        param.Transform = GetParamTransform();
        param.ShapeRid = shapeRID;
        param.Margin = 0.08f;
        param.Motion = Velocity;
        param.CollideWithAreas = false;
        param.CollideWithBodies = true;
        param.CollisionLayer = CollisionLayer;
        return directSpace.IntersectShape(param, 6);
    }

    private Transform2D GetParamTransform()
    {
        return Transform2D.Identity.Translated(Position);
    }

    private void CheckResults(GDCollections.Array results)
    {
        Node2D closestDisc = null;
        float dist = Mathf.Inf;
        foreach (GDCollections.Dictionary result in results)
        {
            if (!(result["collider"] as Node).IsInGroup("disc"))
            {
                continue;
            }
            Node2D disc = result["collider"] as Node2D;
            float newDist = Position.DistanceTo(disc.Position);
            if (newDist < dist)
            {
                dist = newDist;
                closestDisc = disc;
            }
        }
        closestDisc?.Call("ApplyForce", Velocity.Normalized() * KickForce);
    }
}
