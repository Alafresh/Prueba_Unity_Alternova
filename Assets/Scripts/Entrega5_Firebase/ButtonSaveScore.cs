using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSaveScore : MonoBehaviour
{
    private DatabaseReference mDatabaseRef;

    private void Start() {
        mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void HandleSaveScoreButtonClicked() {
        var currentUser = FirebaseAuth.DefaultInstance.CurrentUser;

        if (currentUser != null) {
            int score = GameManager.Instance.CalculateScore();
            int oldScore = 0;
            mDatabaseRef.Child("users").Child(currentUser.UserId).Child("score")
                .GetValueAsync().ContinueWithOnMainThread(task => {
                    if (task.IsCompleted) {
                        DataSnapshot snap = task.Result;
                        oldScore = Convert.ToInt32(snap.Value);
                        Debug.Log($"oldScore={oldScore}, newScore={score}");
                        if (oldScore == 0) {
                            mDatabaseRef.Child("users").Child(currentUser.UserId).
                            Child("score").SetValueAsync(score);
                        }
                        if (score > oldScore) {
                            mDatabaseRef.Child("users").Child(currentUser.UserId).
                            Child("score").SetValueAsync(score);
                        }
                    }
                });
            
        }
    }
}
