using UnityEngine;

public class WallManager : MonoBehaviour
{
    public static WallManager Instance;
    public GameObject redWalls;
    public GameObject blueWalls;
    private bool _swapped = false;

    void Awake() => Instance = this;

    public void SwapWalls()
    {
        _swapped = !_swapped;
        SetWalls(redWalls, _swapped);
        SetWalls(blueWalls, !_swapped);
    }

    void SetWalls(GameObject walls, bool active)
    {
        walls.SetActive(active);
    }
}