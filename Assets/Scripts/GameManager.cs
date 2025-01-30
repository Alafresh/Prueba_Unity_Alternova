using Ricimi;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Block 
{
    public int R;
    public int C;
    public int number;
}

[System.Serializable]
public class BlocksData
{
    public Block[] blocks;
}

[System.Serializable]
public class Results
{
    public int total_clicks;
    public int total_time;
    public int pairs;
    public int score;
}

[System.Serializable]
public class GamesResults
{
    public Results[] results;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private Timer timer;
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Transform cardPrefab;
    [SerializeField] private List<Card> cardPool;
    [SerializeField] private PopupOpener winPanel;
    [SerializeField] private Sprite rightPair;
    private GameObject card1, card2;
    private Card card1Component, card2Component;
    private BlocksData blocksData;
    private GamesResults gameResults;
    private int totalClicks;
    private int totalPairs;
    public UnityEvent<int> UpdateClicksUI;
    public UnityEvent<int> UpdatePairsUI;
    private List<Results> resultsList = new List<Results>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Hay mas de un GameManager " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SaveSystem.Init();
    }

    void Start()
    {
        string path = "Blocks.json";
        int numberRows;
        int numberColumns;
        bool numberInRange;
        string saveString = SaveSystem.Load(path);
        blocksData = JsonUtility.FromJson<BlocksData>(saveString);

        if (blocksData.blocks.Length % 2 != 0)
        {
            Debug.LogError("El número de bloques debe ser par.");
            return;
        }
        numberInRange = GetNumbersInRange(blocksData.blocks);
        if (!numberInRange)
        {
            Debug.LogError("El valor de los bloques debe estar entre 0 y 9.");
            return;
        }
        numberRows = GetRowCount(blocksData.blocks);
        if(numberRows < 2 || numberRows > 8)
        {
            Debug.LogError("El número de filas debe estar entre 2 y 8.");
            return;
        }
        numberColumns = GetColumnCount(blocksData.blocks);
        if (numberColumns < 2 || numberColumns > 8)
        {
            Debug.LogError("El número de columnas debe estar entre 2 y 8.");
            return;
        }
        if (numberRows < 5)
        {
            gridLayoutGroup.cellSize = new Vector2(170, 170);
        }
        gridLayoutGroup.constraintCount = numberRows;

        Array.Sort(blocksData.blocks, (a, b) =>
        {
            if (a.R == b.R)
                return a.C.CompareTo(b.C);
                return a.R.CompareTo(b.R); 
        });

        foreach(Card card in cardPool)
        {
            card.gameObject.SetActive(false);
        }

        for (int i = 0; i < blocksData.blocks.Length; i++)  
        {
            cardPool[i].cardId = blocksData.blocks[i].number;
            cardPool[i].row = blocksData.blocks[i].R;
            cardPool[i].column = blocksData.blocks[i].C;
            cardPool[i].numberText.alpha = 0;
            cardPool[i].numberText.text = blocksData.blocks[i].number.ToString();
            cardPool[i].gameObject.SetActive(true);
        }
    }

    public void GetPair()
    {
        if (card1 == null && card2 == null)
        {
            card1 = EventSystem.current.currentSelectedGameObject;
            card1.TryGetComponent(out Card cardComponent);
            card1Component = cardComponent;
            cardComponent.numberText.alpha = 1;
            cardComponent.isUsed = true;
            card1.TryGetComponent(out Button buttonComponent);
            buttonComponent.interactable = false;
        }
        else if (card1 != null && card2 == null)
        {
            if(EventSystem.current.currentSelectedGameObject != card1)
            {
                card2 = EventSystem.current.currentSelectedGameObject;
                card2.TryGetComponent(out Card cardComponent);
                card2Component = cardComponent;
                cardComponent.numberText.alpha = 1;
                cardComponent.isUsed = true;
                card2.TryGetComponent(out Button buttonComponent);
                buttonComponent.interactable = false;
                CheckPair(card1Component, card2Component);
            }
        }
    }
    
    private void CheckPair(Card card1, Card card2)
    {
        if (card1.cardId == card2.cardId)
        {
            card1.transform.GetChild(0).TryGetComponent(out Image image1);
            card2.transform.GetChild(0).TryGetComponent(out Image image2);
            image1.sprite = image2.sprite = rightPair;
            Debug.Log("Par encontrado");
            totalPairs++;
            UpdatePairsUI.Invoke(totalPairs);
            this.card1 = this.card2 = null;
            card1Component = card2Component = null;

            if (CheckWin())
            {
                winPanel.OpenPopup();
                CheckResultsToJson();
            }
        }
        else
        {
            card1Component.isUsed = card2Component.isUsed = false;
            this.card1.TryGetComponent(out Button buttonComponent1);
            this.card2.TryGetComponent(out Button buttonComponent2);
            buttonComponent1.interactable = buttonComponent2.interactable = true;
            StartCoroutine(HideCards());
        }
    }

    private IEnumerator HideCards()
    {
        yield return new WaitForSeconds(1);
        card1Component.numberText.alpha = card2Component.numberText.alpha = 0;
        card1.TryGetComponent(out Button buttonComponent1);
        card2.TryGetComponent(out Button buttonComponent2);
        buttonComponent1.animationTriggers.normalTrigger = buttonComponent1.animationTriggers.normalTrigger = "Pressed";
        card1 = card2 = null;
        card1Component = card2Component = null;
    }

    private bool GetNumbersInRange(Block[] blocks) 
    {
        foreach (Block block in blocks)
        {
            if(block.number < 0 || block.number > 9)
            {
                Debug.LogError($"El bloque con el valor {block.number} no se encuentra en el rango");
                return false;
            }
        }
        return true;
    }

    private bool CheckWin()
    {
        int totalCards = blocksData.blocks.Length;
        int count = 0;
        foreach (Card card in cardPool)
        {
            if (card.isUsed)
            {
                count++;
            }
        }
        if (count == totalCards)
        {
            timer.StopTimer();
            return true;
        }
        return false;
    }

    public void AddClick()
    {
        totalClicks++;
        UpdateClicksUI.Invoke(totalClicks);
        Debug.Log("Total clicks: " + totalClicks);
    }

    public int CalculateScore()
    {
        int score = 0;
        score += 1000 - timer.GetTime();
        score += 1000 - totalClicks;
        score += 1000 * totalPairs;
        return score;
    }

    private void CheckResultsToJson()
    {
        string pathGame = "GameResults.json";
        string existingJson = SaveSystem.Load(pathGame);


        if (string.IsNullOrEmpty(existingJson))
        {
            SaveSystem.CreateGameResults();
            return;
        }
        existingJson = SaveSystem.Load(pathGame);
        gameResults = JsonUtility.FromJson<GamesResults>(existingJson);
        
        if (gameResults.results != null && gameResults != null)
        {
            resultsList.AddRange(gameResults.results);
        }
        SaveResultsToJson();
    }

    private void SaveResultsToJson()
    {
        gameResults = SaveSystem.AddGameResult(resultsList, gameResults);
        string json = JsonUtility.ToJson(gameResults, true);
        SaveSystem.Save("GameResults.json", json);
    }

    private int GetRowCount(Block[] blocks)
    {
        HashSet<int> uniqueRows = new HashSet<int>();
        foreach (Block block in blocks)
        {
            uniqueRows.Add(block.R);
        }
        return uniqueRows.Count;
    }

    private int GetColumnCount(Block[] blocks)
    {

        HashSet<int> uniqueColumns = new HashSet<int>();

        foreach (Block block in blocks)
        {
            uniqueColumns.Add(block.C);
        }
        return uniqueColumns.Count;
    }

    public int GetTotalPairs()
    {
        return totalPairs;
    }
    public int GetTotalClicks()
    {
        return totalClicks;
    }
    public int GetTotalTime()
    {
        return timer.GetTime();
    }

    public void OnApplicationQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void RestartScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}