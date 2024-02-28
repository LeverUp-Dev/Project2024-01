using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Start Menu")]
    [SerializeField] private GameObject startMenu;

    [SerializeField] private GameObject loginPanel;
    [SerializeField] private InputField loginId;
    [SerializeField] private InputField loginPassword;
    [SerializeField] private Button sendLoginBT;

    [SerializeField] private GameObject signupPanel;
    [SerializeField] private InputField signupName;
    [SerializeField] private InputField signupId;
    [SerializeField] private InputField signupPassword;
    [SerializeField] private Button sendSignupBT;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenu;

    [Header("Network Massage")]
    [SerializeField] private GameObject massageImage;
    [SerializeField] private Text networkMassage;

    [Header("Network Manager")]
    public HTTPSever HTTPSever;

    void Start()
    {
        startMenu.SetActive(true);
    }

    public void OnClickLogin()
    {
        loginPanel.SetActive(true);
    }

    public void OnClickLoginCancel()
    {
        loginId.text = null;
        loginPassword.text = null;

        loginPanel.SetActive(false);
    }

    public void OnClickSandLogin()
    {

    }

    public void OnClickSignUp()
    {
        signupPanel.SetActive(true);
    }

    public void OnClickSignUpCancel()
    {
        signupName.text = null;
        signupId.text = null;
        signupPassword.text = null;

        signupPanel.SetActive(false);
    }

    public void OnClickSandSignUp()
    {
        HTTPSever.SendSignUpRequest(signupName.text, signupId.text, signupPassword.text);
    }

    IEnumerator NetworkMassage()
    {
        massageImage.SetActive(true);

        yield return new WaitForSeconds(3f);

        massageImage.SetActive(false);

        OnClickLoginCancel();
        OnClickSignUpCancel();

        StopCoroutine(NetworkMassage());
    }

    /// <summary>
    /// Network ����� ���� ������ �޼����� �޼����� ��Ʈ ���� ������ �� �ֽ��ϴ�.
    /// </summary>
    /// <param name="massageLog"></param>
    /// <param name="logColor"></param>
    public void SetNetworkMassageLog(string massageLog, Color logColor)
    {
        networkMassage.text = massageLog;
        networkMassage.color = logColor;

        StartCoroutine(NetworkMassage());
    }

    void Update()
    {
        if (loginPanel.activeSelf || signupPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClickLoginCancel();
                OnClickSignUpCancel();
            }
        }

        CheckLoginReady();
        CheckSignUpReady();
    }

    public void CheckLoginReady()
    {
        if (loginPanel.activeSelf)
        {
            bool allEmpty = string.IsNullOrEmpty(loginId.text) ||
                        string.IsNullOrEmpty(loginPassword.text);

            if (allEmpty || massageImage.activeSelf) //inputField�� �ϳ��� ��� �ְų� �޼���â�� ������ false
            {
                sendLoginBT.interactable = false;
            }
            else
            {
                sendLoginBT.interactable = true;
            }
        }
    }

    public void CheckSignUpReady()
    {
        if (signupPanel.activeSelf)
        {
            bool allEmpty = string.IsNullOrEmpty(signupName.text) ||
                        string.IsNullOrEmpty(signupId.text) ||
                        string.IsNullOrEmpty(signupPassword.text);

            if (allEmpty || massageImage.activeSelf) //inputField�� �ϳ��� ��� �ְų� �޼���â�� ������ false
            {
                sendSignupBT.interactable = false;
            }
            else
            {
                sendSignupBT.interactable = true;
            }
        }
    }
}
