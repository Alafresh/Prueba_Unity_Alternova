using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ricimi;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Threading.Tasks;

[System.Serializable]
public class PlayerInfo
{
    public string name;
    public int score;

    public PlayerInfo(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
}

[System.Serializable]
public class PlayersResults
{
    public PlayerInfo[] players;
}

public class Ranking : MonoBehaviour
{

    [SerializeField] private TMP_InputField userName;
    [SerializeField] private PopupOpener popupOpener;
    [SerializeField] private Popup popup;
    [SerializeField] private GameObject warningPanel;
    private PlayersResults playersResults;
    private List<PlayerInfo> playersInfoList = new List<PlayerInfo>();

    public void Submit()
    {
        string name = userName.text;
        if(name == "")
        {
            warningPanel.SetActive(true);
            return;
        }
        int score = GameManager.Instance.CalculateScore();
        PlayerInfo playerInfo = new PlayerInfo(name, score);
        playersInfoList.Add(playerInfo);
        CheckResultsToJson();
    }

    private void CheckResultsToJson()
    {
        string pathPlayersResults = "PlayersResults.json";
        string existingJson = SaveSystem.Load(pathPlayersResults);

        if (string.IsNullOrEmpty(existingJson))
        {
            SaveSystem.CreatePlayersResults(playersResults, playersInfoList);
            return;
        }

        playersResults = JsonUtility.FromJson<PlayersResults>(existingJson);

        if (playersResults.players != null && playersResults != null)
        {
            playersInfoList.AddRange(playersResults.players);
        }
        SaveResultsToJson();
    }

    private void SaveResultsToJson()
    {
        playersResults = SaveSystem.AddPlayerResult(playersInfoList, playersResults);
        string json = JsonUtility.ToJson(playersResults, true);
        SaveSystem.Save("PlayersResults.json", json);
        popup.Close();
        popupOpener.OpenPopup();
    }
    public void RestartScene()
    {
        GameManager.Instance.RestartScene();
    }
}
