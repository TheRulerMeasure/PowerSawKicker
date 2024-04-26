using Godot;
using System;

public class GameUserInterface : Node
{
    [Export]
    public NodePath Player1LifePoint { get; set; }
    [Export]
    public NodePath Player2LifePoint { get; set; }
}
