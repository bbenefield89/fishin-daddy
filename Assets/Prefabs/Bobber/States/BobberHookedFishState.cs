using UnityEngine;

public class BobberHookedFishState : BobberState
{
    public override BobberStateType State => BobberStateType.HookedFish;
    private float _currentLineTension = 0f;

    public BobberHookedFishState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _bobber.StopAllCoroutines();
    }

    public override void UpdateState()
    {
        FishStateType fishStateType =
            FishController.Instance.CurrentStateType;

        if (
            Input.GetMouseButton(1)
            && fishStateType == FishStateType.Fighting)
        {
            if (_currentLineTension >= _bobber.MaxLineTension)
            {
                _bobber.SetState(new BobberIdleState(_bobber));
                FishController.Instance.SetState(new FishSwimmingAwayState(FishController.Instance));
            }
            else  // Increase line tension
            {
                _currentLineTension +=
                    _bobber.LineTensionIncreaseSpeed * Time.deltaTime;
            }
        }
        else  // Decrease line tension
        {
            float reduceLineTensionBy =
                _bobber.LineTensionDecreaseSpeed * Time.deltaTime;

            _currentLineTension -=
                Mathf.Clamp(reduceLineTensionBy, 0f, _bobber.MaxLineTension);
        }
    }

    public override void ExitState()
    {
        _bobber.StopAllCoroutines();
    }

}
