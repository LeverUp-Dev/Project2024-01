using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

class User
{
    public string nickname;
    public string userId;
    public string password;
}

public class HTTPSever : MonoBehaviour
{
    public MenuManager menuManager;

    /// <summary>
    /// User의 회원가입 승인 요청을 서버에 보냅니다.
    /// </summary>
    /// <param name="nickname"></param>
    /// <param name="userId"></param>
    /// <param name="password"></param>
    public void SendSignUpRequest(string nickname, string userId, string password)
    {
        User user = new User() { nickname = nickname, userId = userId, password = password };

        string userJson = JsonUtility.ToJson(user);

        //print(userJson);

        StartCoroutine(PostRequest("http://localhost:8080/api/v2/users", userJson));
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

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                menuManager.SetNetworkMassageLog("이미 사용중인 이름이나 아이디입니다." + webRequest.error, Color.red);
            }
            else
            {
                menuManager.SetNetworkMassageLog("회원 등록이 완료되었습니다.", Color.green);
            }
        }
    }
}
