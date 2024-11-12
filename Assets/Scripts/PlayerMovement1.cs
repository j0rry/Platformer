using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public enum PlayerState
    {
        idle,
        walking,
        running
    }

    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float moveSpeed = 5f;
    Rigidbody2D rb;

    Vector2 movement;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void HandleInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        movement = new Vector2(horizontalInput, rb.velocity.y).normalized;

        if (Input.GetButtonDown("Jump") && IsGround())
        {
            rb.AddForce(new Vector2(0, 5 * 10), ForceMode2D.Impulse);
        }
    }

    void MovePlayer()
    {
        rb.velocity = movement * moveSpeed;
    }

    bool IsGround() => (Physics2D.Raycast(transform.position, Vector2.down, 1f, whatIsGround));
}
