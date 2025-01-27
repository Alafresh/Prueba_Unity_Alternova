using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


