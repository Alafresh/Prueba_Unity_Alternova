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
    private PlayersResults playersResults;
    private List<PlayerInfo> playersInfoList = new List<PlayerInfo>();

    public void Submit()
    {
        StartCoroutine(SubmitCoroutine());
    }

    private IEnumerator SubmitCoroutine()
    {
        string name = userName.text;
        int score = GameManager.Instance.CalculateScore();
        PlayerInfo playerInfo = new PlayerInfo(name, score);
        playersInfoList.Add(playerInfo);
        Task task = CheckResultsToJson();
        yield return new WaitUntil(() => task.IsCompleted);
    }
    private async Task CheckResultsToJson()
    {
        string pathPlayersResults = Application.dataPath + "/PlayersResults.json";
        string existingJson;

        if (File.Exists(pathPlayersResults))
        {
            try
            {
                existingJson = File.ReadAllText(pathPlayersResults);
                playersResults = JsonUtility.FromJson<PlayersResults>(existingJson);
                if (playersResults.players != null && playersResults != null)
                {
                    playersInfoList.AddRange(playersResults.players);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error al cargar el archivo JSON: " + e.Message);
                return;
            }

            await SaveResultsToJson();

            Debug.Log("Resultados guardados en: " + pathPlayersResults);
        }
        else
        {
            await SaveResultsToJson();
        }
    }

    private async Task SaveResultsToJson()
    {
        playersResults = new PlayersResults()
        {
            players = playersInfoList.ToArray()
        };

        string json = JsonUtility.ToJson(playersResults, true);
        string path = Application.dataPath + "/PlayersResults.json";
        await File.WriteAllTextAsync(path, json);
        popupOpener.OpenPopup();
    }
}
