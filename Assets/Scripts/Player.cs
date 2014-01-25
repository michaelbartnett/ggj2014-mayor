using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public Camera fpCamera;
    private PartyGoer lookingAt;
    public List<string> maskInventory;
    public string WornMask { get; private set; }

    void Awake()
    {
        WornMask = string.Empty;
    }

    public void GiveMask(string mask)
    {
        if (!maskInventory.Contains(mask)) {
            maskInventory.Add(mask);
        }
    }

    public bool HasMask(string mask)
    {
        return maskInventory.Contains(mask);
    }

    void Update()
    {
        lookingAt = null;
        var mouseHitRay = fpCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hitInfo;
        if (Physics.Raycast(mouseHitRay, out hitInfo, float.MaxValue)) {
            var partyGoer = hitInfo.collider.GetComponent<PartyGoer>();
            if (partyGoer != null) {
                lookingAt = partyGoer;
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (DialogueDisplay.Instance.Visible) {
                DialogueDisplay.Instance.HideDialogue();
            } else if (lookingAt != null) {
                lookingAt.SendMessage("OnPlayerActivate", this);
            }
        }

        for (int i = 0; i < maskInventory.Count; i++) {
            string keyAsString = (i + 1).ToString();
            if (Input.GetKeyDown(keyAsString)) {
                if (maskInventory.Count < 1) {
                    Debug.Log("You have no masks");
                } else {
                    WearMask(i);
                    break;
                }
            }
        }
    }

    private void WearMask(int maskIndex)
    {
        if (maskIndex >= 0 && maskIndex < maskInventory.Count) {
            WornMask = maskInventory[maskIndex];
            Debug.Log("You are not wearing " + WornMask);
        }
    }

    void OnGUI()
    {
        GUILayout.Label("LOOKING AT: " + (lookingAt == null ? "Nobody" : lookingAt.name));
    }
}
