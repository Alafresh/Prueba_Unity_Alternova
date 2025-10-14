using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using TMPro;
using System;
using Firebase.Extensions;

public class StatisticsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI totalClicksText;
    [SerializeField] TextMeshProUGUI totalPairsText;
    [SerializeField] private TextMeshProUGUI textUsername;

    private void Start()
    {
        FirebaseAuth.DefaultInstance.StateChanged += HanldeAuthChange;
        GameManager.Instance.UpdateClicksUI.AddListener(UpdateClicksUI);
        GameManager.Instance.UpdatePairsUI.AddListener(UpdatePairsUI);
    }

    private void HanldeAuthChange(object sender, EventArgs e) {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;

        if (currentUser != null) {
            SetLabelUsername(currentUser.UserId);
        }
    }

    private void SetLabelUsername(string userId) {
        FirebaseDatabase.DefaultInstance
            .GetReference("users/" + userId + "/username")
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsFaulted) {
                    Debug.Log(task.Exception);
                    textUsername.text = "NULL";
                } else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    textUsername.text = (string)snapshot.Value;
                }
            });
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
