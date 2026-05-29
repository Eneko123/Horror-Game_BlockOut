using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Configuraci�n")]
    [SerializeField] float speed = 4f;
    [SerializeField] float jumpForce = 6f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] CameraPlayer cameraPlayer;

    private CharacterController controller;
    private float verticalVelocity = 0f;

    void Awake() => controller = GetComponent<CharacterController>();

    void Update()
    {
        // Movimiento horizontal (WASD / Flechas)
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 forward = cameraPlayer.transform.forward;
        Vector3 right = cameraPlayer.transform.right;
        forward.y = 0f; right.y = 0f;
        forward.Normalize(); right.Normalize();

        Vector3 move = (forward * v + right * h).normalized * speed;

        // Logica de salto y gravedad
        if (controller.isGrounded)
        {
            // Valor negativo pequenio para mantener al jugador "pegado" al suelo
            verticalVelocity = -1f;

            // Detectar tecla Espacio para saltar
            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpForce;
            }
        }

        // Aplicar gravedad en cada frame
        verticalVelocity += gravity * Time.deltaTime;

        // Combinar movimiento horizontal y vertical
        move.y = verticalVelocity;

        // Aplicar movimiento final
        controller.Move(move * Time.deltaTime);
    }
}