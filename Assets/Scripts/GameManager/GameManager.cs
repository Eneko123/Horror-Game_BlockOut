using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private Transform _currentCheckpoint;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void SetCheckpoint(Transform point)
    {
        _currentCheckpoint = point;
        Debug.Log($"Checkpoint: {point.name}");
    }

    public void RespawnPlayer(GameObject player)
    {
        if (_currentCheckpoint == null) return;

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc) cc.enabled = false;
        player.transform.position = _currentCheckpoint.position;
        if (cc) cc.enabled = true;
    }
}