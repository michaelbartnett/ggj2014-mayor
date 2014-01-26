using UnityEngine;
using System.Collections;
using System;

public class MayorMiniGame : MonoBehaviour
{
    public Transform successBeginXform;
    public Transform successEndXform;
    public Transform barBeginXform;
    public Transform barEndXform;
    public float leeway = 0f;

    public Transform slider;
    public float sliderSpeed;

    public bool Running { get; private set; }

    public event Action<MayorMiniGame> MiniGameInitiated;
    public event Action<MayorMiniGame> MiniGameBegan;
    public event Action<MayorMiniGame, bool> MiniGameFinished;

    public static MayorMiniGame Instance { get; private set; }

    private GoTween sliderTween;
    private Vector3 startLocalPos;

    void Awake()
    {
        Running = false;
        Instance = this;
        startLocalPos = this.transform.localPosition;
        this.transform.localPosition -= Vector3.right * 15f;
    }

    public void BeginGame()
    {
        DialogueDisplay.Instance.ShowDialogue("The Mayor", "What is the meaning of this!?");
        var tcfg = new GoTweenConfig()
            .localPosition(startLocalPos)
            .setEaseType(GoEaseType.BackOut)
            .onComplete(OnMiniGameEnterComplete);
        Go.to(this.transform, 0.35f, tcfg);
        if (MiniGameInitiated != null) MiniGameInitiated(this);
            
    }

    private void OnMiniGameEnterComplete(AbstractGoTween _)
    {
        StartCoroutine(BeginGameForReal());
    }

    private IEnumerator BeginGameForReal()
    {
        yield return new WaitForSeconds(1.25f);
        var sliderStart = barBeginXform.position;
        var sliderEnd = barEndXform.position;
        float slideDuration = Mathf.Abs(sliderEnd.x - sliderStart.x) / sliderSpeed;
        var tweenCfg = new GoTweenConfig()
            .position(sliderEnd)
            .onComplete(OnSlideCompleted);
        slider.position = sliderStart;

        sliderTween = Go.to(slider, slideDuration, tweenCfg);
        Running = true;
        if (MiniGameBegan != null) MiniGameBegan(this);
    }

    public void PlayerAttacks()
    {
        float sliderX = slider.position.x;

        if (sliderX > (successBeginXform.position.x - leeway) && sliderX < (successEndXform.position.x + leeway)) {
            EndGame(true);
        } else {
            EndGame(false);
        }
    }

    private void EndGame(bool playerWon)
    {
        sliderTween.destroy();
        sliderTween = null;
        Running = false;
        if (MiniGameFinished != null) MiniGameFinished(this, playerWon);
        Debug.LogWarning("MINIGAME OVER, PLAYER WON? " + playerWon);
        DialogueDisplay.Instance.ChangeDialogue("The Mayor", playerWon ? "*dies*" : "No one can stop me!");
    }

    private void OnSlideCompleted(AbstractGoTween _)
    {
        if (MiniGameFinished != null) EndGame(false);
    }
}