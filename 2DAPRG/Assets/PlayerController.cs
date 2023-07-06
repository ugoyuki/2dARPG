using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;

    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;
    SpriteRenderer spriteRenderer;
    bool canMove = true;
    public SwordAttack swordAttack;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("aa");
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {


            if (movementInput != Vector2.zero)
            {
                Debug.Log("bbb");
                bool success = TryMove(movementInput);

                if (!success && movementInput.x > 0)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }
                if (!success && movementInput.y > 0)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
            if (movementInput.x < 0)
            {

                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {

                spriteRenderer.flipX = false;

            }
        }

    }

    private bool TryMove(Vector2 directions)
    {
        if (directions != Vector2.zero)
        {

            int count = rb.Cast(
                directions,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset
                );
            if (count == 0)
            {
                rb.MovePosition(rb.position + movementInput * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }


    void OnMove(InputValue movementValue)
    {
        Debug.Log("zz");
        // Moveアクションの入力値を取得
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        print("Fire");
        animator.SetTrigger("swordAttack");
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }

    public void SwordAttack()
    {
        LockMovement();
        if(spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }
}
