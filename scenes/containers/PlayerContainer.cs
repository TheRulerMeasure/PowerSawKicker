using Godot;
using GDCollections = Godot.Collections;
using System;

public class PlayerContainer : Node2D
{
    [Signal] public delegate void PlayerLivesChanged(int lives);
    [Signal] public delegate void BotLivesChanged(int lives);
    [Signal] public delegate void PlayersCountChanged(int count);
    [Signal] public delegate void BotsCountChanged(int count);

    private Rect2 _spawnerRect = new Rect2(100, 100, 200, 100);

    private int _maxBotCount = 1;
    private int _playerLivesCount = 3;
    private int _botLivesCount = 3;

    private Color _player1Color = new Color("ff5d5d");
    private Color _player2Color = new Color("82e5ff");

    private float _maxInvincibleTime = 4.5f;
    private float _maxRespawnTime = 5;

    private PackedScene _packedKicker = ResourceLoader.Load<PackedScene>("res://scenes/mobs/Kicker.tscn");
    private PackedScene _packedKeyInput = ResourceLoader.Load<PackedScene>("res://scenes/keyinputs/KeyInputKicker.tscn");
    private PackedScene _packedCpuInput = ResourceLoader.Load<PackedScene>("res://scenes/artifictualinputs/CpuKickerStateMachine.tscn");

    [Export]
    public Rect2 SpawnerRect
    {
        get { return _spawnerRect; }
        set { _spawnerRect = value; }
    }

    [Export]
    public int MaxBotCount
    {
        get {  return _maxBotCount; }
        set { _maxBotCount = value; }
    }

    [Export]
    public int PlayerLivesCount
    {
        get { return _playerLivesCount; }
        set { _playerLivesCount = value; }
    }
    [Export]
    public int BotLivesCount
    {
        get { return _botLivesCount; }
        set { _botLivesCount = value; }
    }

    [Export]
    public Color Player1Color
    {
        get { return _player1Color; }
        set { _player1Color = value; }
    }
    [Export]
    public Color Player2Color
    {
        get { return _player2Color; }
        set { _player2Color = value; }
    }

    [Export]
    public float MaxInvincibleTime
    {
        get { return _maxInvincibleTime; }
        set { _maxInvincibleTime += value; }
    }
    [Export]
    public float MaxRespawnTime
    {
        get { return _maxRespawnTime; }
        set { _maxRespawnTime = value; }
    }

    [Export]
    public NodePath GameUserInterfaceNp { get; set; }

    private RandomNumberGenerator _rng = new RandomNumberGenerator();

    private int _botsCount = 0;
    private int _playersCount = 0;

    public override void _Ready()
    {
        var gg = GetNode<SceneTransistor>("/root/SceneTransistor");
        PlayerLivesCount = gg.Player1LifePointsCount;
        BotLivesCount = gg.Player2LifePointsCount;
        MaxBotCount = gg.Player2Count;

        _rng.Randomize();
        if (!GameUserInterfaceNp.IsEmpty())
        {
            var ui = GetNode<GameUserInterface>(GameUserInterfaceNp);
            var pl1 = ui.GetNode(ui.Player1LifePoint);
            var pl2 = ui.GetNode(ui.Player2LifePoint);
            Connect(nameof(PlayerLivesChanged), pl1, "OnLivesCountChanged");
            Connect(nameof(BotLivesChanged), pl2, "OnLivesCountChanged");
        }
        EmitSignal(nameof(PlayerLivesChanged), PlayerLivesCount);
        EmitSignal(nameof(BotLivesChanged), BotLivesCount);
    }

    public void InitializePlayers()
    {
        for (int i = 0; i < MaxBotCount; i++)
        {
            SpawnBotKickerAt(GetPointInSpawnerRect());
        }
        SpawnPlayerKickerAt(GetPointInSpawnerRect());
        _botsCount = MaxBotCount + BotLivesCount - 1;
        _playersCount = PlayerLivesCount;
    }

    public Vector2 GetPointInSpawnerRect()
    {
        var x = SpawnerRect.Position.x + _rng.Randf() * SpawnerRect.Size.x;
        var y = SpawnerRect.Position.y + _rng.Randf() * SpawnerRect.Size.y;
        return new Vector2(x, y);
    }

    public void SpawnPlayerKickerAt(Vector2 pos)
    {
        var kicker = _packedKicker.Instance<Kicker>();
        kicker.Invincible = true;
        kicker.Position = pos;
        
        var arr = new GDCollections.Array(kicker);
        kicker.Connect("ready", this, nameof(PlKickerReady), arr, (uint) ConnectFlags.Oneshot);
        kicker.Connect("Died", this, nameof(OnPlayerKickerExited), null, (uint) ConnectFlags.Oneshot);
        AddChild(kicker);
    }

    public void SpawnBotKickerAt(Vector2 pos)
    {
        var kicker = _packedKicker.Instance<Kicker>();
        kicker.Invincible = true;
        kicker.Position = pos;
        
        var arr = new GDCollections.Array(kicker);
        kicker.Connect("ready", this, nameof(BotKickerReady), arr, (uint) ConnectFlags.Oneshot);
        kicker.Connect("Died", this, nameof(OnBotKickerExited), null, (uint) ConnectFlags.Oneshot);
        AddChild(kicker);
    }

    private void PlKickerReady(Kicker kicker)
    {
        var input = _packedKeyInput.Instance();
        kicker.Sprite.Modulate = Player1Color;
        kicker.AddChild(input);
        var timer = GetTree().CreateTimer(MaxInvincibleTime);
        var arr = new GDCollections.Array(kicker);
        timer.Connect("timeout", this, nameof(OnKickerInvincibleTimeout), arr, (uint) ConnectFlags.Oneshot);
    }

    private void BotKickerReady(Kicker kicker)
    {
        var input = _packedCpuInput.Instance();
        kicker.Sprite.Modulate = Player2Color;
        kicker.AddChild(input);
        var timer = GetTree().CreateTimer(MaxInvincibleTime);
        var arr = new GDCollections.Array(kicker);
        timer.Connect("timeout", this, nameof(OnKickerInvincibleTimeout), arr, (uint) ConnectFlags.Oneshot);
    }

    private void OnKickerInvincibleTimeout(Kicker kicker)
    {
        kicker.Invincible = false;
    }

    private void OnBotKickerExited()
    {
        BotLivesCount--;
        EmitSignal(nameof(BotLivesChanged), BotLivesCount);
        GD.PrintS("Bot Lives =", BotLivesCount);
        _botsCount--;
        EmitSignal(nameof(BotsCountChanged), _botsCount);
        GD.PrintS("Bots =", _botsCount);
        if (BotLivesCount <= 0)
        {
            return;
        }
        var timer = GetTree().CreateTimer(MaxRespawnTime);
        timer.Connect("timeout", this, nameof(OnRequestBotSpawn), null, (uint) ConnectFlags.Oneshot);
    }

    private void OnPlayerKickerExited()
    {
        PlayerLivesCount--;
        EmitSignal(nameof(PlayerLivesChanged), PlayerLivesCount);
        _playersCount--;
        EmitSignal(nameof(PlayersCountChanged), _playersCount);
        GD.PrintS("Players =", _playersCount);
        if (PlayerLivesCount <= 0)
        {
            return;
        }
        var timer = GetTree().CreateTimer(MaxRespawnTime);
        timer.Connect("timeout", this, nameof(OnRequestPlayerSpawn), null, (uint) ConnectFlags.Oneshot);
    }

    private void OnRequestBotSpawn()
    {
        SpawnBotKickerAt(GetPointInSpawnerRect());
    }

    private void OnRequestPlayerSpawn()
    {
        SpawnPlayerKickerAt(GetPointInSpawnerRect());
    }
}
