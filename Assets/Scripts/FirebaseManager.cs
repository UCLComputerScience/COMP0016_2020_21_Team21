using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    [Header("Frebase")]
    public DependencyStatus dependencyStatus;
    public static FirebaseAuth auth;
    public static FirebaseUser user;
    public DatabaseReference DB_Reference;

    [Header("Login Page")]
    public InputField loginEmailField;
    public InputField loginPasswordField;
    public Text loginWarningText;
    public Text loginConfirmText;

    [Header("Register Page")]
    public InputField registerEmailField;
    public InputField registerPasswordField;
    public InputField registerpasswordVerifyField;
    public Text registerWarningText;

    [Header("Setting Page")]
    public InputField sequenceField;
    public Text saveConfirm;
    public Text currentSequence;

    void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if(dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        if(auth == null)
        {
            auth = FirebaseAuth.DefaultInstance;
        }
        DB_Reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void LoginButtonOnClick()
    {
        StartCoroutine(Login());
    }

    public void RegisterButtonOnClick()
    {
        StartCoroutine(CreateAccount());
    }

    public void SaveButtonOnClick()
    {
        StartCoroutine(SaveAndCheck());
    }

    private IEnumerator SaveAndCheck()
    {
        Debug.Log("1");
        yield return StartCoroutine(UpdataDemonstrationSequence(sequenceField.text));
        Debug.Log("2");
        yield return StartCoroutine(GetDemonstrationSequence());
        Debug.Log("3");
        StartCoroutine(CheckSavedSuccessfully());
    }

    private IEnumerator CheckSavedSuccessfully()
    {
        if(sequenceField.text == SequenceManager.sequence)
        {
            saveConfirm.color = Color.green;
            saveConfirm.text = "Saved successfully!";
        }
        else
        {
            saveConfirm.color = Color.red;
            saveConfirm.text = "Save failed, please try again";
        }
        currentSequence.text = "current sequence: " + SequenceManager.sequence;
        yield return new WaitForSeconds(1.5f);
        saveConfirm.text = "";
    }

    private IEnumerator CreateAccount()
    {
        if(registerPasswordField.text != registerpasswordVerifyField.text)
        {
            registerWarningText.text = "Password does not match!";
        }
        else
        {
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(registerEmailField.text, registerpasswordVerifyField.text);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);
            if(RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException e = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError) e.ErrorCode;

                string message = "Register Failed! ;(";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email already in use";
                        break;
                    case AuthError.InvalidEmail:
                        message = "Invalid email";
                        break;
                }
                registerWarningText.color = Color.red;
                registerWarningText.text = message;
            }
            else
            {
                registerWarningText.color = Color.green;
                registerWarningText.text = "Registration successful!";
                yield return new WaitForSeconds(1);
                LoginUIManager.instance.RegisterBackToLogin();
            }
        }
    }

    private IEnumerator Login()
    {
        LoginUIManager.instance.SetLoginButtonInteractable(false);
        LoginUIManager.instance.SetToRegisterPageButtonInteractable(false);

        string email = loginEmailField.text;
        string password = loginPasswordField.text;

        var LoginTask = auth.SignInWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        if(LoginTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException e = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError) e.ErrorCode;

            string message = "Login Failed! ;(";
            switch(errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing password";
                    break;
                case AuthError.WrongPassword:
                    message = "Incorrect username or password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid email";
                    break;
                case AuthError.UserNotFound:
                    message = "Incorrect username or password";
                    break;
            }
            loginWarningText.text = message;
            LoginUIManager.instance.SetLoginButtonInteractable(true);
            LoginUIManager.instance.SetToRegisterPageButtonInteractable(true);
        }
        else
        {
            user = LoginTask.Result;
            StartCoroutine(LoginUIManager.instance.LoginSuccessfully(user.UserId));
            StartCoroutine(GetDemonstrationSequence());
        }
    }

    public void SignOutButtonOnClick()
    {
        auth.SignOut();
        LoginUIManager.instance.SignOut();
        SequenceManager.sequence = SequenceManager.DEFAULT;
    }

    private IEnumerator UpdataDemonstrationSequence(string sequence)
    {
        var DBTask = DB_Reference.Child("Sequences").Child("AR MRI").SetValueAsync(sequence);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        // else AR MRI sequence is now updated
    }

    private IEnumerator GetDemonstrationSequence()
    {
        var DBTask = DB_Reference.Child("Sequences").Child("AR MRI").GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            SequenceManager.sequence = SequenceManager.DEFAULT;
            StartCoroutine(UpdataDemonstrationSequence(SequenceManager.sequence));
        }
        else
        {
            SequenceManager.sequence = DBTask.Result.Value.ToString();
        }
    }
}
