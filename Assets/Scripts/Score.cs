using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score instance;

    [SerializeField] private TMP_Text _text;

    private int _score;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance == this)
            Destroy(gameObject);

        UpdateUI();
    }

    public void AddScore()
    {
        _score++;
        UpdateUI();
    }
    public void AddScore2x()
    {
        _score += 2;
        UpdateUI();
    }

    public void RemoveScore()
    {
        _score--;
        UpdateUI();
    }

    private void UpdateUI() 
    {
        _text.text = "Score: " + _score.ToString();
    }
}