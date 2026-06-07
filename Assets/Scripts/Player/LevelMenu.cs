using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMenu : MonoBehaviour
{
    [Header("Niveles")]
    [Tooltip("Nombres exactos de las escenas tal como aparecen en Build Settings")]
    public string level1Scene = "Nivel5";
    public string level2Scene = "Nivel6";
    public string level3Scene = "Nivel7";

    private bool isOpen = false;

    // Asigna este GameObject en el Inspector (el panel del menú)
    public GameObject menuPanel;

    private void Start()
    {
        menuPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SetMenu(!isOpen);
    }

    private void SetMenu(bool open)
    {
        isOpen = open;
        menuPanel.SetActive(open);
        Time.timeScale = open ? 0f : 1f;
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = open;
    }

    public void LoadLevel1() => LoadLevel(level1Scene);
    public void LoadLevel2() => LoadLevel(level2Scene);
    public void LoadLevel3() => LoadLevel(level3Scene);

    private void LoadLevel(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}
