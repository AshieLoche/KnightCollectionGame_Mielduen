using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour
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
        if (!horizontalCheck && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            if (Input.GetKey(KeyCode.UpArrow)) 
                movementInput.y = 1f;
            else if (Input.GetKey(KeyCode.DownArrow))
                movementInput.y = -1f;

            movementInput.x = 0f;
            verticalCheck = true;
            horizontalCheck = false;
        }
        else if (!verticalCheck && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                movementInput.x = -1f;
            else if (Input.GetKey(KeyCode.RightArrow))
                movementInput.x = 1f;

            movementInput.y = 0;
            horizontalCheck = true;
            verticalCheck = false;
        }
        else if (horizontalCheck || verticalCheck)
        {
            movementInput.x = 0f;
            movementInput.y = 0f;
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

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                anim.SetBool("Forward_Idle", true);
                anim.SetBool("Backward_Idle", false);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                anim.SetBool("Forward_Idle", false);
                anim.SetBool("Backward_Idle", true);
            }

            anim.SetBool("Left_Idle", false);
            anim.SetBool("Right_Idle", false);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                anim.SetBool("Left_Idle", true);
                anim.SetBool("Right_Idle", false);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
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
