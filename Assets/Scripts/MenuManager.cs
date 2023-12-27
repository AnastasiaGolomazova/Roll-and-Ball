using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartRollAndBall()
    {
        SceneManager.LoadScene("Menu");
    }

    public void CatchingUp()
    {
        SceneManager.LoadScene("CatchingUp");
    }

    public void Aqurium()
    {
        SceneManager.LoadScene("Aqurium");
    }
}