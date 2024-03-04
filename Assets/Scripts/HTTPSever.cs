using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

class User
{
    public string username;
    public string password;
    public string nickname;
    public string email;
}

public class HTTPSever : MonoBehaviour
{
    public MenuManager menuManager;

    /// <summary>
    /// User�� ȸ������ ���� ��û�� ������ �����ϴ�.
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="nickname"></param>
    /// <param name="email"></param>
    public void SendSignUpRequest(string username, string password, string nickname, string email)
    {
        User user = new User() { username = username, password = password, nickname = nickname, email = email };

        string userJson = JsonUtility.ToJson(user);

        print(userJson);

        StartCoroutine(PostRequest("http://localhost:8080/join", userJson));
    }

    IEnumerator PostRequest(string url, string userData)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(url, userData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(userData);
            webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // ��û ������
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                menuManager.SetNetworkMassageLog("�̹� ������� �̸��̳� ���̵��Դϴ�." + webRequest.error, Color.red);
            }
            else
            {
                menuManager.SetNetworkMassageLog("ȸ�� ����� �Ϸ�Ǿ����ϴ�.", Color.green);
            }
        }
    }
}
