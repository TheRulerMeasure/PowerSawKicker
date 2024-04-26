using Godot;
using System;

public class KeyInputKicker : Node
{
    [Signal] public delegate void MotionAxisChanged(Vector2 axis);
    [Signal] public delegate void FaceDirectionChanged(Vector2 dir);
    [Signal] public delegate void Attack1High();
    [Signal] public delegate void Attack1Low();
    [Signal] public delegate void Attack2High();
    [Signal] public delegate void Attack2Low();

    private NodePath _kickerNp = "..";

    [Export]
    public NodePath KickerNp
    { get => _kickerNp; set => _kickerNp = value; }

    private Node2D _node;

    public override void _Ready()
    {
        _node = GetNode<Node2D>(KickerNp);
        Connect(nameof(MotionAxisChanged), _node, "OnMotionAxisChanged");
        Connect(nameof(FaceDirectionChanged), _node, "OnFacingDirectionChanged");
        Connect(nameof(Attack1Low), _node, "OnAttack1Lowed");
        Connect(nameof(Attack1High), _node, "OnAttack1Highed");
        Connect(nameof(Attack2Low), _node, "OnAttack2Lowed");
        Connect(nameof(Attack2High), _node, "OnAttack2Highed");
    }

    public override void _Process(float delta)
    {
        float x = Input.GetAxis("move_left", "move_right");
        float y = Input.GetAxis("move_up", "move_down");
        EmitSignal(nameof(MotionAxisChanged), new Vector2(x, y).Normalized());
        Vector2 faceDir = _node.GlobalPosition.DirectionTo(_node.GetGlobalMousePosition());
        EmitSignal(nameof(FaceDirectionChanged), faceDir);
        if (Input.IsActionJustPressed("attack1"))
        {
            EmitSignal(nameof(Attack1High));
        }
        else if (Input.IsActionJustReleased("attack1"))
        {
            EmitSignal(nameof(Attack1Low));
        }
        if (Input.IsActionJustPressed("attack2"))
        {
            EmitSignal(nameof(Attack2High));
        }
        else if (Input.IsActionJustReleased("attack2"))
        {
            EmitSignal(nameof(Attack2Low));
        }
    }
}
