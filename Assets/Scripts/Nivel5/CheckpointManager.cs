using UnityEngine;

/// <summary>
/// Singleton que lleva la cuenta del último checkpoint activo.
/// Coloca este script en un GameObject vacío llamado "CheckpointManager" en la escena.
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    [Header("Estado actual")]
    [SerializeField, Tooltip("Solo lectura: checkpoint activo en este momento")]
    private CheckpointPlatform currentCheckpoint;

    [Header("Efectos")]
    [Tooltip("Partículas o efecto al activar un nuevo checkpoint (opcional)")]
    public GameObject activationEffect;

    private void Awake()
    {
        // Patrón Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Persiste entre escenas si lo necesitas
    }

    /// <summary>
    /// Registra una plataforma como el último checkpoint activo.
    /// </summary>
    public void SetCheckpoint(CheckpointPlatform newCheckpoint)
    {
        if (newCheckpoint == currentCheckpoint) return;

        // Desactivar el anterior visualmente
        if (currentCheckpoint != null)
            currentCheckpoint.SetVisual(false);

        // Activar el nuevo
        currentCheckpoint = newCheckpoint;
        currentCheckpoint.SetVisual(true);

        // Efecto visual opcional
        if (activationEffect != null)
        {
            Instantiate(activationEffect,
                        newCheckpoint.GetSpawnPosition(),
                        Quaternion.identity);
        }

        Debug.Log($"[Checkpoint] Activo: {currentCheckpoint.checkpointID}");
    }

    /// <summary>
    /// Devuelve la posición de respawn del último checkpoint.
    /// Si no hay ninguno registrado, devuelve Vector3.zero.
    /// </summary>
    public Vector3 GetRespawnPosition()
    {
        if (currentCheckpoint != null)
            return currentCheckpoint.GetSpawnPosition();

        Debug.LogWarning("[Checkpoint] No hay checkpoint activo. Respawn en origen.");
        return Vector3.zero;
    }

    public CheckpointPlatform CurrentCheckpoint => currentCheckpoint;
}
