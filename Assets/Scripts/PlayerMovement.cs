using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement playerMovement { get; private set; }

    [Header("Player")]
    public int speed;
    public int jumpForce;
    public int dashForce;

    [Header("References")]
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsWall;
    [HideInInspector] public Rigidbody2D rb;
    [SerializeField] Transform weaponHolder;
    LineRenderer lineRenderer;

    [Header("Debug")]
    public PlayerState playerState = PlayerState.Normal;

    public enum PlayerState
    {
        Normal,
        Grappling,
        Dashing,
        Sprinting
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        HandlePlayerInput();

        if (transform.position.y < -10f)
        {
            ResetPlayerPosition();
        }
    }

    private void HandlePlayerInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        Vector2 movement = new Vector2(horizontalInput * speed * 10f, rb.velocity.y);

        if (playerState == PlayerState.Normal)
        {
            if (!IsWallColliding() || IsGrounded())
            {
                rb.velocity = movement;
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        if (playerState == PlayerState.Grappling)
        {
            foreach (Transform child in weaponHolder)
            {
                child.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (Transform child in weaponHolder)
            {
                child.gameObject.SetActive(true);
            }
        }

        if (!IsGrounded() && Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.AddForce(new Vector2(horizontalInput * dashForce * 10f, 0), ForceMode2D.Impulse);
        }

        if (horizontalInput < 0 && playerState == PlayerState.Normal)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (horizontalInput > 0 && playerState == PlayerState.Normal)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.AddForce(new Vector2(0, jumpForce * 10), ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, 1f, whatIsGround);
    }

    private bool IsWallColliding()
    {
        return Physics2D.Raycast(transform.position, Vector2.right, 0.5f, whatIsWall) ||
               Physics2D.Raycast(transform.position, Vector2.left, 0.5f, whatIsWall);
    }

    private void ResetPlayerPosition()
    {
        transform.position = new Vector2(0, 0);
        rb.velocity = Vector2.zero;
    }
}
