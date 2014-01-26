using UnityEngine;
using System.Collections;

public class MaskUIElement : MonoBehaviour
{
    private const float deactivatedAlpha = 0.35f;
    public SpriteRenderer spriteRenderer;

    public bool Activated { get; private set; }

    void Start()
    {
        Deactivate();
    }

    public void Activate()
    {
        var color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;
        Activated = true;
    }

    public void Deactivate()
    {
        var color = spriteRenderer.color;
        color.a = deactivatedAlpha;
        spriteRenderer.color = color;
        Activated = false;
    }
}
