using TMPro;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class ButtonLogIn : MonoBehaviour
{
    [SerializeField] private Button loginBtn;
    [SerializeField] private TMP_InputField emailInputFiel;
    [SerializeField] private TMP_InputField passwordInputFiel;

    private void Start() {
        loginBtn.onClick.AddListener(HandleLoginButtonClicked);
    }

    private void HandleLoginButtonClicked() {
        var auth = FirebaseAuth.DefaultInstance;
        auth.SignInWithEmailAndPasswordAsync(emailInputFiel.text.ToLower(), passwordInputFiel.text.ToLower())
            .ContinueWith(task => {
                if (task.IsCanceled) {
                    Debug.LogError("SignInWithEmailAndPasswordAsync was canceled");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                }
                AuthResult result = task.Result;
                Debug.LogFormat("User signed in successfully: {0} {1}",
                    result.User.DisplayName, result.User.UserId);
            });
    }
}
