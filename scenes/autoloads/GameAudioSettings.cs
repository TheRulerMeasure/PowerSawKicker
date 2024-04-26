using Godot;
using System;

public class GameAudioSettings : Node
{
    private float _volume = 0.8f;

    [Export]
    public float Volume
    {
        get { return _volume; }
        set { _volume = value; }
    }
}
