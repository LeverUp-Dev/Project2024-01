using System.Collections;
using UnityEngine;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine.UI;
using Google;

public class FirebaseGoogleLogin : MonoBehaviour
{
    // �� API Ű�� ���̾�̽����� ������ �� �ְ� �� Ű�� ���� �α��ο� �ʼ���
    public string googleWebAPI = "1055524208047-csul82epr4qe9f3tetgnjguvt9075gms.apps.googleusercontent.com";

    private GoogleSignInConfiguration configuration;

    Firebase.DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    public Text usernameTxt;
    public Text userEmailTxt;

    public Image userProfilePic;

    public string imageUrl;

    public GameObject loginScreen;
    public GameObject profileScreen;

    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = googleWebAPI,
            RequestIdToken = true
        };
    }

    void Start()
    {
        InitFirebase();

        Debug.Log("auth Ȯ��:" + auth);
    }

    void InitFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }

    public void GoogleSignInClick()
    {
        Debug.Log("Ŭ��");

        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnGoogleAuthenticatedFinished);
    }

    void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
    {
        Debug.Log("�α��� �õ�");

        if (task.IsFaulted)
        {
            Debug.LogError("���� �α��� ����");
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("���� �α��� �õ� ��ҵ�");
        }
        else
        {
            Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync�� ��ҵ�");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync���� ������ �߻�: " + task.Exception);
                    return;
                }

                user = auth.CurrentUser;

                usernameTxt.text = user.DisplayName;
                userEmailTxt.text = user.Email;

                loginScreen.SetActive(false);
                profileScreen.SetActive(true);

                StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.ToString())));

                Debug.Log("�̹��� URL: " + CheckImageUrl(user.PhotoUrl.ToString()));
            });
        }
    }

    private string CheckImageUrl(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            return url;
        }

        return imageUrl;
    }

    IEnumerator LoadImage(string imageUrl)
    {
        WWW www = new WWW(imageUrl);
        yield return www;

        userProfilePic.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }
}
