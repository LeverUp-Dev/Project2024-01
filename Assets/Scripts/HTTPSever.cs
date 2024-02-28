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
    /// User�� ȸ������ ���� ��û�� ������ �����ϴ�.
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
