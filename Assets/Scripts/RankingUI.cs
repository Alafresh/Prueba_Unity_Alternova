using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankingUI : MonoBehaviour
{
    [SerializeField] Transform item;
    [SerializeField] Transform content;

    private void Awake()
    {
        item.gameObject.SetActive(false);

        for (int i = 0; i < 10; i++)
        {
            Transform newItem = Instantiate(item, content);
            newItem.gameObject.SetActive(true);

            int rank = i + 1;
            string rankString;

            switch(rank)
            {
                case 1:
                    rankString = "#1";
                    break;
                case 2:
                    rankString = "#2";
                    break;
                case 3:
                    rankString = "#3";
                    break;
                default:
                    rankString = "#" + rank;
                    break;
            }

            newItem.Find("Rank/RankText").GetComponent<TMPro.TextMeshProUGUI>().text = rankString;
            //newItem.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = "Player" + i;
            //newItem.Find("Score").GetComponent<TMPro.TextMeshProUGUI>().text = Random.Range(0, 10000).ToString();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
