using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    [Header("Configuraci�n")]
    [SerializeField] float sensibilityX = 0.1f;
    [SerializeField] float sensibilityY = 0.1f;

    [SerializeField] Transform player;
    [SerializeField] float maxY = 360f;
    [SerializeField] float minY = -360f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Leer movimiento del rat�n
        float mouseX = Input.GetAxis("Mouse X") * sensibilityX;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilityY;

        // Acumular rotaci�n
        rotationX += mouseX;
        rotationY += mouseY;

        // Limitar rotacion vertical (mirar arriba/abajo)
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        // Aplicar rotaciones
        player.transform.localEulerAngles = new Vector3(0f, rotationX, 0f);
        transform.localEulerAngles = new Vector3(-rotationY, 0f, 0f);
    }
}