using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class UserData_AuthRequest {
    public string username;
    public string password;
}

public class AuthHandler : MonoBehaviour {
    string auth = "auth/login";

    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TextMeshProUGUI response;
    private const string BASE_URI = "https://sid-restapi.onrender.com/api/";

    public void SignUpBtn() {
        StartCoroutine(SignUp());
    }

    public void LogInBtn() {

    }

    private IEnumerator SignUp() {
        string jsonData = JsonUtility.ToJson(new UserData_AuthRequest { username = username.text, password = password.text });
        string url = BASE_URI + "usuarios";
        using UnityWebRequest www = UnityWebRequest.Post(url, jsonData, "application/json");
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) {
            response.text = "Sign Up successful";
            

        } else {
            response.text = www.downloadHandler.text;
        }
    }

    private IEnumerator LogIn() {
        yield return new WaitForSeconds(3);
    }
}
