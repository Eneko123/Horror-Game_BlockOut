using UnityEngine;

/// <summary>
/// Coloca este script en cada plataforma BLANCA del mapa.
/// Actúa como checkpoint y/o como extremo de un teletransporte.
/// </summary>
public class CheckpointPlatform : MonoBehaviour
{
    [Header("Identificación")]
    [Tooltip("Nombre único para identificar este checkpoint (ej: 'Piso1_Inicio')")]
    public string checkpointID;

    [Header("Teletransporte")]
    [Tooltip("Plataforma destino a la que teletransporta al jugador. Dejar vacío si no hay TP.")]
    public CheckpointPlatform linkedPlatform;

    [Tooltip("Punto exacto donde aparece el jugador al llegar aquí por TP (opcional)")]
    public Transform spawnPoint;

    [Header("Visual Feedback")]
    [Tooltip("Material cuando el checkpoint está inactivo")]
    public Material inactiveMaterial;
    [Tooltip("Material cuando el checkpoint está activo")]
    public Material activeMaterial;

    private Renderer platformRenderer;
    private bool isActive = false;

    // Cooldown para evitar bucles de TP infinitos
    private static float tpCooldown = 1f;
    private static float lastTpTime = -999f;

    private void Awake()
    {
        platformRenderer = GetComponent<Renderer>();
        SetVisual(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null) return;

        // 1. Registrar como último checkpoint
        CheckpointManager.Instance.SetCheckpoint(this);

        // 2. Teletransportar si hay plataforma vinculada y no estamos en cooldown
        if (linkedPlatform != null && Time.time - lastTpTime > tpCooldown)
        {
            lastTpTime = Time.time;
            TeleportPlayer(player);
        }
    }

    private void TeleportPlayer(PlayerController player)
    {
        Vector3 destination = linkedPlatform.GetSpawnPosition() + new Vector3 (0, 1, 0);
        player.TeleportTo(destination);

        // Activar checkpoint destino también
        CheckpointManager.Instance.SetCheckpoint(linkedPlatform);

        Debug.Log($"[TP] {checkpointID} → {linkedPlatform.checkpointID}");
    }

    /// <summary>Devuelve la posición de spawn de esta plataforma.</summary>
    public Vector3 GetSpawnPosition()
    {
        if (spawnPoint != null)
            return spawnPoint.position;

        // Por defecto: encima de la plataforma
        return transform.position + Vector3.up * 1.5f;
    }

    /// <summary>Activa o desactiva el visual del checkpoint.</summary>
    public void SetVisual(bool active)
    {
        isActive = active;
        if (platformRenderer == null) return;

        if (active && activeMaterial != null)
            platformRenderer.material = activeMaterial;
        else if (!active && inactiveMaterial != null)
            platformRenderer.material = inactiveMaterial;
    }

    public bool IsActive => isActive;

    private void OnDrawGizmos()
    {
        // Dibuja línea hacia la plataforma vinculada en el editor
        if (linkedPlatform != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position + Vector3.up, linkedPlatform.transform.position + Vector3.up);
            Gizmos.DrawSphere(transform.position + Vector3.up, 0.15f);
        }

        // Dibuja el punto de spawn
        if (spawnPoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnPoint.position, 0.3f);
        }
    }
}
