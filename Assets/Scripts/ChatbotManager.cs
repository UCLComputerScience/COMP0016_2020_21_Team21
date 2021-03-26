using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Watson.Assistant.V2;
using IBM.Watson.Assistant.V2.Model;


public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;
}

public enum MessageType
{
    User,
    Assistant
}

public class ChatbotManager : MonoBehaviour
{
    #region ASSISTANT
    [Space(10)]
    [Header("Assistant")]
    [Tooltip("The service URL (optional). This defaults to \"https://api.us-south.assistant.watson.cloud.ibm.com\"")]
    [SerializeField]
    private string assistant_url;
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string assistant_api;
    [SerializeField]
    private string assistant_id;
    [SerializeField]
    private string assistant_version_date;
    #endregion

    private AssistantService assistant_service;
    private string session_id;
    private bool session_created = false;
    private bool firstMessage;

    List <Message> Messages = new List<Message>();

    public GameObject chatPanel, textGameObject;
    public InputField chatBox;
    public Color userColor, assistantColor;
    public GameObject scrollView;
    
    // Start is called before the first frame update
    void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            UpdataNewText();
        }
    }

    public void UpdataNewText()
    {
        var input = new MessageInput()
        {
            Text = chatBox.text
        };
        AddMessage(chatBox.text + "\n", MessageType.User);
        assistant_service.Message(OnMessage, assistant_id, session_id, input);
        chatBox.text = "";
    }

    private IEnumerator CreateService()
    {
        if(string.IsNullOrEmpty(assistant_api)) {
            throw new IBMException("Please provide API key for the Assistant service. ");
        }

        IamAuthenticator assistant_authenticator = new IamAuthenticator(apikey: assistant_api);

        while (!assistant_authenticator.CanAuthenticate())
            yield return null;

        assistant_service = new AssistantService(assistant_version_date, assistant_authenticator);

        Debug.Log("Done authenticating");

        if (!string.IsNullOrEmpty(assistant_url))
        {
            assistant_service.SetServiceUrl(assistant_url);
        }

        assistant_service.CreateSession(OnCreateSession, assistant_id);

        while (!session_created)
        {
            yield return null;
        }

        WelcomeMessage();
        
    }

    // display message
    public void AddMessage(string input, MessageType messageType) 
    {
        if (Messages.Count >= 25)
        {
        //Remove when too many.
        Destroy(Messages[0].textObject.gameObject);
        Messages.Remove(Messages[0]);
        }

        var newMessage = new Message { text = input };

        var newText = Instantiate(textGameObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = input;
        newMessage.textObject.color = messageType == MessageType.User ? userColor : assistantColor;

        Messages.Add(newMessage);
    }

    private void WelcomeMessage() 
    {
        AddMessage("Hi, how can I help you? \n", MessageType.Assistant);
        firstMessage = true;
        var input = new MessageInput()
        {
            Text = "Hello"
        };
        assistant_service.Message(OnMessage, assistant_id, session_id, input);
    }

    private void OnMessage(DetailedResponse<MessageResponse> response, IBMError error)
    {
        if (!firstMessage) 
        {
            string outputText2 = response.Result.Output.Generic[0].Text;
            AddMessage(outputText2 + "\n", MessageType.Assistant); 
        }

        firstMessage = false;
    }
    
    private void OnCreateSession(DetailedResponse<SessionResponse> response, IBMError error)
    {
        Log.Debug("ExampleStreamingError.OnCreateSession()", response.Result.SessionId);
        session_id = response.Result.SessionId;
        session_created = true;
    }
}
