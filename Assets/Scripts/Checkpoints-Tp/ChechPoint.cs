using UnityEngine;

public class WhitePlatform : MonoBehaviour
{
    public WhitePlatform linkedPlatform;
    public Transform spawnPoint; // hijo vacío sobre la plataforma
    private bool _justArrived = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || _justArrived) return;

        // Guardar checkpoint
        GameManager.Instance.SetCheckpoint(linkedPlatform.spawnPoint);

        // Teletransportar al destino
        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        other.transform.position = linkedPlatform.spawnPoint.position;
        if (cc != null) cc.enabled = true;

        linkedPlatform._justArrived = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) _justArrived = false;
    }
}