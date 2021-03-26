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
        CleanLoginPages();
        if (FirebaseManager.auth != null)
        {
            menuPage.SetActive(true);
            if (FirebaseManager.user.UserId == "8wpcdvNckNfGzCYyP18504SyL3K2")
            {
                toSettingPageButton.gameObject.SetActive(true);
            }
        }
        else
        {
            loginPage.SetActive(true);
        }
    }

    public void RegisterBackToLogin()
    {
        CleanLoginPages();
        loginPage.SetActive(true);
        CleanRegisterPage();
    }

    public void LoginToRegister()
    {
        CleanLoginPages();
        registerPage.SetActive(true);
        CleanLoginFields();
    }
    public void DefaultButtonOnClick()
    {
        sequenceField.text = SequenceManager.DEFAULT;
        saveConfirm.text = "";
    }

    public IEnumerator LoginSuccessfully(string UID)
    {
        if(UID == "8wpcdvNckNfGzCYyP18504SyL3K2")
        {
            toSettingPageButton.gameObject.SetActive(true);
        }
        loginWarningText.text = "";
        loginConfirmText.text = "Logging in...";
        yield return new WaitForSeconds(2);
        CleanLoginPages();
        menuPage.SetActive(true);
        CleanLoginFields();
        loginButton.interactable = true;
        toRegisterPageButton.interactable = true;
    }

    public void SignOut()
    {
        CleanLoginPages();
        toSettingPageButton.gameObject.SetActive(false);
        loginPage.SetActive(true);
    }

    public void SetLoginButtonInteractable(bool value)
    {
        loginButton.interactable = value;
    }

    public void SetToRegisterPageButtonInteractable(bool value)
    {
        toRegisterPageButton.interactable = value;
    }

    public void MenuToSetting()
    {
        CleanLoginPages();
        settingPage.SetActive(true);
        currentSequence.text = "current sequence: " + SequenceManager.sequence;
    }

    public void SettingToMenu()
    {
        CleanLoginPages();
        sequenceField.text = "";
        saveConfirm.text = "";
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
        loginEmailField.text = "";
        loginPasswordField.text = "";
        loginWarningText.text = "";
        loginConfirmText.text = "";
    }

    private void CleanLoginPages()
    {
        loginPage.SetActive(false);
        menuPage.SetActive(false);
        settingPage.SetActive(false);
        registerPage.SetActive(false);
    }

    private void CleanRegisterPage()
    {
        registerEmailField.text = "";
        registerPasswordField.text = "";
        registerPasswordVerifyField.text = "";
        registerWarningText.text = "";
        registerWarningText.color = Color.red;
    }
}
