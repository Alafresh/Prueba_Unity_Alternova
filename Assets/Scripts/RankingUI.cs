using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class RankingUI : MonoBehaviour
{
    [SerializeField] List<Transform> itemList = new List<Transform>();
    private PlayersResults playersResults;
    private List<PlayerInfo> playersInfoList = new List<PlayerInfo>();

    private void Awake()
    {
        foreach (var item in itemList)
        { 
            item.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        ReadPlayerResults();
        SortScores();
        SetUpRanking();
    }

    private void ReadPlayerResults()
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
            catch (System.Exception e)
            {
                Debug.LogError("Error al cargar el archivo JSON: " + e.Message);
                return;
            }
        }
    }
    private void SetUpRanking()
    {
        for(int i = 0; i < playersInfoList.Count; i++)
        {
            Debug.Log(playersInfoList[i].name + " " + playersInfoList[i].score);
            Debug.Log(itemList[i]);
            Debug.Log(playersInfoList[i].name);
            itemList[i].Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = playersInfoList[i].name;
            itemList[i].Find("ScoreTitle").GetComponent<TMPro.TextMeshProUGUI>().text = playersInfoList[i].score.ToString();
            itemList[i].gameObject.SetActive(true);
        }
    }
    private void SortScores()
    {
        playersInfoList.Sort((x, y) => y.score.CompareTo(x.score));
    }
}
