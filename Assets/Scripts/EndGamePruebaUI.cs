using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class EndGamePruebaUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI clicks;
    [SerializeField] private TextMeshProUGUI pairs;
    void Start()
    {
        scoreText.text = GameManager.Instance.CalculateScore().ToString();
        time.text = GameManager.Instance.GetTotalTime().ToString();
        clicks.text = GameManager.Instance.GetTotalClicks().ToString();
        pairs.text = GameManager.Instance.GetTotalPairs().ToString();
    }
    public void RestartScene()
    {
        GameManager.Instance.RestartScene();
    }
}
