using UnityEngine;

public class CarZone : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerController player;

    [Header("Velocidad")]
    public float boostedSpeed = 20f;

    private float originalSpeed;
    private float originalJump;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        originalSpeed = player.moveSpeed;
        originalJump = player.jumpHeight;

        player.moveSpeed = boostedSpeed;
        player.jumpHeight = 0f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        player.moveSpeed = originalSpeed;
        player.jumpHeight = originalJump;
    }
}
