using Godot;
using GDCollections = Godot.Collections;
using System;

public class LevelResult : Node
{
    private const string WINNER_LABEL_NP = "CanvasLayer/Control/MarginContainer/CenterContainer/VBoxContainer/MarginContainer3/WinnerLabel";
    private const string PLAYER1_DEATHS_LABEL_NP = "CanvasLayer/Control/MarginContainer/CenterContainer/VBoxContainer/MarginContainer/CenterContainer/HBoxContainer/VBoxContainer2/MarginContainer/LabelPlayer1Deaths";
    private const string PLAYER2_DEATHS_LABEL_NP = "CanvasLayer/Control/MarginContainer/CenterContainer/VBoxContainer/MarginContainer/CenterContainer/HBoxContainer/VBoxContainer2/MarginContainer2/LabelPlayer2Deaths";
    private const string PLAYER1_KICKS_LABEL_NP = "CanvasLayer/Control/MarginContainer/CenterContainer/VBoxContainer/MarginContainer2/CenterContainer/HBoxContainer/VBoxContainer2/MarginContainer/LabelPlayer1Kicks";
    private const string PLAYER2_KICKS_LABEL_NP = "CanvasLayer/Control/MarginContainer/CenterContainer/VBoxContainer/MarginContainer2/CenterContainer/HBoxContainer/VBoxContainer2/MarginContainer2/LabelPlayer2Kicks";
    private const string BUTTON_NP = "CanvasLayer/Control/MarginContainer/CenterContainer/VBoxContainer/Button";

    private Color _player1Color = new Color("ff5d5d");
    private Color _player2Color = new Color("82e5ff");

    private int _winningPlayer = 0;
    private int _player1DeathsCount = 0;
    private int _player2DeathsCount = 0;
    private int _player1KicksCount = 0;
    private int _player2KicksCount = 0;

    [Export(PropertyHint.Enum, "Player1,Player2")]
    public int WinningPlayer
    {
        get { return _winningPlayer; }
        set { _winningPlayer = value; }
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
    public int Player1DeathsCount
    {
        get { return _player1DeathsCount; }
        set { _player1DeathsCount = value; }
    }
    [Export]
    public int Player2DeathsCount
    {
        get { return _player2DeathsCount; }
        set { _player2DeathsCount = value; }
    }
    [Export]
    public int Player1KicksCount
    {
        get { return _player1KicksCount; }
        set { _player1KicksCount = value; }
    }
    [Export]
    public int Player2KicksCount
    {
        get { return _player2KicksCount; }
        set { _player2KicksCount = value; }
    }

    [Export(PropertyHint.File, "*.tscn")]
    public string LevelMenuFilePath { get; set; }

    public override void _Ready()
    {
        var gg = GetNode<SceneTransistor>("/root/SceneTransistor");
        WinningPlayer = gg.Winner;
        Player1DeathsCount = gg.Player1DeathsCount;
        Player2DeathsCount = gg.Player2DeathsCount;
        Player1KicksCount = gg.Player1KicksCount;
        Player2KicksCount = gg.Player2KicksCount;

        var winLabel = GetNode<Label>(WINNER_LABEL_NP);
        winLabel.Hide();
        if (WinningPlayer >= 1)
        {
            winLabel.Text = "Opponent Wins!";
            winLabel.Modulate = Player2Color;
        }
        else
        {
            winLabel.Text = "Player Wins!";
            winLabel.Modulate = Player1Color;
        }
        GetTree().CreateTimer(1.5f).Connect("timeout", winLabel, "show");
        Button b = GetNode<Button>(BUTTON_NP);
        b.Connect("pressed", this, nameof(ChangeToMenuLevel));
        GetTree().CreateTimer(2.5f).Connect("timeout", b, "set", new GDCollections.Array("disabled", false), (uint) ConnectFlags.Oneshot);
        var tween = CreateTween();
        Label pl1DeathsLabel = GetNode<Label>(PLAYER1_DEATHS_LABEL_NP);
        Label pl2DeathsLabel = GetNode<Label>(PLAYER2_DEATHS_LABEL_NP);
        tween.TweenMethod(this, nameof(SetLabelNumber), 0, Player1DeathsCount, 1, new GDCollections.Array(pl1DeathsLabel)).SetDelay(1.75f);
        tween.TweenMethod(this, nameof(SetLabelNumber), 0, Player2DeathsCount, 1, new GDCollections.Array(pl2DeathsLabel)).SetDelay(0.25f);
        Label pl1KicksLabel = GetNode<Label>(PLAYER1_KICKS_LABEL_NP);
        Label pl2KicksLabel = GetNode<Label>(PLAYER2_KICKS_LABEL_NP);
        tween.TweenMethod(this, nameof(SetLabelNumber), 0, Player1KicksCount, 1, new GDCollections.Array(pl1KicksLabel)).SetDelay(0.25f);
        tween.TweenMethod(this, nameof(SetLabelNumber), 0, Player2KicksCount, 1, new GDCollections.Array(pl2KicksLabel)).SetDelay(0.25f);
    }

    private void SetLabelNumber(int n, Label label)
    {
        label.Text = n.ToString();
    }

    private void ChangeToMenuLevel()
    {
        GetTree().ChangeScene(LevelMenuFilePath);
    }
}
