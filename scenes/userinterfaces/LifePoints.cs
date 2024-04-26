using Godot;
using GDCollections = Godot.Collections;
using System;

public class LifePoints : Control
{
    private const int HEART_TEXTURES_COUNT = 3;

    private int _livesCount = 3;

    [Export]
    public int LivesCount
    {
        get { return _livesCount; }
        set { SetLivesCount(value); }
    }

    private GDCollections.Array<Control> _liveTextures = new GDCollections.Array<Control>();
    private Label _liveLabel;

    public override void _Ready()
    {
        _liveLabel = GetChild<Label>(3);
        for (int i = 0; i < HEART_TEXTURES_COUNT; i++)
        {
            _liveTextures.Add(GetChild<Control>(i));
        }
        UpdateUI(LivesCount);
    }

    private void SetLivesCount(int livesCount)
    {
        _livesCount = livesCount;
        UpdateUI(_livesCount);
    }

    private void UpdateUI(int livesCount)
    {
        if (_liveTextures.Count <= 0)
        {
            return;
        }
        if (_livesCount >= 4)
        {
            HideAll();
            _liveTextures[0].Show();
            _liveLabel.Show();
            _liveLabel.Text = _livesCount.ToString();
            return;
        }
        HideAll();
        int amount = Mathf.Clamp(livesCount, 0, HEART_TEXTURES_COUNT);
        for (int i = 0; i < amount; i++)
        {
            _liveTextures[i].Show();
        }
    }

    private void HideAll()
    {
        for (int i = 0; i < HEART_TEXTURES_COUNT; i++)
        {
            _liveTextures[i].Hide();
        }
        _liveLabel.Hide();
    }

    private void OnLivesCountChanged(int count)
    {
        LivesCount = count;
    }
}
