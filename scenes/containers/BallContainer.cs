using Godot;
using System;

public class BallContainer : Node2D
{
    private float _delayBeforeNextDeploy = 17;

    private int _sawsLeftToBeDeployed = 2;

    [Export]
    public PackedScene PackedDeployer { get; set; }

    [Export(PropertyHint.Range, "2,99,0.2")]
    public float DelayBeforeNextDeploy
    {
        get { return _delayBeforeNextDeploy; }
        set { _delayBeforeNextDeploy = value; }
    }
    [Export]
    public int SawsLeftToBeDeployed
    {
        get { return _sawsLeftToBeDeployed; }
        set { _sawsLeftToBeDeployed = value; }
    }

    [Export]
    public Vector2 DeployPos { get; set; }

    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        _rng.Randomize();
        var gg = GetNode<SceneTransistor>("/root/SceneTransistor");
        SawsLeftToBeDeployed = gg.SawsCount;
    }

    public void InitializeDeployer()
    {
        DeployAt(DeployPos, GetRandomVel());
        SawsLeftToBeDeployed--;
        if (SawsLeftToBeDeployed <= 0)
        {
            return;
        }
        GetTree().CreateTimer(DelayBeforeNextDeploy).Connect("timeout", this, nameof(DeployNext), null, (uint)ConnectFlags.Oneshot);
    }

    public void DeployAt(Vector2 pos, Vector2 vel)
    {
        var deployer = PackedDeployer.Instance<SawDeployer>();
        deployer.Position = pos;
        deployer.StartVelocity = vel;
        AddChild(deployer);
    }

    public void DeployNext()
    {
        DeployAt(DeployPos, GetRandomVel());
        SawsLeftToBeDeployed--;
        if (SawsLeftToBeDeployed <= 0)
        {
            return;
        }
        GetTree().CreateTimer(DelayBeforeNextDeploy).Connect("timeout", this, nameof(DeployNext), null, (uint)ConnectFlags.Oneshot);
    }

    private Vector2 GetRandomVel()
    {
        return Vector2.Right.Rotated(_rng.Randf() * Mathf.Tau) * 3.7f;
    }
}
