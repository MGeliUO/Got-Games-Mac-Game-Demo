using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public float jumpForce = 500f;

    private bool grounded = false;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get references to the attached rigidbody2D and animator components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        // Jump controls
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
            SetGrounded(false);
        }
    }

    private void Jump()
    {
        // Apply upwards force to the rigidbody
        rb.AddForce(jumpForce * Vector3.up);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // When we hit the ground, set grounded to true
        if (collision.gameObject.CompareTag("Ground"))
        {
            SetGrounded(true);
        }

        // When we hit an obstacle, it's game over!
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            StartCoroutine(OnHitObstacle());
        }
    }

    private void SetGrounded(bool newValue)
    {
        grounded = newValue;
        anim.SetBool("isGrounded", newValue);
    }

    private IEnumerator OnHitObstacle()
    {
        gameOver = true;

        // Disable hitbox and change the rigidbody's gravity scale to zero so Mac freezes in the air for a moment
        GetComponent<Collider2D>().enabled = false;
        rb.gravityScale = 0;
        rb.velocity = Vector2.zero;

        anim.SetBool("isDead", true);
        
        // Let the gamemanager know the game has ended
        GameManager.instance.OnGameOver();

        // Wait for 0.75f seconds
        yield return new WaitForSeconds(0.75f);

        // Launch Mac into the air and have him fall down past the floor like mario
        rb.gravityScale = 1;
        Jump();
        


    }




}
