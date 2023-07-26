using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private bool horizontalCheck, verticalCheck;
    private Vector2 movementInput;
    public Rigidbody2D rb2D;
    private Animator anim;
    [SerializeField]
    private int coinCounter = 0;
    public int hp;
    private bool movement = true;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        SetMovement();
        SetAnimation();
        AttackAnimation();
    }

    private void FixedUpdate()
    {
        if (movement)
            rb2D.AddForce(moveSpeed * Time.fixedDeltaTime * movementInput);
    }

    private void SetMovement()
    {
        if (Input.GetAxisRaw("Horizontal") != 0f && !verticalCheck && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = 0;
            horizontalCheck = true;
            verticalCheck = false;
        }
        else if (Input.GetAxisRaw("Vertical") != 0f && !horizontalCheck && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
        {
            movementInput.x = 0;
            movementInput.y = Input.GetAxisRaw("Vertical");
            verticalCheck = true;
            horizontalCheck = false;
        }
        else if (Input.GetAxisRaw("Horizontal") == 0f || Input.GetAxisRaw("Vertical") == 0f)
        {
            movementInput.x = 0;
            movementInput.y = 0;
            horizontalCheck = false;
            verticalCheck = false;
        }
    }

    private void SetAnimation()
    {
        if (movement)
        {
            anim.SetFloat("Horizontal", movementInput.x);
            anim.SetFloat("Vertical", movementInput.y);
            anim.SetFloat("Speed", movementInput.sqrMagnitude);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.W))
            {
                anim.SetBool("Forward_Idle", true);
                anim.SetBool("Backward_Idle", false);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                anim.SetBool("Forward_Idle", false);
                anim.SetBool("Backward_Idle", true);
            }

            anim.SetBool("Left_Idle", false);
            anim.SetBool("Right_Idle", false);
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool("Left_Idle", true);
                anim.SetBool("Right_Idle", false);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool("Left_Idle", false);
                anim.SetBool("Right_Idle", true);
            }

            anim.SetBool("Forward_Idle", false);
            anim.SetBool("Backward_Idle", false);
        }
    }

    private void AttackAnimation()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            anim.SetBool("Attack", true);
        else if (Input.GetKeyUp(KeyCode.Space))
            anim.SetBool("Attack", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coins"))
        {
            coinCounter++;
            Destroy(collision.gameObject);
        }
        //else if (collision.gameObject.CompareTag("HealthPotion"))
        //{
        //    Destroy(collision.gameObject);
        //}
        //else if (collision.gameObject.CompareTag("SpeedPotion"))
        //{
        //    Transform col = collision.transform;
        //    col.transform.position = new Vector2(999, 999);
        //}
        else if (collision.gameObject.CompareTag("Lava"))
        {
            hp -= 5;
        }
        else if (collision.gameObject.CompareTag("Side Push Trap"))
            movement = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Side Push Trap"))
            movement = true;
    }
}