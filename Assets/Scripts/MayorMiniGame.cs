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

    public event Action<MayorMiniGame, bool> MiniGameFinished;

    public static MayorMiniGame Instance { get; private set; }

    private GoTween sliderTween;

    IEnumerator Start()
    {
        while (!Running) {
            if (Input.GetKeyDown(KeyCode.O)) {
                BeginGame();
                break;
            }
            yield return null;
        }

        while (true) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                PlayerAttacks();
            }
            yield return null;
        }
    }

    void Awake()
    {
        Running = false;
        Instance = this;
    }

    public void BeginGame()
    {
        var sliderStart = barBeginXform.position;
        var sliderEnd = barEndXform.position;
        float slideDuration = Mathf.Abs(sliderEnd.x - sliderStart.x) / sliderSpeed;
        var tweenCfg = new GoTweenConfig()
            .position(sliderEnd)
            .onComplete(OnSlideCompleted);
        slider.position = sliderStart;

        sliderTween = Go.to(slider, slideDuration, tweenCfg);
        Running = true;
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
    }

    private void OnSlideCompleted(AbstractGoTween _)
    {
        if (MiniGameFinished != null) EndGame(false);
    }
}