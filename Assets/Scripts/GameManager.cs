using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

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

public class GameManager : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] private Transform cardPrefab;
    private List<Card> cards;
    
    // Start is called before the first frame update
    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Bloques.json");
        string jsonContent;
        Transform gameCard;
        BlocksData blocksData;
        int numberRows;
        int numberColumns;
        bool numberInRange;

        if (File.Exists(path))
        {
            try
            {
                jsonContent = File.ReadAllText(path);
            }
            catch (Exception e)
            {
                Debug.LogError("Error al cargar el archivo JSON: " + e.Message);
                return;
            }
            
            Debug.Log("Archivo JSON cargado correctamente:\n" + jsonContent);

            blocksData = JsonUtility.FromJson<BlocksData>(jsonContent);

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

            gridLayoutGroup.constraintCount = numberColumns;

            Array.Sort(blocksData.blocks, (a, b) =>
            {
                if (a.R == b.R)
                    return a.C.CompareTo(b.C);
                return a.R.CompareTo(b.R); 
            });


            foreach (var block in blocksData.blocks)
            {
                gameCard = Instantiate(cardPrefab, gridLayoutGroup.transform);
                if (gameCard.TryGetComponent(out Card cardComponent))
                {
                    cardComponent.cardId = block.number;
                    cardComponent.column = block.C;
                    cardComponent.row = block.R;
                    cardComponent.numberText.text = block.number.ToString();
                }
            }
        }
        else
        {
            Debug.LogError("El archivo JSON no se encontró en la ruta: " + path);
        }
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
}


