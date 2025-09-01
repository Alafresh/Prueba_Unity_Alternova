using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class EndGamePruebaUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private TextMeshProUGUI clicks;
    [SerializeField] private TextMeshProUGUI pairs;
    [SerializeField] private Button updateScoreBtn;    
    void Start()
    {
        scoreText.text = GameManager.Instance.CalculateScore().ToString();
        time.text = GameManager.Instance.GetTotalTime().ToString();
        clicks.text = GameManager.Instance.GetTotalClicks().ToString();
        pairs.text = GameManager.Instance.GetTotalPairs().ToString();
        updateScoreBtn.onClick.AddListener(UpdateScore);
    }
    public void UpdateScore() => AuthHandler.Instance.UpdateScore(GameManager.Instance.CalculateScore());
    public void RestartScene() => GameManager.Instance.RestartScene();
}
