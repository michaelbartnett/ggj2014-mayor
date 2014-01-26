using UnityEngine;
using System.Collections;
using System;

public class DialogueDisplay : MonoBehaviour
{
    public TextMesh speakerNameTextMesh;
    public TextMesh dialogueTextMesh;
    public Vector3 hiddenLocalPosition;
    public Vector3 visibleLocalPosition;

    public bool Visible { get; private set; }
    private bool transitioning = false;
    private Transform dialogueRoot;

    public static DialogueDisplay Instance { get; private set; }

    void Awake()
    {
        dialogueRoot = this.transform;
        Visible = false;
        dialogueRoot.localPosition = hiddenLocalPosition;
        Instance = this;
    }

    public void ChangeDialogue(string speaker, string dialogue)
    {
        if (!Visible) {
            ShowDialogue(speaker, dialogue);
        } else {
            speakerNameTextMesh.text = "<i>" + speaker + "</i>:";
            dialogueTextMesh.text = dialogue;
        }
    }

    public void ShowDialogue(string speaker, string dialogue)
    {
        if (transitioning || Visible) return;

        speakerNameTextMesh.text = "<i>" + speaker + "</i>:";
        dialogueTextMesh.text = dialogue;

        var tweenCfg = new GoTweenConfig()
            .localPosition(visibleLocalPosition)
            .setEaseType(GoEaseType.CubicIn)
            .onComplete(OnDialogueShowComplete);
        Go.to(dialogueRoot, 0.35f, tweenCfg);
        transitioning = true;
    }

    public void HideDialogue()
    {
        if (transitioning || !Visible) return;

        var tweenCfg = new GoTweenConfig()
            .localPosition(hiddenLocalPosition)
            .setEaseType(GoEaseType.CubicInOut)
            .onComplete(OnDialogueHideComplete);
        Go.to(dialogueRoot, 0.25f, tweenCfg);
        transitioning = true;
    }

    private void OnDialogueHideComplete(AbstractGoTween _)
    {
        Visible = false;
        transitioning = false;
    }

    private void OnDialogueShowComplete(AbstractGoTween _)
    {
        Visible = true;
        transitioning = false;
    }
}
