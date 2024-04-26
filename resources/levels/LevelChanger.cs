using Godot;
using System;

public class LevelChanger : Node
{
    [Export(PropertyHint.File, "*.tscn")]
    public string NewLevel { get; set; }

    public void ChangeLevel()
    {
        GetTree().ChangeScene(NewLevel);
    }
}
