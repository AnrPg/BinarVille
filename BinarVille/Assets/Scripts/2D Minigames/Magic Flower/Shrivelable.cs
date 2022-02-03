using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrivelable : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void isShrivelEnabled(bool commence)
    {
        animator.SetBool("shrivel", commence);
    }
}
