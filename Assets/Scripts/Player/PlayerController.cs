using System.Collections;
using UnityEngine;

/// <summary>
/// Script base del jugador.
/// Incluye los métodos necesarios para el sistema de checkpoint/TP/muerte.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Detección de suelo")]
    [Tooltip("Layer(s) que se consideran suelo")]
    public LayerMask groundLayers = ~0;
    [Tooltip("Distancia extra bajo los pies para detectar el suelo (0.05–0.15 suele ir bien)")]
    public float groundCheckDistance = 0.08f;

    [Header("Feel del salto")]
    [Tooltip("Segundos tras salir del suelo en que aún puedes saltar (coyote time)")]
    public float coyoteTime = 0.12f;
    [Tooltip("Segundos antes de tocar suelo en que se recuerda la pulsación de salto (jump buffer)")]
    public float jumpBufferTime = 0.15f;

    [Header("Estado")]
    [SerializeField] private bool isDead = false;

    // ─── Componentes ────────────────────────────────────────────────
    private CharacterController cc;
    private Vector3 velocity;
    private bool isTeleporting = false;

    // ─── Suelo ──────────────────────────────────────────────────────
    private bool isGrounded;
    private float coyoteTimer;
    private float jumpBufferTimer;

    // ─── Propiedades públicas ────────────────────────────────────────
    public bool IsDead => isDead;
    public bool IsTeleporting => isTeleporting;

    // ═══════════════════════════════════════════════════════════════
    // Unity Messages
    // ═══════════════════════════════════════════════════════════════

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
    }

    // ═══════════════════════════════════════════════════════════════
    // Detección de suelo
    // ═══════════════════════════════════════════════════════════════

    private bool CheckGrounded()
    {
        float radius = cc.radius * 0.9f;
        Vector3 origin = transform.position + Vector3.up * (cc.radius + 0.01f);
        float distance = cc.height * 0.5f - cc.radius + groundCheckDistance;

        return Physics.SphereCast(origin, radius, Vector3.down, out _, distance, groundLayers,
                                  QueryTriggerInteraction.Ignore);
    }

    // ═══════════════════════════════════════════════════════════════
    // Movimiento
    // ═══════════════════════════════════════════════════════════════

    private void HandleMovement()
    {
        // ── Suelo ──────────────────────────────────────────────────
        isGrounded = CheckGrounded();

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
            if (velocity.y < 0f) velocity.y = -2f;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }

        // ── Jump buffer ────────────────────────────────────────────
        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = jumpBufferTime;
        else
            jumpBufferTimer -= Time.deltaTime;

        // ── Salto ──────────────────────────────────────────────────
        if (coyoteTimer > 0f && jumpBufferTimer > 0f)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            coyoteTimer = 0f;
            jumpBufferTimer = 0f;
        }

        // ── Movimiento horizontal ──────────────────────────────────
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        cc.Move(move * moveSpeed * Time.deltaTime);

        // ── Gravedad ───────────────────────────────────────────────
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    // ═══════════════════════════════════════════════════════════════
    // API pública
    // ═══════════════════════════════════════════════════════════════

    public void TeleportTo(Vector3 position)
    {
        StartCoroutine(TeleportCoroutine(position));
    }

    private IEnumerator TeleportCoroutine(Vector3 position)
    {
        isTeleporting = true;
        cc.enabled = false;
        transform.position = position;
        velocity = Vector3.zero;

        yield return null;

        cc.enabled = true;
        isTeleporting = false;
    }

    public void Die()
    {
        isDead = true;
        velocity = Vector3.zero;
    }

    public void Revive()
    {
        isDead = false;
        velocity = Vector3.zero;
    }
}
