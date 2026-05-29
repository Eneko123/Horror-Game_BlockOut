using UnityEngine;

public class WallTrigger : MonoBehaviour
{
    public bool activateOnEnter = true;

    void OnTriggerEnter(Collider other)
    {
        if (!activateOnEnter) return;
        if (other.CompareTag("Player"))
            WallManager.Instance.SwapWalls();
    }

    void Update()
    {
        // Alternativa: activar con tecla E
        if (Input.GetKeyDown(KeyCode.E))
            WallManager.Instance.SwapWalls();
    }
}