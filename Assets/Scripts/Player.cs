using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public Camera fpCamera;
    public FPSInputController inputController;
    public MouseLook mouseLookX;
    public MouseLook mouseLookY;
    public CharacterMotor motor;
    public float minActivateDistance = 2f;
    private GameObject lookingAt;
    private List<string> maskInventory = new List<string>();
    public string WornMask { get; private set; }
    private int wornMaskIndex = 0;

    public static Player Instance { get; private set; }

    public bool ControlsEnabled { get; private set; }

    void Awake()
    {
        DisableControls();
        WornMask = string.Empty;
        maskInventory.Add(WornMask);
        Instance = this;
    }

    void Start()
    {
        MayorMiniGame.Instance.MiniGameInitiated += OnMiniGameInitiated;
        EnableControls();
    }

    private void OnMiniGameInitiated(MayorMiniGame _)
    {
        _.MiniGameInitiated -= OnMiniGameInitiated;
        KnifeScript.Instance.Raise();
    }

    public void EnableControls()
    {
        inputController.enabled = true;
        mouseLookX.enabled = true;
        mouseLookY.enabled = true;


        ControlsEnabled = true;
    }

    public void DisableControls()
    {
        inputController.enabled = false;
        mouseLookX.enabled = false;
        mouseLookY.enabled = false;
        motor.inputMoveDirection = Vector3.zero;

        ControlsEnabled = false;
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

        if (MayorMiniGame.Instance.Running) {
            if (Input.GetKeyDown(KeyCode.E) && MayorMiniGame.Instance.SliderRunning) {
                MayorMiniGame.Instance.PlayerAttacks();
                KnifeScript.Instance.Attack();
            }
        } else {
            if (Input.GetKeyDown(KeyCode.E)) {
                if (DialogueDisplay.Instance.Visible) {
                    DialogueDisplay.Instance.HideDialogue();
                } else if (lookingAt != null) {
                    float distToTargetSqr = (this.transform.position.ToXZ() - lookingAt.transform.position.ToXZ()).sqrMagnitude;
                    if ((distToTargetSqr) <= (minActivateDistance * minActivateDistance)) {
                        lookingAt.SendMessage("OnPlayerActivate", this);
                    } else {
                        Debug.Log("Distance to target: " + Mathf.Sqrt(distToTargetSqr));
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Q)) {
                wornMaskIndex = (wornMaskIndex + 1) % maskInventory.Count;
                WornMask = maskInventory[wornMaskIndex];
                MaskSelectorUI.Instance.SelectMask(WornMask);
            }
        }
    }

    void OnGUI()
    {
        GUILayout.Label("LOOKING AT: " + (lookingAt == null ? "Nobody" : lookingAt.name));
    }
}
