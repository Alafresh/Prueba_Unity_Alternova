using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        updateScoreBtn.onClick.AddListener(GetLeaderBoard);
    }
    public void GetLeaderBoard() {
        FirebaseDatabase.DefaultInstance
            .GetReference("users").OrderByChild("score").LimitToLast(10)
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    GameManager.Instance.SetLeaderBoard((Dictionary<string, object>)snapshot.Value);
                } 
            });
    }
    public void RestartScene() => GameManager.Instance.RestartScene();
}
