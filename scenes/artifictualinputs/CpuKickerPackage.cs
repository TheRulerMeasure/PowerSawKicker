using Godot;
using System;

public class CpuKickerPackage : Node
{
    private PackedScene _packedAreaDiscDetector = ResourceLoader.Load<PackedScene>("res://scenes/artifictualinputs/AreaDiscDetector.tscn");
    private Vector2 _detectedDiscPos = Vector2.One * -1;
    private Vector2 _detectedDiscVel = Vector2.Zero;

    [Export]
    public PackedScene PackedAreaDiscDetector
    {
        get { return _packedAreaDiscDetector; }
        set { _packedAreaDiscDetector = value; }
    }
    public Vector2 DetectedDiscPos
    {
        get { return _detectedDiscPos; }
        set { _detectedDiscPos = value; }
    }
    public Vector2 DetectedDiscVel
    {
        get { return _detectedDiscVel; }
        set { _detectedDiscVel = value; }
    }

    private KickerPackage _kickerPck;

    public override void _Ready()
    {
        Node kicker = GetNode("../..");
        Area2D area = PackedAreaDiscDetector.Instance<Area2D>();
        kicker.CallDeferred("add_child", area);
        area.Connect("DiscPosChanged", this, nameof(OnDiscPosChanged));
        _kickerPck = kicker.GetNode<KickerPackage>("KickerStateMachine/KickerPackage");
    }

    private void OnDiscPosChanged(Vector2 pos, Vector2 vel)
    {
        DetectedDiscPos = pos;
        DetectedDiscVel = vel;
    }

    public bool CanShoot()
    {
        return _kickerPck.CanShootBulletA();
    }
}
