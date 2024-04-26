using Godot;
using GDCollections = Godot.Collections;
using System;

public class Disc : KinematicBody2D
{
    private const float CIRCLE_RADIUS = 18;

    private Vector2 _velocity = Vector2.Zero;
    private float _friction = 3.2f;
    private uint _hurtboxCollisionLayer = 4;

    [Export]
    public Vector2 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    [Export(PropertyHint.Range, "0.0,100.0,0.2")]
    public float Friction
    {
        get => _friction;
        set => _friction = value;
    }

    [Export]
    public PackedScene PackedSpark { get; set; }
    [Export]
    public PackedScene PackedSplat1 { get; set; }

    [Export(PropertyHint.Layers2dPhysics)]
    public uint HurtBoxCollisionLayer
    {
        get { return _hurtboxCollisionLayer; }
        set {  _hurtboxCollisionLayer = value; }
    }

    private Node2D _overLayer;

    private RandomNumberGenerator _rng;

    private GDCollections.Array<AudioStreamPlayer2D> _soundSawHits = new GDCollections.Array<AudioStreamPlayer2D>();
    private AudioStreamPlayer2D _soundFleshHit;

    public override void _Ready()
    {
        _soundFleshHit = GetNode<AudioStreamPlayer2D>("SoundHitFlesh");
        foreach (AudioStreamPlayer2D aud in GetNode("Sounds").GetChildren())
        {
            _soundSawHits.Add(aud);
        }
        _rng = new RandomNumberGenerator();
        _rng.Randomize();
        _overLayer = GetTree().CurrentScene.GetNode<Node2D>("OverLayer");
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!Mathf.IsZeroApprox(Velocity.Length()))
        {
            DiscIntersection();
        }
        var coll = MoveAndCollide(Velocity);
        if (coll != null)
        {
            Velocity = Velocity.Bounce(coll.Normal);
            CreateSpark(coll.Position, coll.Normal);
            _soundSawHits[_rng.RandiRange(0, _soundSawHits.Count - 1)].Play();
        }
    }

    public void ApplyForce(Vector2 vel)
    {
        CreateSpark(GlobalPosition + vel.Normalized() * -18, vel.Normalized());
        if (Mathf.IsZeroApprox(Velocity.Length()))
        {
            Velocity = vel;
        }
        if (vel.Normalized().Dot(Velocity.Normalized()) > 0)
        {
            float force = vel.Length() + (Velocity.Length() * 0.7f);
            Velocity = vel.Normalized() * force;
        }
        else
        {
            Velocity = vel;
        }
        _soundSawHits[_rng.RandiRange(0, _soundSawHits.Count - 1)].Play();
    }

    private void CreateSpark(Vector2 pos, Vector2 normal)
    {
        Node2D spark = PackedSpark.Instance<Node2D>();
        spark.Position = pos;
        float rot = normal.Angle();
        rot -= Mathf.Pi * 0.5f;
        spark.Rotation = rot + _rng.Randf() * 0.67f;
        _overLayer.AddChild(spark);
    }

    private void DiscIntersection()
    {
        var shapeRID = Physics2DServer.CircleShapeCreate();
        Physics2DServer.ShapeSetData(shapeRID, CIRCLE_RADIUS);
        var results = GetIntersectShapeResults(shapeRID);
        Physics2DServer.FreeRid(shapeRID);
        foreach (GDCollections.Dictionary result in results)
        {
            Node collider = result["collider"] as Node;
            bool tookDamage = (bool) collider.Call("TakeDamage", 1);
            if (tookDamage)
            {
                PutSplat();
                _soundFleshHit.Play();
            }
        }
    }

    private GDCollections.Array GetIntersectShapeResults(RID shapeRID)
    {
        var space = GetWorld2d().DirectSpaceState;
        var param = new Physics2DShapeQueryParameters();
        param.ShapeRid = shapeRID;
        param.Margin = 0.08f;
        var tf = Transform2D.Identity.Translated(GlobalPosition);
        param.Transform = tf;
        param.Motion = Velocity;
        param.CollideWithAreas = false;
        param.CollideWithBodies = true;
        param.CollisionLayer = HurtBoxCollisionLayer;
        return space.IntersectShape(param, 6);
    }

    private void PutSplat()
    {
        Vector2 pos = GlobalPosition + Velocity.Normalized() * 20;
        var splat = PackedSplat1.Instance<Node2D>();
        splat.Position = pos;
        splat.Rotation = Velocity.Normalized().Angle();
        GetParent().AddChild(splat);
    }
}
