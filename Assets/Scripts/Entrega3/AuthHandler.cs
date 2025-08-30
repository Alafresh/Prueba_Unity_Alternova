using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class UserData_AuthRequest {
    public string username;
    public string password;
}
[Serializable]
class AuthResponse {
    public User usuario;
    public string token;
}
[Serializable]
class User {
    public string _id;
    public string username;
    public UserData data;
}
[Serializable]
class UserData {
    public int score;
}

public class AuthHandler : MonoBehaviour {

    private const string BASE_URI = "https://sid-restapi.onrender.com/api/";

    [SerializeField] private GameObject SignUpObject;
    [SerializeField] private GameObject LogInObject;
    [SerializeField] private TMP_InputField username;
    [SerializeField] private TMP_InputField password;
    [SerializeField] private TextMeshProUGUI msgSignUp;
    [SerializeField] private TextMeshProUGUI msgLogIn;


    private bool flag = false;
    private WaitForSeconds waiting = new WaitForSeconds(3);

    public void ChangeWindow() {
        flag = !flag;
        SignUpObject.SetActive(flag);
        LogInObject.SetActive(!flag);
    }
    public void SignUpBtn() {
        StartCoroutine(SignUp());
    }

    public void LogInBtn() {
        StartCoroutine(LogIn());
    }

    private IEnumerator SignUp() {
        string jsonData = JsonUtility.ToJson(new UserData_AuthRequest { username = username.text, password = password.text });
        string url = BASE_URI + "usuarios";
        using UnityWebRequest www = UnityWebRequest.Post(url, jsonData, "application/json");
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) {
            msgSignUp.text = "Sign Up successful";
            yield return waiting;
            ChangeWindow();
        } else {
            msgSignUp.text = www.downloadHandler.text;
        }
    }

    private IEnumerator LogIn() {
        string jsonData = JsonUtility.ToJson(new UserData_AuthRequest { username = username.text, password = password.text });
        string url = BASE_URI + "auth/login";
        using UnityWebRequest www = UnityWebRequest.Post(url, jsonData, "application/json");
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) {
            Debug.Log("Login successful");
            AuthResponse response = JsonUtility.FromJson<AuthResponse>(www.downloadHandler.text);

            PlayerPrefs.SetString("token", response.token);
            PlayerPrefs.SetString("username", response.usuario.username);
            msgLogIn.text = "Login successful";

        } else {
            msgLogIn.text = www.downloadHandler.text;
        }

    }
    private IEnumerator GetProfile() {
        string url = BASE_URI + $"usuarios/{PlayerPrefs.GetString("username")}";
        using UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("x-token", PlayerPrefs.GetString("token"));
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) {

        } else {
        
        }
    }
}
