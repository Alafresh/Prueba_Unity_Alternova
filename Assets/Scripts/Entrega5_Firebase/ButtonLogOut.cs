using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLogOut : MonoBehaviour
{
    [SerializeField] Button logOutBtn;
    private void Start() {
        logOutBtn.onClick.AddListener(SignOut);
    }
    private void SignOut() {
        FirebaseAuth.DefaultInstance.SignOut();
    }
}
