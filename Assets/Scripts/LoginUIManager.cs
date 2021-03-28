using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoginUIManager : MonoBehaviour
{
    public static LoginUIManager instance;

    [Header("Login Page")]
    public GameObject loginPage;
    public Button loginButton;
    public InputField loginEmailField;
    public InputField loginPasswordField;
    public Text loginConfirmText;
    public Text loginWarningText;

    [Header("Register Page")]
    public GameObject registerPage;
    public Button toRegisterPageButton;
    public InputField registerEmailField;
    public InputField registerPasswordField;
    public InputField registerPasswordVerifyField;
    public Text registerWarningText;

    [Header("Menu Page")]
    public GameObject menuPage;
    public Text loadingConfirm;
    public Button toAR_MRI_Button;

    [Header("Setting Page")]
    public GameObject settingPage;
    public InputField sequenceField;
    public Button toSettingPageButton;
    public Text saveConfirm;
    public Text currentSequence;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        if (FirebaseManager.auth != null)
        {
            CleanPages();
            menuPage.SetActive(true);
            toSettingPageButton.gameObject.SetActive(FirebaseManager.user.UserId == "8wpcdvNckNfGzCYyP18504SyL3K2");
        }
        else
        {
            ToLogin();
        }
    }

    public void ToLogin()
    {
        CleanPages();
        loginPage.SetActive(true);
        CleanLoginFields();
    }

    public void ToRegister()
    {
        CleanPages();
        registerPage.SetActive(true);
        CleanRegisterPage();
    }
    public void DefaultButtonOnClick()
    {
        sequenceField.text = SequenceManager.DEFAULT;
        saveConfirm.text = string.Empty;
    }

    public IEnumerator LoginSuccessfully(string UID)
    {
        toSettingPageButton.gameObject.SetActive(UID == "8wpcdvNckNfGzCYyP18504SyL3K2");
        loginWarningText.text = string.Empty;
        loginConfirmText.text = "Logging in...";
        yield return new WaitForSeconds(2);
        CleanPages();
        menuPage.SetActive(true);
        CleanLoginFields();
        loginButton.interactable = true;
        toRegisterPageButton.interactable = true;
    }

    public void SignOut()
    {
        ToLogin();
        toSettingPageButton.gameObject.SetActive(false);
    }

    public void ToSetting()
    {
        CleanPages();
        settingPage.SetActive(true);
        toSettingPageButton.gameObject.SetActive(true);
        sequenceField.text = string.Empty;
        saveConfirm.text = string.Empty;
        currentSequence.text = "current sequence: " + SequenceManager.sequence;
    }

    public void SettingToMenu()
    {
        CleanPages();
        sequenceField.text = string.Empty;
        saveConfirm.text = string.Empty;
        toSettingPageButton.gameObject.SetActive(true);
        menuPage.SetActive(true);
    }

    public void LoadAR_MRI()
    {
        toAR_MRI_Button.interactable = false;
        SceneManager.LoadScene("AR MRI");
        Debug.Log(SequenceManager.sequence);
    }

    private void CleanLoginFields()
    {
        loginEmailField.text = string.Empty;
        loginPasswordField.text = string.Empty;
        loginWarningText.text = string.Empty;
        loginConfirmText.text = string.Empty;
    }

    private void CleanPages()
    {
        loginPage.SetActive(false);
        menuPage.SetActive(false);
        settingPage.SetActive(false);
        registerPage.SetActive(false);
    }

    private void CleanRegisterPage()
    {
        registerEmailField.text = string.Empty;
        registerPasswordField.text = string.Empty;
        registerPasswordVerifyField.text = string.Empty;
        registerWarningText.text = string.Empty;
        registerWarningText.color = Color.red;
    }
}
