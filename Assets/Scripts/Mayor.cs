using UnityEngine;
using System.Collections;

public class Mayor : MonoBehaviour
{
    public PartyGoer[] guards;
    public MessageForwarder guardTrigger;
    public float blockPlayerDistance = 0.5f;
    public Animator mayorAnimator;

    private int pushingPlayer = 0;

    void Awake()
    {
        //guardTrigger.ControllerColliderHit += OnGuardTriggerEntered;

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, blockPlayerDistance);
    }

    IEnumerator Start()
    {
        MayorMiniGame.Instance.MiniGameFinished += OnMiniGameFinished;
        while (true) {
            var player = Player.Instance;
            if (player != null) {
                var playerPos = player.transform.position.ToXZ();
                var myPos = this.transform.position.ToXZ();
                if ((playerPos - myPos).sqrMagnitude < (blockPlayerDistance * blockPlayerDistance)) {
                    if (!CheckWithGuards()) {
                        PushPlayerBackRepositionGuards(player);
                        player.DisableControls();
                        while (pushingPlayer > 0) yield return null;
                        DialogueDisplay.Instance.ChangeDialogue("Guards", "You must not speak to the Mayor.");
                        while (!Input.GetKeyDown(KeyCode.E)) yield return null;
                        DialogueDisplay.Instance.HideDialogue();
                        Player.Instance.EnableControls();
                    }
                }
            }
            yield return null;
        }
    }

    void OnMiniGameFinished(MayorMiniGame _, bool playerWon)
    {
        if (playerWon) {
            mayorAnimator.SetTrigger("MayorDeath");
        }
    }

    private void PushPlayerBackRepositionGuards(Player player)
    {
        pushingPlayer += 3;

        var playerPos2D = player.transform.position.ToXZ();
        var myPos2D = this.transform.position.ToXZ();
        var pushVector = 2 * (playerPos2D - myPos2D).ToX0Z().normalized;
        var midpoint = ((playerPos2D - myPos2D) * 0.5f + myPos2D).ToX0Z();
        var perp = (playerPos2D - myPos2D).Perpendicular().normalized.ToX0Z();
        var newGuardForward = (playerPos2D - myPos2D).normalized.ToX0Z();

        float alternatingSign = 1f;
        foreach (var guard in guards) {
            guard.collider.enabled = false;
            guard.transform.forward = newGuardForward;
            guard.transform.positionTo(0.65f, midpoint + (perp * 1f * alternatingSign)).setOnCompleteHandler(OnPushPlayerTweenActionComplete);
            alternatingSign *= -1f;
        }

        var playerTweenConfig = new GoTweenConfig()
            .position(player.transform.position + pushVector)
            .vector3Prop("forward", -newGuardForward)
            .onComplete(OnPushPlayerTweenActionComplete);
        Go.to(player.transform, 0.65f, playerTweenConfig);
    }

    private void OnPushPlayerTweenActionComplete(AbstractGoTween _)
    {
        pushingPlayer--;
        if (pushingPlayer < 1) {
            pushingPlayer = 0;
            foreach (var guard in guards) {
                guard.collider.enabled = true;
            }
        }
    }

    private bool CheckWithGuards()
    {
        bool okToTalkToPLayer = true;
        foreach (var guard in guards) {
            okToTalkToPLayer &= guard.CurrentAttitude == PartyGoerAttitude.Happy;
        }
        return okToTalkToPLayer;
    }

    void OnPlayerActivate(Player player)
    {
        if (CheckWithGuards()) {
            MayorMiniGame.Instance.BeginGame();
        }
    }
}