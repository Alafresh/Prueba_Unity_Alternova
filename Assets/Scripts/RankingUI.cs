using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        ReadPlayerResults();
        SortScores();
        SetUpRanking();
    }

    private void ReadPlayerResults()
    {
        string pathPlayersResults = "PlayersResults.json";
        string existingJson = SaveSystem.Load(pathPlayersResults);

        if (string.IsNullOrEmpty(existingJson))
        {
            Debug.LogError("Error al cargar el archivo JSON");
            return;
        }

        playersResults = JsonUtility.FromJson<PlayersResults>(existingJson);
        
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
            Debug.Log(playersInfoList[i].name + " " + playersInfoList[i].score);
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

    public void RestartScene()
    {
        GameManager.Instance.RestartScene();
    }

}
