using Godot;
using System;

public class KickerStateMachine : Node
{
    private NodePath _kickerBodyNp = "..";

    [Export] public NodePath KickerBodyNp
    {
        get { return _kickerBodyNp; }
        set { _kickerBodyNp = value; }
    }

    private KickerState[] _states = {
        new KickerStateIdling(),
        new KickerStateMoving(),
        new KickerStateAiming(),
        new KickerStateAimMoving(),
        new KickerStateAttack2Recovering(),
        new KickerStateDying(),
    };

    private KickerState _curState;

    public override void _EnterTree()
    {
        GetNode(KickerBodyNp).Connect("ready", this, nameof(OnKickerReady), null, (uint) ConnectFlags.Oneshot);
    }

    public override void _PhysicsProcess(float delta)
    {
        KickerStates newState = _curState.TickPhysics(delta);
        if (newState != KickerStates.None)
        {
            ChangeState(_states[(int) newState]);
        }
    }

    private void InitStates()
    {
        Kicker k = GetNode<Kicker>(KickerBodyNp);
        KickerPackage pck = GetNode<KickerPackage>("KickerPackage");
        foreach (var state in _states)
        {
            state.InitState(k, pck);
        }
    }

    private void ChangeState(KickerState state)
    {
        _curState?.Exit();
        _curState = state;
        _curState.Enter();
    }

    private void OnKickerReady()
    {
        InitStates();
        ChangeState(_states[0]);
    }
}
