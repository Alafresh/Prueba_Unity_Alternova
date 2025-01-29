using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardId;
    public int row;
    public int column;
    public bool isUsed;
    public TextMeshProUGUI numberText;
    [SerializeField] Button button;

    private void Start()
    {
        button.onClick.AddListener(() => GameManager.Instance.GetPair());
        button.onClick.AddListener(() => GameManager.Instance.AddClick());
    }
}
