using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;


namespace Tests
{
    public class LoginUIManagerTests
    {
        private LoginUIManager LM;
        private static readonly string randomString = ",m;aw'oejrp82q3yoeifnvsd;vasdifhaewr";

        [OneTimeSetUp]
        public void Setup()
        {
            // Login UI Manager
            LM = new GameObject().AddComponent<LoginUIManager>();

            // Login Page Set Up
            LM.loginPage = new GameObject();
            LM.loginButton = new GameObject().AddComponent<Button>();
            LM.loginButton.gameObject.transform.SetParent(LM.loginPage.transform);
            LM.loginEmailField = new GameObject().AddComponent<InputField>();
            LM.loginEmailField.gameObject.transform.SetParent(LM.loginPage.transform);
            LM.loginPasswordField = new GameObject().AddComponent<InputField>();
            LM.loginPasswordField.gameObject.transform.SetParent(LM.loginPage.transform);
            LM.loginConfirmText = new GameObject().AddComponent<Text>();
            LM.loginConfirmText.gameObject.transform.SetParent(LM.loginPage.transform);
            LM.loginWarningText = new GameObject().AddComponent<Text>();
            LM.loginWarningText.gameObject.transform.SetParent(LM.loginPage.transform);

            // Register Page Set Up
            LM.registerPage = new GameObject();
            LM.toRegisterPageButton = new GameObject().AddComponent<Button>();
            LM.toRegisterPageButton.gameObject.transform.SetParent(LM.registerPage.transform);
            LM.registerEmailField = new GameObject().AddComponent<InputField>();
            LM.registerEmailField.gameObject.transform.SetParent(LM.registerPage.transform);
            LM.registerPasswordField = new GameObject().AddComponent<InputField>();
            LM.registerPasswordField.gameObject.transform.SetParent(LM.registerPage.transform);
            LM.registerPasswordVerifyField = new GameObject().AddComponent<InputField>();
            LM.registerPasswordVerifyField.gameObject.transform.SetParent(LM.registerPage.transform);
            LM.registerWarningText = new GameObject().AddComponent<Text>();
            LM.registerWarningText.gameObject.transform.SetParent(LM.registerPage.transform);

            // Menu Page Set Up
            LM.menuPage = new GameObject();
            LM.loadingConfirm = new GameObject().AddComponent<Text>();
            LM.loadingConfirm.gameObject.transform.SetParent(LM.menuPage.transform);
            LM.toAR_MRI_Button = new GameObject().AddComponent<Button>();
            LM.toAR_MRI_Button.gameObject.transform.SetParent(LM.menuPage.transform);

            // Setting Page Set Up
            LM.settingPage = new GameObject();
            LM.sequenceField = new GameObject().AddComponent<InputField>();
            LM.sequenceField.gameObject.transform.SetParent(LM.settingPage.transform);
            LM.toSettingPageButton = new GameObject().AddComponent<Button>();
            LM.toSettingPageButton.gameObject.transform.SetParent(LM.settingPage.transform);
            LM.saveConfirm = new GameObject().AddComponent<Text>();
            LM.saveConfirm.gameObject.transform.SetParent(LM.settingPage.transform);
            LM.currentSequence = new GameObject().AddComponent<Text>();
            LM.currentSequence.gameObject.transform.SetParent(LM.settingPage.transform);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator ToLoginPageTest()
        {
            LM.loginConfirmText.text = randomString;
            LM.loginEmailField.text = randomString;
            LM.loginPasswordField.text = randomString;
            LM.loginWarningText.text = randomString;
            SetPagesActive();

            LM.ToLogin();
            yield return null;

            Assert.IsTrue(PagesActive(true, false, false, false)
                       && LoginPageTextsFieldsAreEmpty());
        }

        [UnityTest]
        public IEnumerator ToRegisterPageTest()
        {
            LM.registerEmailField.text = randomString;
            LM.registerPasswordField.text = randomString;
            LM.registerPasswordVerifyField.text = randomString;
            LM.registerWarningText.text = randomString;
            LM.registerWarningText.color = Color.black;
            SetPagesActive();

            LM.ToRegister();
            yield return null;
            
            Assert.IsTrue(PagesActive(false, true, false, false)
                       && RegisterPageTextsFieldsAreEmpty()
                       && LM.registerWarningText.color == Color.red);
        }

        [UnityTest]
        public IEnumerator DefaultButtonTest()
        {
            LM.saveConfirm.text = randomString;
            LM.sequenceField.text = randomString;
            
            LM.DefaultButtonOnClick();
            yield return null;
            
            Assert.IsTrue(LM.saveConfirm.text == string.Empty
                       && LM.sequenceField.text == SequenceManager.DEFAULT);
        }

        [UnityTest]
        public IEnumerator UserLoginTest()
        {
            LM.loginConfirmText.text = randomString;
            LM.loginEmailField.text = randomString;
            LM.loginPasswordField.text = randomString;
            LM.loginWarningText.text = randomString;
            LM.toSettingPageButton.gameObject.SetActive(true);
            SetPagesActive();

            yield return LM.LoginSuccessfully("user_UID");
            
            Assert.IsTrue(PagesActive(false, false, true, false)
                       && LM.toSettingPageButton.gameObject.activeSelf == false
                       && LoginPageTextsFieldsAreEmpty());
        }

        [UnityTest]
        public IEnumerator AdminLoginTest()
        {
            LM.loginConfirmText.text = randomString;
            LM.loginEmailField.text = randomString;
            LM.loginPasswordField.text = randomString;
            LM.loginWarningText.text = randomString;
            LM.toSettingPageButton.gameObject.SetActive(false);
            SetPagesActive();

            yield return LM.LoginSuccessfully("8wpcdvNckNfGzCYyP18504SyL3K2");
            
            Assert.IsTrue(PagesActive(false, false, true, false)
                       && LM.toSettingPageButton.gameObject.activeSelf == true
                       && LoginPageTextsFieldsAreEmpty());
        }

        [UnityTest]
        public IEnumerator SignOutTest()
        {
            LM.loginConfirmText.text = randomString;
            LM.loginEmailField.text = randomString;
            LM.loginPasswordField.text = randomString;
            LM.loginWarningText.text = randomString;
            LM.toSettingPageButton.gameObject.SetActive(true);
            SetPagesActive();

            LM.SignOut();
            yield return null;
            
            Assert.IsTrue(PagesActive(true, false, false, false)
                       && LM.toSettingPageButton.gameObject.activeSelf == false
                       && LoginPageTextsFieldsAreEmpty());
        }

        [UnityTest]
        public IEnumerator ToSettingPageTest()
        {
            LM.sequenceField.text = randomString;
            LM.saveConfirm.text = randomString;
            LM.toSettingPageButton.gameObject.SetActive(false);
            SetPagesActive();

            LM.ToSetting();
            yield return null;
            
            Assert.IsTrue(PagesActive(false, false, false, true)
                       && LM.toSettingPageButton.gameObject.activeSelf == true
                       && LM.saveConfirm.text == string.Empty
                       && LM.sequenceField.text == string.Empty);
        }

        [UnityTest]
        public IEnumerator SettingPageToMenuPageTest()
        {
            LM.sequenceField.text = randomString;
            LM.saveConfirm.text = randomString;
            LM.toSettingPageButton.gameObject.SetActive(false);
            SetPagesActive();


            LM.SettingToMenu();
            yield return null;
            
            Assert.IsTrue(PagesActive(false, false, true, false)
                       && LM.toSettingPageButton.gameObject.activeSelf == true
                       && LM.saveConfirm.text == string.Empty
                       && LM.sequenceField.text == string.Empty);
        }

        private bool PagesActive(bool login, bool register, bool menu, bool setting)
        {
            return LM.loginPage.activeSelf == login
                && LM.registerPage.activeSelf == register
                && LM.menuPage.activeSelf == menu
                && LM.settingPage.activeSelf == setting;
        }

        private bool LoginPageTextsFieldsAreEmpty()
        {
            return LM.loginEmailField.text == string.Empty
                && LM.loginPasswordField.text == string.Empty
                && LM.loginConfirmText.text == string.Empty
                && LM.loginWarningText.text == string.Empty;
        }

        private bool RegisterPageTextsFieldsAreEmpty()
        {
            return LM.registerEmailField.text == string.Empty
                && LM.registerPasswordField.text == string.Empty
                && LM.registerPasswordVerifyField.text == string.Empty
                && LM.registerWarningText.text == string.Empty;
        }

        private void SetPagesActive()
        {
            LM.loginPage.SetActive(true);
            LM.registerPage.SetActive(true);
            LM.menuPage.SetActive(true);
            LM.settingPage.SetActive(true);
        }
    }
}
