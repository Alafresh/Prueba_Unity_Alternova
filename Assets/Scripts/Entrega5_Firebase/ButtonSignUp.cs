using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSignUp : MonoBehaviour
{
    [SerializeField] private Button registrationBtn;
    [SerializeField] private TMP_InputField emailInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField usernameInputField;
    private Coroutine _regristrationCoroutine;
    private DatabaseReference _mDatabaseRef;

    void Start() {
        registrationBtn.onClick.AddListener(HandleRegisterButtonClecked);
        _mDatabaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    }

    private void HandleRegisterButtonClecked() {
        string email = emailInputField.text.ToLower();
        string password = passwordInputField.text.ToLower();

        _regristrationCoroutine = StartCoroutine(RegisterUser(email, password));
    }

    private IEnumerator RegisterUser(string email, string password) {
        var auth = FirebaseAuth.DefaultInstance;
        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.IsCanceled) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled");
        } else if (registerTask.IsFaulted) {
            Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + registerTask.Exception);
        } else {
            AuthResult result = registerTask.Result;
            Debug.LogFormat("Firebase useer created successfully: {0} {1}",
                result.User.DisplayName, result.User.UserId);
            
            var userId = result.User.UserId;
            string username = usernameInputField.text.ToLower();

            _mDatabaseRef.Child("users").Child(userId).Child("username").SetValueAsync(username);
        }
    }
}
