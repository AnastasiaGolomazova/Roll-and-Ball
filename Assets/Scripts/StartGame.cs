using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;

    [SerializeField] private int _count;

    public void ToText()
    {
        if (_count <= 20)
        {
            _count = int.Parse(_inputField.text);
        }
        else
        {
            _count = 20;
        }

        PlayerPrefs.SetInt("Count", _count);
    }

    public void GoGame()
    {
        SceneManager.LoadScene("Game");
    }
}
