using UnityEngine;
using TMPro;

public class StatisticsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalClicksText;
    [SerializeField] TextMeshProUGUI totalPairsText;

    private void Start()
    {
        GameManager.Instance.UpdateClicksUI.AddListener(UpdateClicksUI);
        GameManager.Instance.UpdatePairsUI.AddListener(UpdatePairsUI);
    }

    private void UpdateClicksUI(int numClicks)
    {
        totalClicksText.text = numClicks.ToString();
    }

    private void UpdatePairsUI(int numPairs)
    {
        totalPairsText.text = numPairs.ToString();
    }
}
