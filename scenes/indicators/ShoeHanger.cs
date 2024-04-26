using Godot;
using GDCollections = Godot.Collections;
using System;

public class ShoeHanger : Node2D
{
    private int _visibleShoes = 2;
    private float _rotationSpeed = 45;
    private float _radius = 22;

    [Export]
    public int VisibleShoes
    {
        get { return _visibleShoes; }
        set { SetVisibleShoes(value); }
    }
    [Export(PropertyHint.Range, "0.2,1000.0,0.2")]
    public float RotationSpeed
    {
        get { return _rotationSpeed; }
        set { _rotationSpeed = value; }
    }
    [Export(PropertyHint.Range, "0.02,50.0,0.02")]
    public float Radius
    {
        get { return _radius; }
        set { _radius = value; }
    }

    private GDCollections.Array<Sprite> _sprites = new GDCollections.Array<Sprite>();

    public override void _Ready()
    {
        foreach (Sprite sprite in GetChildren())
        {
            sprite.Hide();
            _sprites.Add(sprite);
        }
        ShowShoes(VisibleShoes);
    }

    public override void _Process(float delta)
    {
        Rotation += Mathf.Deg2Rad(RotationSpeed) * delta;
        if (Rotation >= Mathf.Tau)
        {
            Rotation -= Mathf.Tau;
        }
        var amount = Mathf.Min(_sprites.Count, VisibleShoes);
        for (int i = 0; i < amount; i++)
        {
            Sprite sprite = _sprites[i];
            sprite.GlobalRotation = Mathf.Sin(Rotation * 6) * 1.5f;
        }
    }

    private void SetVisibleShoes(int shoes)
    {
        _visibleShoes = shoes;
        if (!IsProcessing())
        {
            return;
        }
        ShowShoes(_visibleShoes);
    }

    private void ShowShoes(int shoes)
    {
        if (shoes <= 0)
        {
            foreach (Sprite sprite in GetChildren())
            {
                sprite.Hide();
            }
            return;
        }
        int childCount = GetChildCount();
        float amount = Mathf.Min(childCount, shoes);
        for (int i = 0; i < amount; i++)
        {
            float ang = i / amount;
            ang *= Mathf.Tau;
            Vector2 pos = new Vector2(Mathf.Cos(ang), Mathf.Sin(ang));
            _sprites[i].Position = pos * Radius;
            _sprites[i].Show();
        }
        if (amount >= childCount)
        {
            return;
        }
        for (int i = childCount - 1; i >= amount; i--)
        {
            _sprites[i].Hide();
        }
    }
}
