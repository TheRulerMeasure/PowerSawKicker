using Godot;
using System;

public class ResGroundMover : Resource
{
    private float _acceleration = 1250;
    private float _maxSpeed = 145;
    private float _friction = 880;

    [Export(PropertyHint.Range, "0.2,6000.0,0.2")] public float Acceleration
    {
        get => _acceleration;
        set => _acceleration = value;
    }
    [Export(PropertyHint.Range, "0.2,6000.0,0.2")] public float MaxSpeed
    {
        get => _maxSpeed;
        set => _maxSpeed = value;
    }
    [Export(PropertyHint.Range, "0.2,6000.0,0.2")] public float Friction
    {
        get => _friction;
        set => _friction = value;
    }

    public Vector2 ApplyMotion(Vector2 curVel, Vector2 motionAxis, float delta, float maxSpeed = -1)
    {
        float actualMaxSpeed = maxSpeed >= 0 ? maxSpeed : MaxSpeed;
        Vector2 vel = curVel + motionAxis * Acceleration * delta;
        return vel.LimitLength(actualMaxSpeed);
    }

    public Vector2 ApplyFriction(Vector2 curVel, float delta, float fricton = -1)
    {
        float actualFriction = fricton >= 0 ? fricton : Friction;
        float amount = actualFriction * delta;
        if (curVel.Length() < amount)
        {
            return Vector2.Zero;
        }
        return curVel - curVel.Normalized() * amount;
    }
}
