using Godot;
using System;

public class KickerPackage : Node
{
    private int _maxShoes = 2;

    [Export]
    public PackedScene PackedArrow { get; set; }
    [Export]
    public PackedScene PackedBulletA { get; set; }
    [Export]
    public PackedScene PackedShoeHanger { get; set; }
    [Export]
    public PackedScene PackedKickParticle { get; set; }
    [Export]
    public int MaxShoes
    {
        get { return _maxShoes; }
        set { _maxShoes = value; }
    }
    [Export]
    public PackedScene PackedBodyExplosion { get; set; }

    public Node2D ArrowA { get; set; }

    private ShoeHanger _shoeHanger;
    private Node _projectilesContainer;
    private int _shoesCount = 0;
    private Timer _timerBulletALoader;
    private CPUParticles2D _kickPar;

    public override void _Ready()
    {
        _shoesCount = MaxShoes;
        _timerBulletALoader = GetNode<Timer>("TimerBulletALoader");
        _timerBulletALoader.Connect("timeout", this, nameof(OnTimerBulletALoadTimeout));
        var overlayer = GetTree().CurrentScene.GetNode("OverLayer");
        ArrowA = PackedArrow.Instance<Node2D>();
        overlayer.AddChild(ArrowA);
        ArrowA.Hide();
        _projectilesContainer = GetTree().CurrentScene.GetNode("GameWorld/Projectiles");
        var kicker = GetNode("../..");
        _shoeHanger = PackedShoeHanger.Instance<ShoeHanger>();
        kicker.CallDeferred("add_child", _shoeHanger);
        _kickPar = PackedKickParticle.Instance<CPUParticles2D>();
        kicker.CallDeferred("add_child", _kickPar);
    }

    public void ShootBulletA(Vector2 pos, Vector2 vel)
    {
        var bullet = PackedBulletA.Instance<BulletA>();
        bullet.Velocity = vel;
        bullet.GlobalPosition = pos;
        bullet.Rotation = vel.Normalized().Angle();
        _projectilesContainer.AddChild(bullet);
        var oldCount = _shoesCount;
        _shoesCount--;
        _shoeHanger.VisibleShoes = _shoesCount;
        if (oldCount >= MaxShoes)
        {
            _timerBulletALoader.Start();
        }
        _kickPar.Rotation = vel.Normalized().Angle();
        _kickPar.Emitting = true;
    }

    public bool CanShootBulletA()
    {
        return _shoesCount > 0;
    }

    private void OnTimerBulletALoadTimeout()
    {
        _shoesCount++;
        _shoeHanger.VisibleShoes = _shoesCount;
        if (_shoesCount >= MaxShoes)
        {
            return;
        }
        _timerBulletALoader.Start();
    }
}
