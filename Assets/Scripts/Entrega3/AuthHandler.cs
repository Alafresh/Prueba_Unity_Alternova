using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public string username;
    public UserData data;
}
[Serializable]
class UserData {
    public int score;
}

public class AuthHandler : MonoBehaviour {

    private const string BASE_URI = "https://sid-restapi.onrender.com/api/";

    [SerializeField] private Button authBtn;
    [SerializeField] private GameObject SignUpObject;
    [SerializeField] private GameObject LogInObject;
    [SerializeField] private TMP_InputField usernameSignUp;
    [SerializeField] private TMP_InputField usernameLogin;
    [SerializeField] private TMP_InputField passwordSignUp;
    [SerializeField] private TMP_InputField passwordLogin;
    [SerializeField] private TextMeshProUGUI msgSignUp;
    [SerializeField] private TextMeshProUGUI msgLogIn;
    [SerializeField] private TextMeshProUGUI msgGetProfile;

    private bool _flag = false;
    private readonly WaitForSeconds _waiting = new(3);
    private static AuthResponse _response;
    private static User _user;

    public static AuthHandler Instance { get; private set; }

    public void SignUpBtn() => StartCoroutine(SignUp());
    public void LogInBtn() => StartCoroutine(LogIn());
    public void UpdateScore(int score) => StartCoroutine(UpdateData(score));
    public void LeaderBoard() => StartCoroutine(GetScoreBoard());
    public string GetUsername() => _response.usuario.username.ToString();

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is mora than one AuthHandler");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("token")) && !string.IsNullOrEmpty(PlayerPrefs.GetString("username"))) {
            StartCoroutine(GetProfile());
        } else {
            msgGetProfile.text = "No token found, please log in.";
        }
    }

    public void ChangeWindow() {
        _flag = !_flag;
        SignUpObject.SetActive(_flag);
        LogInObject.SetActive(!_flag);
    }

    private IEnumerator SignUp() {
        string jsonData = JsonUtility.ToJson(new UserData_AuthRequest { 
            username = usernameSignUp.text, 
            password = passwordSignUp.text 
        });
        string url = BASE_URI + "usuarios";
        using UnityWebRequest www = UnityWebRequest.Post(url, jsonData, "application/json");
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) {
            msgSignUp.text = "Sign Up successful";
            yield return _waiting;
            ChangeWindow();
        } else {
            msgSignUp.text = www.downloadHandler.text;
        }
    }

    private IEnumerator LogIn() {
        string jsonData = JsonUtility.ToJson(new UserData_AuthRequest { 
            username = usernameLogin.text, 
            password = passwordLogin.text 
        });
        string url = BASE_URI + "auth/login";
        using UnityWebRequest www = UnityWebRequest.Post(url, jsonData, "application/json");
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) {
            Debug.Log("Login successful");
            _response = JsonUtility.FromJson<AuthResponse>(www.downloadHandler.text);
            PlayerPrefs.SetString("token", _response.token);
            PlayerPrefs.SetString("username", _response.usuario.username);
            msgLogIn.text = "Login successful";
            yield return _waiting;
            StartCoroutine(LoadAsyncScene());
        } else {
            Debug.Log(usernameLogin.text.ToString() + " " + passwordLogin.text);
            msgLogIn.text = www.downloadHandler.text;
        }

    }
    private IEnumerator GetProfile() {
        string url = BASE_URI + $"usuarios/{PlayerPrefs.GetString("username")}";
        using UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("x-token", PlayerPrefs.GetString("token"));
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) {
            _response = JsonUtility.FromJson<AuthResponse>(www.downloadHandler.text);
            
            msgGetProfile.text = "Login successful";
            yield return _waiting;
            StartCoroutine(LoadAsyncScene());
        } else {
            msgGetProfile.text = www.downloadHandler.text;
            authBtn.interactable = true;
        }
    }
    private IEnumerator UpdateData(int score) {
        _user = new User {
            username = _response.usuario.username,
            data = new UserData {
                score = score
            }
        };
        string jsonData = JsonUtility.ToJson(_user);
        Debug.Log(jsonData);
        string url = BASE_URI + "usuarios";
        using UnityWebRequest www = UnityWebRequest.Post(url, jsonData, "application/json");
        www.method = "PATCH";
        www.SetRequestHeader("x-token", PlayerPrefs.GetString("token"));
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success) {
            Debug.Log("Se actualizo los datos");
            yield return _waiting;
        } else {
            Debug.LogError(www.downloadHandler.text);
        }
    }
    private IEnumerator GetScoreBoard() {
        yield return _waiting;
    }
    private IEnumerator LoadAsyncScene() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);
        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
