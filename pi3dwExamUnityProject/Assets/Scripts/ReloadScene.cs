using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void RestartScene()
    {
        // Reload the current scene 
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
