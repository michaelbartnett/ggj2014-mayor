using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MaskSelectorUI : MonoBehaviour
{
    public MaskUIElement maskUIElementPrefab;
    public Transform firstPositionLocator;
    public Vector3 subsequentOffsets;

    private Dictionary<string, MaskUIElement> uiMasks = new Dictionary<string, MaskUIElement>();
    private Vector3 lastOffset;
    private MaskUIElement selectedMask = null;

    public static MaskSelectorUI Instance { get; private set; }

    void Awake()
    {
        lastOffset = firstPositionLocator.position - subsequentOffsets;
        Instance = this;
    }

    public void AddMask(string maskName, Sprite maskSprite)
    {
        lastOffset += subsequentOffsets;
        var elementInstance = Instantiate(maskUIElementPrefab, lastOffset, Quaternion.identity) as MaskUIElement;
        elementInstance.spriteRenderer.sprite = maskSprite;
        uiMasks.Add(maskName, elementInstance);
    }

    public void SelectMask(string maskName)
    {
        if (selectedMask != null) {
            selectedMask.Deactivate();
        }
        if (maskName != null && uiMasks.TryGetValue(maskName, out selectedMask)) {
            selectedMask.Activate();
        } else {
            selectedMask = null;
        }
    }
}
