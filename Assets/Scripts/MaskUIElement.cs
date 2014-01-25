using UnityEngine;
using System.Collections;

public class MaskUIElement : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public TextMesh textMesh;

    public void Set(string text, Sprite sprite)
    {
        this.textMesh.text = text;
        this.spriteRenderer.sprite = sprite;
    }
}
