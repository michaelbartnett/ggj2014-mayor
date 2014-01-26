using UnityEngine;
using System.Collections;

public class OrientedBillboarder : MonoBehaviour
{
    [SerializeField] private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private SpriteRenderer childSprite;

    //public bool FacingBackward { get; private set; }
    
    void Awake()
    {

    }

    void Update()
    {
        var camTransform = Camera.main.transform;
        float camDot = Vector2.Dot(this.transform.forward.ToXZ(), camTransform.position.ToXZ() - this.transform.position.ToXZ());
        //if (camDot > 0 && FacingBackward) {
        if (camDot > 0) {
            childSprite.sprite = frontSprite;
            //FacingBackward = false;
        //} else if (!FacingBackward) {
        } else {
            //FacingBackward = true;
            childSprite.sprite = backSprite;
        }
    }
}
