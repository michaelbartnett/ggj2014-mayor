using UnityEngine;
using System.Collections;

public class KnifeScript : MonoBehaviour
{
    public static KnifeScript Instance { get; private set; }

    public Animator animator;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) {
            Raise();
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            Attack();
        }
    }

    public void Raise()
    {
        animator.SetTrigger("KnifeRaise");
    }

    public void Attack()
    {
        animator.SetTrigger("KnifeAttack");
    }
}
