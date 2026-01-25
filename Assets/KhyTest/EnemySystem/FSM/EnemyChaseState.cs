using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : MonoBehaviour
{
    Animator animator;

    void Update()
    {
        animator.SetBool("isMove", true);
    }
}
