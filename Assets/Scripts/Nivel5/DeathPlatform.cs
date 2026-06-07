using System.Collections;
using UnityEngine;

/// <summary>
/// Coloca este script en cada plataforma NEGRA (zona de muerte).
/// Al tocarla, el jugador reaparece en el último checkpoint.
/// Usa SOLO Trigger o SOLO Collider en el GameObject, no ambos a la vez.
/// </summary>
public class DeathPlatform : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Tiempo en segundos antes de que el jugador reaparezca")]
    public float respawnDelay = 0.5f;

    [Tooltip("Efecto de muerte a instanciar (opcional)")]
    public GameObject deathEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        TryKillPlayer(other.GetComponent<PlayerController>());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        TryKillPlayer(collision.gameObject.GetComponent<PlayerController>());
    }

    private void TryKillPlayer(PlayerController player)
    {
        if (player == null) return;

        // Guard SÍNCRONO: si ya está muerto o teleportando, ignorar
        if (player.IsDead || player.IsTeleporting) return;

        StartCoroutine(RespawnSequence(player));
    }

    private IEnumerator RespawnSequence(PlayerController player)
    {
        player.Die();

        if (deathEffect != null)
            Instantiate(deathEffect, player.transform.position, Quaternion.identity);

        Debug.Log("[Death] Jugador murió. Respawn en: " +
                  CheckpointManager.Instance.CurrentCheckpoint?.checkpointID);

        yield return new WaitForSeconds(respawnDelay);

        Vector3 respawnPos = CheckpointManager.Instance.GetRespawnPosition();
        player.TeleportTo(respawnPos);
        player.Revive();
    }
}
