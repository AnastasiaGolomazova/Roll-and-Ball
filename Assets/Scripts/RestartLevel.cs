using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public void RestartLevelClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}