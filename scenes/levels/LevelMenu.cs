using Godot;
using GDCollections = Godot.Collections;
using System;

public class LevelMenu : Node
{
    [Signal]
    public delegate void ChangeToPlayLevel();
    [Signal]
    public delegate void Player1LifePointsChanged(int points);
    [Signal]
    public delegate void Player2LifePointsChanged(int points);
    [Signal]
    public delegate void Player2CountChanged(int count);
    [Signal]
    public delegate void SawsCountChanged(int count);

    [Export]
    public NodePath PlayButtonNp { get; set; }
    [Export]
    public NodePath LvlChangerNp { get; set; }
    [Export]
    public NodePath CustomGameCheckboxNp { get; set; }
    [Export]
    public NodePath CustomGamePanelNp { get; set; }
    [Export]
    public NodePath SettingsNp { get; set; }

    private Control _customGamePanel;

    private HSlider _player1LifePointsSlider;
    private HSlider _player2LifePointsSlider;
    private HSlider _player2CountSlider;
    private HSlider _sawsCountSlider;

    private Label _player1LifePointsLabel;
    private Label _player2LifePointsLabel;
    private Label _player2CountLabel;
    private Label _sawsCountLabel;

    public override void _Ready()
    {
        Node sliders = GetNode(SettingsNp);
        _player1LifePointsLabel = sliders.GetNode<Label>("HBoxContainer/Label");
        _player1LifePointsSlider = sliders.GetNode<HSlider>("HBoxContainer/HSlider");
        _player1LifePointsSlider.Connect("value_changed", this, nameof(OnSliderChanged), new GDCollections.Array(0));
        _player2LifePointsLabel = sliders.GetNode<Label>("HBoxContainer2/Label");
        _player2LifePointsSlider = sliders.GetNode<HSlider>("HBoxContainer2/HSlider");
        _player2LifePointsSlider.Connect("value_changed", this, nameof(OnSliderChanged), new GDCollections.Array(1));
        _player2CountLabel = sliders.GetNode<Label>("HBoxContainer3/Label");
        _player2CountSlider = sliders.GetNode<HSlider>("HBoxContainer3/HSlider");
        _player2CountSlider.Connect("value_changed", this, nameof(OnSliderChanged), new GDCollections.Array(2));
        _sawsCountLabel = sliders.GetNode<Label>("HBoxContainer4/Label");
        _sawsCountSlider = sliders.GetNode<HSlider>("HBoxContainer4/HSlider");
        _sawsCountSlider.Connect("value_changed", this, nameof(OnSliderChanged), new GDCollections.Array(3));

        var node = GetNode<Button>(PlayButtonNp);
        node.Connect("pressed", this, nameof(OnPlayButtonPressed));
        Connect(nameof(ChangeToPlayLevel), GetNode(LvlChangerNp), "ChangeLevel");
        _customGamePanel = GetNode<Control>(CustomGamePanelNp);
        var checkbox = GetNode<CheckBox>(CustomGameCheckboxNp);
        checkbox.Connect("toggled", this, nameof(OnCustomGameToggled));

        var gg = GetNode("/root/SceneTransistor");
        Connect(nameof(Player1LifePointsChanged), gg, "OnPlayer1LifePointsChanged");
        Connect(nameof(Player2LifePointsChanged), gg, "OnPlayer2LifePointsChanged");
        Connect(nameof(Player2CountChanged), gg, "OnPlayer2CountChanged");
        Connect(nameof(SawsCountChanged), gg, "OnSawsCountChanged");
        EmitDefaultSettings();
    }

    private void OnCustomGameToggled(bool b)
    {
        _customGamePanel.Visible = b;
        if (!b)
        {
            EmitDefaultSettings();
        }
        else
        {
            EmitCustomSettings();
        }
    }

    private void EmitDefaultSettings()
    {
        EmitSignal(nameof(Player1LifePointsChanged), 5);
        EmitSignal(nameof(Player2LifePointsChanged), 5);
        EmitSignal(nameof(Player2CountChanged), 1);
        EmitSignal(nameof(SawsCountChanged), 2);
    }

    private void EmitCustomSettings()
    {
        _player1LifePointsLabel.Text = _player1LifePointsSlider.Value.ToString();
        EmitSignal(nameof(Player1LifePointsChanged), (int) _player1LifePointsSlider.Value);
        _player2LifePointsLabel.Text = _player2LifePointsSlider.Value.ToString();
        EmitSignal(nameof(Player2LifePointsChanged), (int) _player2LifePointsSlider.Value);
        _player2CountLabel.Text = _player2CountSlider.Value.ToString();
        EmitSignal(nameof(Player2CountChanged), (int) _player2CountSlider.Value);
        _sawsCountLabel.Text = _sawsCountSlider.Value.ToString();
        EmitSignal(nameof(SawsCountChanged), (int) _sawsCountSlider.Value);
    }

    private void OnPlayButtonPressed()
    {
        EmitSignal(nameof(ChangeToPlayLevel));
    }

    private void OnSliderChanged(float v, int index)
    {
        switch (index)
        {
            case 1:
                _player2LifePointsLabel.Text = v.ToString();
                EmitSignal(nameof(Player2LifePointsChanged), (int)v);
                break;
            case 2:
                _player2CountLabel.Text = v.ToString();
                EmitSignal(nameof(Player2CountChanged), (int)v);
                break;
            case 3:
                _sawsCountLabel.Text = v.ToString();
                EmitSignal(nameof(SawsCountChanged), (int)v);
                break;
            default:
                _player1LifePointsLabel.Text = v.ToString();
                EmitSignal(nameof(Player1LifePointsChanged), (int)v);
                break;
        }
    }
}
