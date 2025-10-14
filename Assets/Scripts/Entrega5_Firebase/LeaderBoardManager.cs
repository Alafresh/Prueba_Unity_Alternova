using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardManager : MonoBehaviour
{
    public Dictionary<string, object> GetLeaderBoard() {
        Dictionary<string, object> leaderBoardUsers = new();
        FirebaseDatabase.DefaultInstance
            .GetReference("users").OrderByChild("score").LimitToLast(2)
            .GetValueAsync().ContinueWithOnMainThread(task => {
                if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log(snapshot.Value);

                    leaderBoardUsers = (Dictionary<string, object>)snapshot.Value;
                    Debug.Log(leaderBoardUsers.Count);
                } else {
                    leaderBoardUsers = null;
                }
            });
        return leaderBoardUsers;
    }
}
