using UnityEngine;
 
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower = 10f;

    [Header("Pro Feel Settings")]
    [SerializeField] private float coyoteTime = 0.2f;    // Time to jump after falling off ledge
    [SerializeField] private float jumpBufferTime = 0.2f; // Time to queue a jump before landing
    private Rigidbody2D body;
    private Animator anim;

    private float coyoteCounter;//khele dekho
    private float jumpBufferCounter;//ekhon koto age space dile jump hobe

    private bool grounded;
    AudioManagerL2 audioManager;
    private void Awake()
{   
    body = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    
    // Safer way to find Audio:
    audioManager = FindObjectOfType<AudioManagerL2>();
    
    if (audioManager == null) 
        Debug.LogWarning("Warning: No AudioManagerL2 found in the scene. Sound won't play.");
}
 
    private void Update()


    {   
        Debug.Log("Trying to move: " + Input.GetAxis("Horizontal"));
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        //Flip player when facing left/right.
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }

        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
 
        // if (Input.GetKey(KeyCode.Space) && grounded)
        //     Jump();
 
        //sets animation parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
        // 3. COYOTE TIME (The "Fairness" Timer)
        if (grounded)
        {
            coyoteCounter = coyoteTime; // Reset timer while on ground
        }
        else
        {
            coyoteCounter -= Time.deltaTime; // Count down when in air
        }

        if (Input.GetButtonDown("Jump")) // Better than GetKey for jumping
        {
            jumpBufferCounter = jumpBufferTime;
            audioManager.PlaySFX(audioManager.jump);
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        if (jumpBufferCounter > 0f && coyoteCounter > 0f)
        {
            Jump();
        }
    }
 
    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpPower);
        anim.SetTrigger("jump");
        grounded = false;
        coyoteCounter = 0f;
        jumpBufferCounter = 0f;
    }
 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            grounded = true;

    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            grounded = false;
    }
}