using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class RankingUI : MonoBehaviour
{
    [SerializeField] List<Transform> itemList = new List<Transform>();
    private PlayersResults playersResults;
    private List<PlayerInfo> playersInfoList = new List<PlayerInfo>();
    [SerializeField] private ParticleSystem particleSystem;
    private Dictionary<string, object> _leaderBoardUsers = new();

    private void Awake()
    {
        foreach (var item in itemList)
        { 
            item.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        Invoke("SetUpLeaderBoard", 0.2f);
    }
    private void SetUpLeaderBoard() {
        _leaderBoardUsers = GameManager.Instance.GetLeaderBoard();

        if (_leaderBoardUsers is null) {
            Debug.Log("Nullllll");
        }
        foreach (var usuarioDoc in _leaderBoardUsers) {
            var usuario = (Dictionary<string, object>)usuarioDoc.Value;
            PlayerInfo player = new(usuario["username"].ToString(), usuario["score"].ToString());
            playersInfoList.Add(player);
        }
        SortScores();
    }

    private void SetUpRanking() {
        for (int i = 0; i < playersInfoList.Count; i++) {
            if (i > itemList.Count) {
                break;
            }
            itemList[i].Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = playersInfoList[i].name;
            itemList[i].Find("ScoreTitle").GetComponent<TMPro.TextMeshProUGUI>().text = playersInfoList[i].score.ToString();
            itemList[i].gameObject.SetActive(true);
        }
    }
    private void SortScores() {
        playersInfoList.Sort((x, y) => y.score.CompareTo(x.score));
        SetUpRanking();
    }
    public void RestartScene()
    {
        GameManager.Instance.RestartScene();
    }
}
