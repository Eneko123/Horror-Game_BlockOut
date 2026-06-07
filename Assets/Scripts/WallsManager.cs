using UnityEngine;

public class WallsManager : MonoBehaviour
{
    public GameObject redWalls;
    public GameObject blueWalls;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
            redWalls.SetActive(false);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
                redWalls.SetActive(true);
            

                blueWalls.SetActive(false);

            gameObject.SetActive(false);
        }
    }
}
