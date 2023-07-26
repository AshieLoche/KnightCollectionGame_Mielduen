using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesController : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Input.GetKey(KeyCode.E))
            anim.SetBool("Triggered", true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetBool("Triggered", false);
    }
}
