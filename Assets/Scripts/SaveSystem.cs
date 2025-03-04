using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading.Tasks;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.streamingAssetsPath + "/";

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    public static void Save(string saveName, string jsonContent)
    {
        string path = SAVE_FOLDER + saveName;
        try
        {
            File.WriteAllText(path, jsonContent);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving file: " + e.Message);
        }
    }

    public static async Task SaveAsync(string saveName, string jsonContent)
    {
        string path = SAVE_FOLDER + saveName;
        try
        {
            await File.WriteAllTextAsync(path, jsonContent);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving file: " + e.Message);
        }
    }

    public static string Load(string saveName)
    {
        Debug.Log("Loading file: " + SAVE_FOLDER + saveName);
        string path = SAVE_FOLDER + saveName;
        string jsonContent;
        if (File.Exists(path))
        {
            try
            {
                jsonContent = File.ReadAllText(path);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Error loading file: " + e.Message);
                return null;
            }
            return jsonContent;
        }
        else
        {
            if(saveName == "GameResults.json" || saveName == "PlayersResults.json")
            {
                return null;
            }
            Debug.LogError("File not found: " + path);
            return null;
        }
    }

    public static void CreatePlayersResults(PlayersResults playersResults, List<PlayerInfo> playersInfoList)
    {
        playersResults = new PlayersResults()
        {
            players = playersInfoList.ToArray()
        };

        string json = JsonUtility.ToJson(playersResults, true);
        Save("PlayersResults.json", json);
    }

    public static PlayersResults AddPlayerResult(List<PlayerInfo> playersInfoList, PlayersResults playersResults)
    {
        playersResults = new PlayersResults()
        {
            players = playersInfoList.ToArray()
        };
        return playersResults;
    }

    public static void CreateGameResults()
    {
        List<Results> resultsList = new List<Results>();
        resultsList.Add(new Results()
        {
            total_clicks = GameManager.Instance.GetTotalClicks(),
            total_time = GameManager.Instance.GetTotalTime(),
            pairs = GameManager.Instance.GetTotalPairs(),
            score = GameManager.Instance.CalculateScore()
        });

        GamesResults gamesResults = new GamesResults();
        gamesResults = new GamesResults()
        {
            results = resultsList.ToArray()
        };
        string json = JsonUtility.ToJson(gamesResults, true);
        SaveSystem.Save("GameResults.json", json);
    }
    public static GamesResults AddGameResult(List<Results> resultsList, GamesResults gamesResults)
    {
        resultsList.Add(new Results()
        {
            total_clicks = GameManager.Instance.GetTotalClicks(),
            total_time = GameManager.Instance.GetTotalTime(),
            pairs = GameManager.Instance.GetTotalPairs(),
            score = GameManager.Instance.CalculateScore()
        });

        gamesResults = new GamesResults()
        {
            results = resultsList.ToArray()
        };
        return gamesResults;
    }
}
