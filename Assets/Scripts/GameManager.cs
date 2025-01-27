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

    private List<Card> cards;
    // Start is called before the first frame update
    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Bloques.json");

        if (File.Exists(path))
        {
            string jsonContent = File.ReadAllText(path);
            Debug.Log("Archivo JSON cargado correctamente:\n" + jsonContent);

            BlocksData blocksData = JsonUtility.FromJson<BlocksData>(jsonContent);

            foreach (var block in blocksData.blocks)
            {
                Debug.Log($"R: {block.R}, C: {block.C}, Number: {block.number}");
            }
            int numberColumns = GetColumnCount(blocksData.blocks);
            gridLayoutGroup.constraintCount = numberColumns;
            int numberRows = GetRowCount(blocksData.blocks);
            Debug.Log("Numero de columnas es"+ numberColumns);
            Debug.Log("Numero de filas es" + numberRows);
        }
    }

    int GetRowCount(Block[] blocks)
    {
        // Usamos un HashSet para almacenar valores únicos de rows
        HashSet<int> uniqueRows = new HashSet<int>();

        foreach (Block block in blocks)
        {
            uniqueRows.Add(block.R); // Agrega la fila al conjunto
        }
        return uniqueRows.Count; // El número de rows únicas
    }
    
    int GetColumnCount(Block[] blocks)
    {
        // Usamos un HashSet para almacenar valores únicos de columnas
        HashSet<int> uniqueColumns = new HashSet<int>();

        foreach (Block block in blocks)
        {
            uniqueColumns.Add(block.C); // Agrega la columna al conjunto
        }
        return uniqueColumns.Count; // El número de columnas únicas
    }
}


