using Godot;
using System;

public class Level : Node
{
    [Signal] public delegate void InitDeployer();
    [Signal] public delegate void InitPlayers();
    [Signal] public delegate void Player1Won();
    [Signal] public delegate void Player2Won();
    [Signal] public delegate void RequestClearData();

    [Export(PropertyHint.File, "*.tscn")]
    public string LevelResultFilePath { get; set; }

    [Export]
    public NodePath PlayerContainerNp { get; set; }
    [Export]
    public NodePath BallContainerNp { get; set; }

    private LevelGameStates _curState = LevelGameStates.Starting;

    public override void _Ready()
    {
        var node = GetNode(PlayerContainerNp);
        node.Connect("PlayersCountChanged", this, nameof(OnPlayersCountChanged));
        node.Connect("BotsCountChanged", this, nameof(OnBotsCountChanged));
        Connect(nameof(InitPlayers), node, "InitializePlayers", null, (uint) ConnectFlags.Oneshot);

        var node2 = GetNode(BallContainerNp);
        Connect(nameof(InitDeployer), node2, "InitializeDeployer", null, (uint) ConnectFlags.Oneshot);

        var timer = GetTree().CreateTimer(2);
        timer.Connect("timeout", this, nameof(OnInitTimerTimeout), null, (uint) ConnectFlags.Oneshot);
        var timer2 = GetTree().CreateTimer(3.5f);
        timer2.Connect("timeout", this, nameof(OnDeployTimerTimeout));

        var gg = GetNode("/root/SceneTransistor");
        node.Connect("PlayersCountChanged", gg, "OnPlayer1Died");
        node.Connect("BotsCountChanged", gg, "OnPlayer2Died");
        Connect(nameof(RequestClearData), gg, "OnClearRequested");
        Connect(nameof(Player1Won), gg, "OnPlayer1Won");
        Connect(nameof(Player2Won), gg, "OnPlayer2Won");
        EmitSignal(nameof(RequestClearData));
    }

    private void OnPlayersCountChanged(int count)
    {
        if (_curState != LevelGameStates.Gaming)
        {
            return;
        }
        if (count <= 0)
        {
            _curState = LevelGameStates.BotWinning;
            EmitSignal(nameof(Player2Won));
            GetTree().CreateTimer(5).Connect("timeout", this, nameof(OnWinTimerTimeout));
        }
    }

    private void OnBotsCountChanged(int count)
    {
        if (_curState != LevelGameStates.Gaming)
        {
            return;
        }
        if (count <= 0)
        {
            _curState = LevelGameStates.PlayerWinning;
            EmitSignal(nameof(Player1Won));
            GetTree().CreateTimer(5).Connect("timeout", this, nameof(OnWinTimerTimeout));
        }
    }

    private void OnWinTimerTimeout()
    {
        ChangeToLevelResult();
    }

    private void OnInitTimerTimeout()
    {
        EmitSignal(nameof(InitPlayers));
        _curState = LevelGameStates.Gaming;
    }

    private void OnDeployTimerTimeout()
    {
        EmitSignal(nameof(InitDeployer));
    }

    private void ChangeToLevelResult()
    {
        GetTree().ChangeScene(LevelResultFilePath);
    }
}
