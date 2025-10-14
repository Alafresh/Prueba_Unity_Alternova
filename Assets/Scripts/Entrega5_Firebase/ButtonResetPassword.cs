using Firebase.Auth;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonResetPassword : MonoBehaviour
{
    [SerializeField] Button btnResetPassword;
    [SerializeField] private TMP_InputField emailInputFiel;
    private void Start() {
        btnResetPassword.onClick.AddListener(HandleResetPasswordButtonClicked);
    }

    public void HandleResetPasswordButtonClicked() {
        var auth = FirebaseAuth.DefaultInstance;
        var user = auth.CurrentUser;

        string emailAddress = emailInputFiel.text.ToLower();
        auth.SendPasswordResetEmailAsync(emailAddress).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                return;
            }

            Debug.Log("Password reset email sent successfully.");
        });

        Debug.Log("Hola");
    }
}
