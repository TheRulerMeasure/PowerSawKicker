using Godot;
using System;

public class CpuKickerStateMachine : Node
{
    private NodePath _kickerNp = "..";

    [Export]
    public NodePath KickerNp
    {
        get { return _kickerNp; }
        set { _kickerNp = value; }
    }

    private CpuKickerState[] _states =
    {
        new CpuKickerStateLookingForDisc(),
        new CpuKickerStateGointToDisc(),
        new CpuKickerStateAimingAtDisc(),
        new CpuKickerStateDodging(),
        new CpuKickerStateQuickShooting(),
    };
    private CpuKickerState _curState;

    public override void _Ready()
    {
        OnKickerReady();
    }

    public override void _Process(float delta)
    {
        CpuKickerStates newState = _curState.Tick(delta);
        if (newState != CpuKickerStates.None)
        {
            ChangeState(_states[(int)newState]);
        }
    }

    private void ChangeState(CpuKickerState newState)
    {
        _curState?.Exit();
        _curState = newState;
        _curState.Enter();
    }

    private void OnKickerReady()
    {
        Kicker kicker = GetNode<Kicker>(KickerNp);
        CpuKickerPackage pck = GetNode<CpuKickerPackage>("CpuKickerPackage");
        foreach (var state in _states)
        {
            state.InitState(kicker, pck);
        }
        ChangeState(_states[0]);
    }
}
