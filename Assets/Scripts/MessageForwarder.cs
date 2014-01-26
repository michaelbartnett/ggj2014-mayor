using UnityEngine;
using System.Collections;
using System;

public class MessageForwarder : MonoBehaviour
{
    public event Action<MessageForwarder, Collider> TriggerEntered;
    public event Action<MessageForwarder, ControllerColliderHit> ControllerColliderHit;

    void OnControllerColliderHit(ControllerColliderHit hitInfo)
    {
        if (ControllerColliderHit != null) ControllerColliderHit(this, hitInfo);
    }

    void OnTrigger(Collider other)
    {
        if (TriggerEntered != null) TriggerEntered(this, other);
    }
}