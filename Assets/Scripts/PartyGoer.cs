using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public enum PartyGoerAttitude : int
{
    Unhappy = 0,
    Ambivalent,
    Happy,
}

public enum NeedType
{
    TalkToPerson,
    MakePersonHappy,
    WearMask,
}

[Serializable]
public class NeedParams
{
    public PartyGoer person;
    public string mask;
}

[Serializable]
public class PartyGoerNeed
{
    public NeedType needType;
    public NeedParams needParams;

    public bool Evaluate(Player player)
    {
        switch (needType) {
            case NeedType.TalkToPerson:
                return EvalTalkToPerson();
            case NeedType.MakePersonHappy:
                return needParams.person.CurrentAttitude == PartyGoerAttitude.Happy;
            case NeedType.WearMask:
                return player != null && player.WornMask == needParams.mask;
        }

        return false;
    }

    private bool EvalTalkToPerson()
    {
        var person = needParams.person;
        return person.HasTalkedWithPlayer;
    }
}

[Serializable]
public class PartyGoerGift
{
    public string mask;
}

[Serializable]
public class PartyGoerAttitudeMeta
{
    public string name;
    public List<PartyGoerNeed> needs;
    public List<PartyGoerGift> gives;
}

[Serializable]
public class AttitudeDialogue
{
    public string name;
    public List<string> dialogueItems;

    private int prevDialogueIndex = -1;

    public string GetDialogueItem()
    {
        if (dialogueItems.Count < 1) {
            return "[NO DIALOGUE SPECIFIED in PartyGoer.AttitudeDialogue]";
        }
        prevDialogueIndex = (prevDialogueIndex + 1) % dialogueItems.Count;
        return dialogueItems[prevDialogueIndex];
    }
}

public class PartyGoer : MonoBehaviour
{
    public PartyGoerAttitude CurrentAttitude { get; private set; }
    public bool HasTalkedWithPlayer { get; private set; }
    public List<PartyGoerAttitudeMeta> attitudesMetadata;
    public List<AttitudeDialogue> attitudesDialogue;

    void Awake()
    {
        CurrentAttitude = PartyGoerAttitude.Unhappy;
        while (EvaluateCurrentNeeds(null)) {
            IncrementAttitudeValue();
        }
    }

    void OnPlayerActivate(Player player)
    {
        if (EvaluateCurrentNeeds(player)) {
            GivePlayerGift(player);
            IncrementAttitudeValue();
        }
        switch (CurrentAttitude) {
            case PartyGoerAttitude.Unhappy:
                Debug.Log(this.name + " is unhappy and not talking to you");
                break;
            case PartyGoerAttitude.Ambivalent:
                Debug.Log(this.name + " is ambivalent and talking to you");
                HasTalkedWithPlayer = true;
                break;
            case PartyGoerAttitude.Happy:
                Debug.Log(this.name + " is happy and talking to you");
                break;
        }

        ShowDialogueForCurrentAttitude();
    }

    private void ShowDialogueForCurrentAttitude()
    {
        int attitudeIndex = (int)CurrentAttitude;
        string dialogueItem = "NO DIALOGUE SPECIFIED FOR CURRENT ATTITUDE (" + CurrentAttitude.ToString() + ")";
        if (attitudeIndex < attitudesDialogue.Count) {
            dialogueItem = attitudesDialogue[attitudeIndex].GetDialogueItem();
        }
        Debug.Log("DIALOGUE: " + dialogueItem);
        DialogueDisplay.Instance.ShowDialogue(this.name, dialogueItem);
    }

    private void GivePlayerGift(Player player)
    {
        var attitudeMeta = attitudesMetadata[(int)CurrentAttitude];
        var givenGifts = new List<string>();
        foreach (var gift in attitudeMeta.gives) {
            if (!string.IsNullOrEmpty(gift.mask)) {
                player.maskInventory.Add(gift.mask);
                givenGifts.Add(gift.mask);
            }
        }
        if (givenGifts.Count > 0) {
            Debug.Log(this.name + " has given you: " + string.Join(", ", givenGifts.ToArray()));
        }
    }

    private bool EvaluateCurrentNeeds(Player player)
    {
        var finalAttitudeValue = GetFinalAttitudeValue();
        if (CurrentAttitude == finalAttitudeValue) return false;
        var attitudeMeta = attitudesMetadata[(int)CurrentAttitude];
        foreach (var need in attitudeMeta.needs) {
            if (!need.Evaluate(player)) {
                return false;
            }
        }
        return true;
    }

    private PartyGoerAttitude GetFinalAttitudeValue()
    {
        int lastInEnum = (Enum.GetValues(typeof(PartyGoerAttitude)) as int[]).Last();
        int lastInMetaList = attitudesMetadata.Count - 1;
        return (PartyGoerAttitude)Math.Min(lastInEnum, lastInMetaList + 1);
    }

    private void IncrementAttitudeValue()
    {
        var finalAttitudeValue = GetFinalAttitudeValue();
        this.CurrentAttitude = (PartyGoerAttitude)(Math.Min((int)GetFinalAttitudeValue(), (int)this.CurrentAttitude + 1));
    }
}
