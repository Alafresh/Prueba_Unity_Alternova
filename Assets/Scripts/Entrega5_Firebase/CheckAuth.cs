using System;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class CheckAuth : MonoBehaviour
{
    [SerializeField] string sceneToLoad;

    private void Start() {
        FirebaseAuth.DefaultInstance.StateChanged += HandleAuthStateChange;
    }
    private void HandleAuthStateChange(object sender, EventArgs e) {
        if (FirebaseAuth.DefaultInstance.CurrentUser != null) {
            Debug.LogFormat(FirebaseAuth.DefaultInstance.CurrentUser.UserId);
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    private void OnDestroy() {
        FirebaseAuth.DefaultInstance.StateChanged -= HandleAuthStateChange;
    }
}
