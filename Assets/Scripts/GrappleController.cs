using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerMovement))]
public class GrappleController : MonoBehaviour
{
    public float grappleRange = 10f;
    public LineRenderer lineRenderer;
    private Rigidbody2D rb;
    private PlayerMovement playerMovement;
    private bool isDashing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (isDashing) return;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, grappleRange);
        Collider2D closestCollider = null;
        float closestDistance = Mathf.Infinity;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.layer == LayerMask.NameToLayer("Grappable"))
            {
                float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestCollider = hitCollider;
                }
            }
        }

        if (closestCollider != null)
        {
            if (Input.GetKey(KeyCode.W))
            {
                playerMovement.playerState = PlayerMovement.PlayerState.Grappling;
                float step = 10f * Time.deltaTime;
                transform.position = Vector2.Lerp(transform.position, closestCollider.transform.position, step);
                rb.gravityScale = 0;

                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, closestCollider.transform.position);
                lineRenderer.enabled = true;
                rb.velocity = Vector2.zero;
                playerMovement.rb.velocity = Vector2.zero;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartCoroutine(DashUpwards());
                }
            }
            else
            {
                lineRenderer.enabled = false;
                rb.gravityScale = 5;
                playerMovement.playerState = PlayerMovement.PlayerState.Normal;
            }
        }
    }

    private IEnumerator DashUpwards()
    {
        isDashing = true;
        playerMovement.playerState = PlayerMovement.PlayerState.Normal;
        rb.AddForce(new Vector2(0, playerMovement.jumpForce * 10), ForceMode2D.Impulse);
        lineRenderer.enabled = false;
        rb.gravityScale = 5;

        yield return new WaitForSeconds(0.5f);

        isDashing = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, grappleRange);
    }
}
