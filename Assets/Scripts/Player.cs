using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public Camera fpCamera;
    private GameObject lookingAt;
    private List<string> maskInventory = new List<string>();
    public string WornMask { get; private set; }
    private int wornMaskIndex = 0;


    void Awake()
    {
        WornMask = string.Empty;
        maskInventory.Add(WornMask);
    }

    void Start()
    {
        MayorMiniGame.Instance.MiniGameBegan += OnMiniGameBegan;
    }

    private void OnMiniGameBegan(MayorMiniGame _)
    {
        _.MiniGameBegan -= OnMiniGameBegan;
        // hold up your knife
    }

    public void GiveMask(string maskName, Sprite maskSprite)
    {
        if (!maskInventory.Contains(maskName)) {
            maskInventory.Add(maskName);
            MaskSelectorUI.Instance.AddMask(maskName, maskSprite);
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
        if (Physics.Raycast(mouseHitRay, out hitInfo, float.MaxValue, 1 << LayerMask.NameToLayer("PlayerActivatable"))) {
            lookingAt = hitInfo.collider.gameObject;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            if (DialogueDisplay.Instance.Visible) {
                DialogueDisplay.Instance.HideDialogue();
            } else if (lookingAt != null) {
                lookingAt.SendMessage("OnPlayerActivate", this);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            wornMaskIndex = (wornMaskIndex + 1) % maskInventory.Count;
            WornMask = maskInventory[wornMaskIndex];
            MaskSelectorUI.Instance.SelectMask(WornMask);
        }

        if (Input.GetKeyDown(KeyCode.F) && MayorMiniGame.Instance.Running) {
            MayorMiniGame.Instance.PlayerAttacks();
        }
    }

    void OnGUI()
    {
        GUILayout.Label("LOOKING AT: " + (lookingAt == null ? "Nobody" : lookingAt.name));
    }
}
