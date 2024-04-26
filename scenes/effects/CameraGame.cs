using Godot;
using System;

public class CameraGame : Camera2D
{
    private float _orbitSpeed = 25;
    private float _offsetRadius = 16;

    [Export(PropertyHint.Range, "-1000,1000,0.2")]
    public float OrbitSpeed
    {
        get { return _orbitSpeed; }
        set { _orbitSpeed = value; }
    }
    [Export]
    public float OffsetRadius
    {
        get { return _offsetRadius; }
        set { _offsetRadius = value; }
    }

    private float _ang = 0;
    private Vector2 _orbitOffset;

    public override void _Ready()
    {
        _orbitOffset = new Vector2(Mathf.Cos(_ang), Mathf.Sin(_ang)) * OffsetRadius;
    }

    public override void _PhysicsProcess(float delta)
    {
        _ang += Mathf.Deg2Rad(OrbitSpeed) * delta;
        if (_ang >= Mathf.Tau)
        {
            _ang -= Mathf.Tau;
        }
        _orbitOffset = new Vector2(Mathf.Cos(_ang), Mathf.Sin(_ang)) * OffsetRadius;
        Offset = _orbitOffset;
    }
}
