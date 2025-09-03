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

    private void Awake()
    {
        foreach (var item in itemList)
        { 
            item.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        AuthHandler.Instance.LeaderBoard();
        AuthHandler.Instance.SetUp += AuthHandler_SetUp;

    }
    private void AuthHandler_SetUp(object sender, UsersResponse e) {
        
        if (playersResults == null)
            playersResults = new PlayersResults();
        
        int n = e.usuarios.Length;

        if(playersResults.players == null || playersResults.players.Length != n) {
            playersResults.players = new PlayerInfo[n];
            for (int k = 0; k < n; k++)
                playersResults.players[k] = new PlayerInfo(default, default);
        }

        for (int i = 0; i < e.usuarios.Length; i++) {
            playersResults.players[i].name = e.usuarios[i].username;
            playersResults.players[i].score = e.usuarios[i].data.score;
        }

        if (playersResults.players != null && playersResults != null) {
            playersInfoList.AddRange(playersResults.players);
        }
        SortScores();
    }

    private void ReadPlayerResults()
    {
        UsersResponse users = AuthHandler.Instance.GetUsers();

        for (int i = 0; i < users.usuarios.Length; i++) {
            playersResults.players[i].name = users.usuarios[i].username;
            playersResults.players[i].score = users.usuarios[i].data.score;
        }

        if (playersResults.players != null && playersResults != null)
        {
            playersInfoList.AddRange(playersResults.players);
        }
    }
    private void SetUpRanking()
    {
        for(int i = 0; i < playersInfoList.Count; i++)
        {
            if(i > itemList.Count)
            {
                break;
            }
            itemList[i].Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = playersInfoList[i].name;
            itemList[i].Find("ScoreTitle").GetComponent<TMPro.TextMeshProUGUI>().text = playersInfoList[i].score.ToString();
            itemList[i].gameObject.SetActive(true);
        }
    }
    private void SortScores()
    {
        playersInfoList.Sort((x, y) => y.score.CompareTo(x.score));
        SetUpRanking();
    }
    public void RestartScene()
    {
        GameManager.Instance.RestartScene();
    }
}
