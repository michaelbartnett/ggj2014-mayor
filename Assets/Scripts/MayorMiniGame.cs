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
    public float increaseSpeedEachRoundBy = 1f;

    public bool Running { get; private set; }
    public bool SliderRunning { get { return sliderTween != null; } }

    public event Action<MayorMiniGame> MiniGameInitiated;
    public event Action<MayorMiniGame> MiniGameBegan;
    public event Action<MayorMiniGame, bool> MiniGameFinished;

    public static MayorMiniGame Instance { get; private set; }

    private GoTween sliderTween;
    private Vector3 startLocalPos;
    private bool playerWonLastRound = false;


    void Awake()
    {
        Running = false;
        Instance = this;
        startLocalPos = this.transform.localPosition;
        this.transform.localPosition -= Vector3.right * 15f;
    }

    void OnDestroy()
    {
        Instance = null;
    }

    public void BeginGame()
    {
        StartCoroutine(RunMiniGame());
    }

    private IEnumerator RunMiniGame()
    {
        Running = true;
        DialogueDisplay.Instance.ShowDialogue("The Mayor", "Hello citizen!\n\n(Stab the mayor 3 times with <E>. You lose if you miss 3 times.)");
        Audio.Instance.FadeOutBallroomMusic(3f);
        var tcfg = new GoTweenConfig()
            .localPosition(startLocalPos)
            .setEaseType(GoEaseType.BackOut);
        yield return Go.to(this.transform, 0.35f, tcfg).waitForCompletion();
        if (MiniGameInitiated != null) MiniGameInitiated(this);

        float speed = sliderSpeed;
        Player.Instance.DisableControls();

        float t = Time.time;
        while (!Input.GetKeyDown(KeyCode.E)) yield return null;
        float dt = Math.Max(0, 1.25f - Time.time - t);
        Audio.Instance.PlayBossMusic();
        yield return new WaitForSeconds(dt);
        DialogueDisplay.Instance.ChangeDialogue("The Mayor", "Wait you're not my pusher! Guards!");
        if (MiniGameBegan != null) MiniGameBegan(this);

        int hitsToWIn = 3;
        int missesToLose = 3;

        while (true) {
            DoSlide(speed);
            speed += increaseSpeedEachRoundBy;
            while (sliderTween != null) yield return null;
            DialogueDisplay.Instance.ChangeDialogue("The Mayor", playerWonLastRound ? "OOF!" : "One does not simply stab\n ...\nTHE MAYOR!");
            if (playerWonLastRound) {
                hitsToWIn--;
            } else {
                missesToLose--;
            }

            if (hitsToWIn < 1) {
                EndGame(true);
                break;
            } else if (missesToLose < 1) {
                EndGame(false);
                break;
            }

            yield return new WaitForSeconds(0.65f);
        }
        Audio.Instance.FadeOutBossMusic(3f);
    }

    private void DoSlide(float speedThisRound)
    {
        var sliderStart = barBeginXform.position;
        var sliderEnd = barEndXform.position;
        float slideDuration = Mathf.Abs(sliderEnd.x - sliderStart.x) / speedThisRound;
        var tweenCfg = new GoTweenConfig()
            .position(sliderEnd)
            .onComplete(OnSliderReachedEnd);
        slider.position = sliderStart;
        sliderTween = Go.to(slider, slideDuration, tweenCfg);
    }

    private void OnSliderReachedEnd(AbstractGoTween _)
    {
        sliderTween = null;
        playerWonLastRound = false;
    }

    public void PlayerAttacks()
    {
        float sliderX = slider.position.x;
        sliderTween.destroy();
        sliderTween = null;
        if (sliderX > (successBeginXform.position.x - leeway) && sliderX < (successEndXform.position.x + leeway)) {
            playerWonLastRound = true;
            Audio.Instance.PlayKnifeHit();
        } else {
            playerWonLastRound = false;
            Audio.Instance.PlayKnifeMiss();
        }
    }

    private void EndGame(bool playerWon)
    {
        Running = false;
        Running = false;
        if (MiniGameFinished != null) MiniGameFinished(this, playerWon);
        Debug.LogWarning("MINIGAME OVER, PLAYER WON? " + playerWon);
        StartCoroutine(EndScreen(playerWon));
    }

    IEnumerator EndScreen(bool playerWon)
    {
        DialogueDisplay.Instance.ChangeDialogue("The Mayor", playerWon ? "I shoulda crushed you like a cockroach." : "That's what you get when you mess with the Candy!");
        yield return new WaitForSeconds(1.0f);
        DialogueDisplay.Instance.HideDialogue();
        MainScreensScript.Instance.SetGameState(playerWon ? GameState.Win : GameState.Lose);
        float initTime = Time.time;
        while (!(Time.time - initTime > 10f || ((Time.time - initTime > 1) && Input.GetKeyDown(KeyCode.E)))) yield return null;
        Application.LoadLevel(0);
    }
}