using UnityEngine;

public class BobberHookedFishState : BobberState
{
    public override BobberStateType State => BobberStateType.HookedFish;
    private float _currentLineTension = 0f;
    private GameObject _lineTensionBar;

    public BobberHookedFishState(BobberController bobber) : base(bobber) { }

    public override void EnterState()
    {
        _bobber.StopAllCoroutines();
        ActivateLineTensionBar();
    }

    public override void UpdateState()
    {
        if (Input.GetMouseButton(1))
        {
            BeginReelingInFish();
        }
        else  // Decrease line tension
        {
            FishingPole.Instance.StopReelingSfx();
            UpdateLineTension(false);
        }
    }

    public override void ExitState()
    {
        _bobber.StopAllCoroutines();
        _lineTensionBar.SetActive(false);
    }

    private void ActivateLineTensionBar()
    {
        GameObject lineTensionCanvas = GameObject
            .Find(Prefabs.LINE_TENSION_CANVAS);

        _lineTensionBar = lineTensionCanvas.transform
            .Find(Prefabs.LINE_TENSION_BAR)
            .gameObject;

        _lineTensionBar.SetActive(true);
    }

    private void BeginReelingInFish()
    {
        FishStateType fishStateType = FishController.Instance.CurrentStateType;
        FishingPole.Instance.PlayReelingSfx();

        if (fishStateType == FishStateType.Fighting)
        {
            if (_currentLineTension >= _bobber.MaxLineTension)
            {
                SnapLine();
            }
            else  // Increase line tension
            {
                UpdateLineTension(true);
            }
        }
        else  // Decrease line Tension
        {
            UpdateLineTension(false);
        }
    }

    private void SnapLine()
    {
        _bobber.SetState(new BobberIdleState(_bobber));
        FishController.Instance
            .SetState(new FishSwimmingAwayState(FishController.Instance));
    }

    private void UpdateLineTension(bool shouldIncreaseTension)
    {
        float tensionChangeSpeed = shouldIncreaseTension
            ? _bobber.LineTensionIncreaseSpeed
            : _bobber.LineTensionDecreaseSpeed * -1;

        _currentLineTension += tensionChangeSpeed * Time.deltaTime;
        _currentLineTension = Mathf.Max(_currentLineTension, 0f);

        LineTensionBarController.Instance.SetLineTension(_currentLineTension);
    }
}
