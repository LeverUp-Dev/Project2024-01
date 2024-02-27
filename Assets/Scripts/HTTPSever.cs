using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

class User
{
    public string username;
    public string userId;
    public string password;
}

public class HTTPSever : MonoBehaviour
{
    User user = new User() { username = "미하루", userId = "ododo3", password = "esa101904" };

    void Start()
    {
        string userJson = JsonUtility.ToJson(user);

        print(userJson);

        StartCoroutine(PostRequest("http://localhost:8080/api/v1/users", userJson));
    }

    IEnumerator PostRequest(string url, string userData)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, userData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(userData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // 요청 보내기
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError(webRequest.error);
            }
            else
            {
                Debug.Log("회원가입 성공!");
            }
        }
    }
}
