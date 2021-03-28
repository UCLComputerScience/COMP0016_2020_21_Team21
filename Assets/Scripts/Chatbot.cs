/**
* (C) Copyright IBM Corp. 2015, 2020.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*
*/
#pragma warning disable 0649

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using IBM.Watson.SpeechToText.V1;
using IBM.Cloud.SDK;
using IBM.Cloud.SDK.Authentication.Iam;
using IBM.Cloud.SDK.Utilities;
using IBM.Cloud.SDK.DataTypes;
using IBM.Watson.TextToSpeech.V1;
using IBM.Watson.Assistant.V2;
using IBM.Watson.Assistant.V2.Model;


public class Chatbot : MonoBehaviour
{
    #region SPEECH TO TEXT
    [Space(10)]
    [Tooltip("The service URL (optional). This defaults to \"https://api.us-south.speech-to-text.watson.cloud.ibm.com\"")]
    [SerializeField]
    private string stt_url;
    [Tooltip("Text field to display the results of streaming.")]
    public Text ResultsField;
    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string stt_api;

    [Header("Parameters")]
    // https://www.ibm.com/watson/developercloud/speech-to-text/api/v1/curl.html?curl#get-model
    [Tooltip("The Model to use. This defaults to en-US_BroadbandModel")]
    [SerializeField]
    private string _recognizeModel;
    #endregion


    #region TEXT TO SPEECH
    [Space(10)]
    [Header("Text To Speech")]
    [Tooltip("The service URL (optional). This defaults to \"https://api.us-south.text-to-speech.watson.cloud.ibm.com\"")]
    [SerializeField]
    private string tts_url;
    [Tooltip("The IAM apikey.")]
    [SerializeField]
    private string tts_api;
    #endregion

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

    private int _recordingRoutine = 0;
    private string _microphoneID = null;
    private AudioClip _recording = null;
    private int _recordingBufferSize = 1;
    private int _recordingHZ = 22050;

    private SpeechToTextService stt_service;
    private TextToSpeechService tts_service;
    private AssistantService assistant_service;

    private string session_id;
    private bool firstMessage;
    private bool stop_listening = false;
    private bool session_created = false;

    private bool respondActive = false;

    void Start()
    {
        LogSystem.InstallDefaultReactors();
        Runnable.Run(CreateService());
    }

    private IEnumerator CreateService() 
    {
        // stt api
        if(string.IsNullOrEmpty(stt_api)) {
            throw new IBMException("Please provide API key for the Speech To Text service. ");
        }

        // tts api
        if(string.IsNullOrEmpty(tts_api)) {
            throw new IBMException("Please provide API key for the Text To Speech service. ");
        }

        // assistant api
        if(string.IsNullOrEmpty(assistant_api)) {
            throw new IBMException("Please provide API key for the Assistant service. ");
        }

        // Create credential and instantiate service
        IamAuthenticator stt_authenticator = new IamAuthenticator(apikey: stt_api);
        IamAuthenticator tts_authenticator = new IamAuthenticator(apikey: tts_api);
        IamAuthenticator assistant_authenticator = new IamAuthenticator(apikey: assistant_api);

        // Wait for tokendata
        while (!stt_authenticator.CanAuthenticate())
            yield return null;

        while (!tts_authenticator.CanAuthenticate())
            yield return null;

        while (!assistant_authenticator.CanAuthenticate())
            yield return null;

        // Create credential and instantiate service
        tts_service = new TextToSpeechService(tts_authenticator);
        stt_service = new SpeechToTextService(stt_authenticator);
        assistant_service = new AssistantService(assistant_version_date, assistant_authenticator);

        // Wait for tokendata
        if (!string.IsNullOrEmpty(tts_url))
        {
            tts_service.SetServiceUrl(tts_url);
        }

        if (!string.IsNullOrEmpty(stt_url))
        {
            stt_service.SetServiceUrl(stt_url);
        }

        if (!string.IsNullOrEmpty(assistant_url))
        {
            assistant_service.SetServiceUrl(assistant_url);
        }

        assistant_service.CreateSession(OnCreateSession, assistant_id);

        // creating session workspace
        while (!session_created)
        {
            yield return null;
        }

        Active = true;

        WelcomeMessage();
    }


    public bool Active
    {
        get { return stt_service.IsListening; }
        set
        {
            if (value && !stt_service.IsListening)
            {
                stt_service.RecognizeModel = (string.IsNullOrEmpty(_recognizeModel) ? "en-US_BroadbandModel" : _recognizeModel);
                stt_service.DetectSilence = true;
                stt_service.EnableWordConfidence = true;
                stt_service.EnableTimestamps = true;
                stt_service.SilenceThreshold = 0.01f;
                stt_service.MaxAlternatives = 1;
                stt_service.EnableInterimResults = true;
                stt_service.OnError = OnError;
                stt_service.InactivityTimeout = -1;
                stt_service.ProfanityFilter = false;
                stt_service.SmartFormatting = true;
                stt_service.SpeakerLabels = false;
                stt_service.WordAlternativesThreshold = null;
                stt_service.EndOfPhraseSilenceTime = null;
                stt_service.StartListening(OnRecognize, OnRecognizeSpeaker);
            }
            else if (!value && stt_service.IsListening)
            {
                stt_service.StopListening();
            }
        }
    }

    public void WelcomeMessage() 
    {
        firstMessage = true;
        var input = new MessageInput()
        {
            Text = "Hello"
        };

        CallTextToSpeech("Hi, Please try to aim the camera at the ground and tap the screen when the ground is detected");
        assistant_service.Message(OnMessage, assistant_id, session_id, input);
    }

    // getting result from assistant
    private void OnMessage(DetailedResponse<MessageResponse> response, IBMError error)
    {
        if (!firstMessage) 
        {
            Debug.Log("On message");
            string outputText2 = response.Result.Output.Generic[0].Text;
            ResultsField.text = outputText2;
            Debug.Log(outputText2);
            CallTextToSpeech(outputText2);
        }

        Debug.Log("first message.");

        firstMessage = false;
    }

    // getting voice input from user
    private void BuildSpokenRequest(string spokenText)
    {
        Debug.Log(spokenText);
        var input = new MessageInput()
        {
            Text = spokenText
        };

        assistant_service.Message(OnMessage, assistant_id, session_id, input);
    }

    public void CallTextToSpeech(string outputText)
    {
        Debug.Log("calling tts");
        ResultsField.text = outputText;
        byte[] synthesizeResponse = null;
        AudioClip clip = null;
        tts_service.Synthesize(
            callback: (DetailedResponse<byte[]> response, IBMError error) =>
            {
                synthesizeResponse = response.Result;
                clip = WaveFile.ParseWAV("myClip", synthesizeResponse);
                PlayClip(clip);
            },
            text: outputText,
            voice: "en-US_AllisonV3Voice",
            accept: "audio/wav"
        );
    }

    private void PlayClip(AudioClip clip)
    {
        Debug.Log("playing clip");
        if (Application.isPlaying && clip != null)
        {
            GameObject audioObject = new GameObject("AudioObject");
            AudioSource source = audioObject.AddComponent<AudioSource>();
            source.spatialBlend = 0.0f;
            source.loop = false;
            source.clip = clip;
            source.Play();

            // invoke record again
            Debug.Log("Invoke");
            if(respondActive)
            {
                Invoke("RecordAgain", source.clip.length);
            }
            Debug.Log("destroy");
            Destroy(audioObject, clip.length);
        }
    }

    public void setStart(bool setValue)
    {
        respondActive = setValue;
    }

    public void TryStart()
    {
        stop_listening = false;
        if (respondActive)
        {
            StartRecording();
        }
    }

    private void RecordAgain()
    {
        Debug.Log("Played Audio received from Watson Text To Speech");
        if (!stop_listening)
        {
            OnListen();
        }
    }

    private void OnListen()
    {
        Log.Debug("ExampleStreaming.OnListen", "Start();");

        Active = true;

        StartRecording();
    }


    private void OnError(string error)
    {
        Active = false;

        Log.Debug("ExampleStreamingError.OnError()", error);
    }

    private void OnCreateSession(DetailedResponse<SessionResponse> response, IBMError error)
    {
        Log.Debug("ExampleStreamingError.OnCreateSession()", response.Result.SessionId);
        session_id = response.Result.SessionId;
        session_created = true;
    }

    public void StartRecording()
    {
        Debug.Log("start recording");
        if (_recordingRoutine == 0)
        {
            UnityObjectUtil.StartDestroyQueue();
            _recordingRoutine = Runnable.Run(RecordingHandler());
        }
    }

    public void StopRecording()
    {
        Debug.Log("stopped recording");
        if (_recordingRoutine != 0)
        {
            Microphone.End(_microphoneID);
            Runnable.Stop(_recordingRoutine);
            _recordingRoutine = 0;
        }
    }

    private IEnumerator RecordingHandler()
    {
        Log.Debug("ExampleStreaming.RecordingHandler()", "devices: {0}", Microphone.devices);
        _recording = Microphone.Start(_microphoneID, true, _recordingBufferSize, _recordingHZ);
        yield return null;      // let _recordingRoutine get set..

        if (_recording == null)
        {
            StopRecording();
            yield break;
        }

        bool bFirstBlock = true;
        int midPoint = _recording.samples / 2;
        float[] samples = null;

        while (_recordingRoutine != 0 && _recording != null)
        {
            int writePos = Microphone.GetPosition(_microphoneID);
            if (writePos > _recording.samples || !Microphone.IsRecording(_microphoneID))
            {
                Log.Error("ExampleStreaming.RecordingHandler()", "Microphone disconnected.");

                StopRecording();
                yield break;
            }

            if ((bFirstBlock && writePos >= midPoint)
                || (!bFirstBlock && writePos < midPoint))
            {
                // front block is recorded, make a RecordClip and pass it onto our callback.
                samples = new float[midPoint];
                _recording.GetData(samples, bFirstBlock ? 0 : midPoint);

                AudioData record = new AudioData();
                record.MaxLevel = Mathf.Max(Mathf.Abs(Mathf.Min(samples)), Mathf.Max(samples));
                record.Clip = AudioClip.Create("Recording", midPoint, _recording.channels, _recordingHZ, false);
                record.Clip.SetData(samples, 0);

                stt_service.OnListen(record);

                bFirstBlock = !bFirstBlock;
            }
            else
            {
                // calculate the number of samples remaining until we ready for a block of audio, 
                // and wait that amount of time it will take to record.
                int remaining = bFirstBlock ? (midPoint - writePos) : (_recording.samples - writePos);
                float timeRemaining = (float)remaining / (float)_recordingHZ;

                yield return new WaitForSeconds(timeRemaining);
            }
        }
        yield break;
    }

    // to build spoken request
    private void OnRecognize(SpeechRecognitionEvent result)
    {
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    if (res.final && alt.confidence > 0)
                    {
                        StopRecording();
                        string text = alt.transcript;
                        ResultsField.text = text;
                        Debug.Log("Watson hears : " + text + " Confidence: " + alt.confidence);
                        BuildSpokenRequest(text);
                    }
                }
            }
        }
    }  

    private void OnRecognizeSpeaker(SpeakerRecognitionEvent result)
    {
        if (result != null)
        {
            foreach (SpeakerLabelsResult labelResult in result.speaker_labels)
            {
                Log.Debug("ExampleStreaming.OnRecognizeSpeaker()", string.Format("speaker result: {0} | confidence: {3} | from: {1} | to: {2}", labelResult.speaker, labelResult.from, labelResult.to, labelResult.confidence));
            }
        }
    }
}
