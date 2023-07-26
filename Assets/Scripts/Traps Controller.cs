using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TrapsController : MonoBehaviour
{
    [SerializeField]
    private Player1Controller Player1;
    [SerializeField]
    private Player2Controller Player2;
    private Animator anim;
    [SerializeField]
    private bool onTop;
    [SerializeField]
    private int trapDamage;
    [SerializeField]
    private Vector2 knockBack_Direction;
    [SerializeField]
    private Vector2 knockBack_Force;
    private bool isTriggered = false;
    private BoxCollider2D bc2D;

    private void Start()
    {
        bc2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player 1"))
        {
            StartCoroutine(KnockBack_Player1());
            anim.SetBool("Triggered", true);
            onTop = true;
        }
        
        if (collision.gameObject.CompareTag("Player 2"))
        {
            StartCoroutine(KnockBack_Player2());
            anim.SetBool("Triggered", true);
            onTop = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player 1") || collision.gameObject.CompareTag("Player 2"))
        {
            anim.SetBool("Triggered", false);
            onTop = false;
        }
    }

    IEnumerator KnockBack_Player1()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Vector2 knockback = knockBack_Direction * knockBack_Force;
        Player1.rb2D.AddForce(knockback, ForceMode2D.Impulse);
    }

    IEnumerator KnockBack_Player2()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Vector2 knockback = knockBack_Direction * knockBack_Force;
        Player2.rb2D.AddForce(knockback, ForceMode2D.Impulse);
    }

    private void PlayerDamage()
    {
        if (onTop)
        {
            Player1.hp -= trapDamage;
            Player2.hp -= trapDamage;
        }
    }
}