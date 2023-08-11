using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BobberState
{
    protected BobberController _bobber;

    public BobberState(BobberController bobber)
    {
        _bobber = bobber;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
