using Godot;
using GDCollections = Godot.Collections;
using System;

public class SceneTransistor : Node
{
    public int Winner = 0;
    public int Player1DeathsCount = 0;
    public int Player2DeathsCount = 0;
    public int Player1KicksCount = 0;
    public int Player2KicksCount = 0;
    public int Player1LifePointsCount = 5;
    public int Player2LifePointsCount = 5;
    public int Player2Count = 1;
    public int SawsCount = 2;

    public override void _Ready()
    {
        
    }

    private void OnPlayer1Died(int count)
    {
        Player1DeathsCount++;
    }

    private void OnPlayer2Died(int count)
    {
        Player2DeathsCount++;
    }

    private void OnPlayer1Kicked()
    {
        Player1KicksCount++;
    }

    private void OnPlayer2Kicked()
    {
        Player2KicksCount++;
    }

    private void OnPlayer1Won()
    {
        Winner = 0;
    }

    private void OnPlayer2Won()
    {
        Winner = 1;
    }

    private void OnClearRequested()
    {
        Winner = 0;
        Player1KicksCount = 0;
        Player2KicksCount = 0;
        Player1DeathsCount = 0;
        Player2DeathsCount = 0;
    }

    private void OnPlayer1LifePointsChanged(int count)
    {
        Player1LifePointsCount = count;
    }

    private void OnPlayer2LifePointsChanged(int count)
    {
        Player2LifePointsCount = count;
    }

    private void OnPlayer2CountChanged(int count)
    {
        Player2Count = count;
    }

    private void OnSawsCountChanged(int count)
    {
        SawsCount = count;
    }
}
