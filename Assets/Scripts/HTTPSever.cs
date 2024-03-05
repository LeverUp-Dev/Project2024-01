using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

class Join
{
    public string username;
    public string password;
    public string nickname;
    public string email;
}

class Login
{
    public string username;
    public string password;
}

public class HTTPSever : MonoBehaviour
{
    public MenuManager menuManager;

    /// <summary>
    /// User의 회원가입 승인 요청을 서버에 보냅니다.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="nickname"></param>
    /// <param name="email"></param>
    public void SendSignUpRequest(string username, string password, string nickname, string email)
    {
        Join user = new Join() { username = username, password = password, nickname = nickname, email = email };

        string joinJson = JsonUtility.ToJson(user);

        StartCoroutine(PostJoinRequest("http://localhost:8080/join", joinJson));
    }

    IEnumerator PostJoinRequest(string url, string userData)
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
                menuManager.SetNetworkMassageLog("이미 사용중인 이름이나 아이디입니다." + webRequest.error, Color.red, false);
            }
            else
            {
                menuManager.SetNetworkMassageLog("회원 등록이 완료되었습니다.", Color.green, false);
            }
        }
    }

    public void SendLoginRequest(string username, string password)
    {
        Login login = new Login { username = username, password = password };

        string loginJson = JsonUtility.ToJson(login);

        StartCoroutine(PostLoginRequest("http://localhost:8080/login", loginJson));
    }

    IEnumerator PostLoginRequest(string url, string userData)
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
                menuManager.SetNetworkMassageLog("존재하지 않는 회원입니다." + webRequest.error, Color.red, true);
            }
            else
            {
                menuManager.SetNetworkMassageLog("로그인이 완료되었습니다.", Color.green, true);
            }
        }
    }
}
