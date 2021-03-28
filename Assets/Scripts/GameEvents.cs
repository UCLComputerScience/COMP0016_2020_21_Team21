using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public Chatbot chatbot;
    private AudioSource _audio;
    private int currentStep = 0;
    private List<int> sequence;

    [Header("MRI scan result")]
    public GameObject resultPage;

    [Header("Main Page")]
    public GameObject nextButton;

    public void InvokeEvent()
    {
        if(sequence == null)
        {
            ParseSequences();
        }
        if(currentStep + 1 == sequence.Count)
        {
            Debug.Log("No more actions");
            nextButton.SetActive(false);
        }
        switch(sequence[currentStep++])
        {
            case 1:
                chatbot.CallTextToSpeech("Welcome to the A R demo of MRI scan");
                Debug.Log("Welcome");
                break;
            case 2:
                chatbot.CallTextToSpeech("I will show you what an " +
                  "MRI room looks like, and explain the principles and precautions of MRI");
                Debug.Log("Introduction");
                break;
            case 3:
                chatbot.CallTextToSpeech("Please follow me");
                GameObject practitioner =  GameObject.Find("Med_Sum_Wom_RtStand_1025");
                practitioner.transform.localPosition = new Vector3(0.1f, 0.301f, 0.6f);
                Debug.Log("3");
                break;
            case 4:
                chatbot.CallTextToSpeech("Magnetic resonance imaging is a type of scan " +
                    "that uses strong magnetic fields and radio waves to produce detailed" +
                    " images of the inside of the body");
                Debug.Log("Explain MRI");
;                break;
            case 5:
                chatbot.CallTextToSpeech("An MRI scanner is a large tube that contains " +
                    "powerful magnets, like this one next to me. You lie inside the tube during the scan");
                Debug.Log("Explain MRI scanner 1");
                break;
            case 6:
                chatbot.CallTextToSpeech("As the MRI scanner produces strong magnetic fields," +
                  " it's important to remove any metal objects from your body");
                Debug.Log("Explain MRI scanner 2");
                break;
            case 7:
                chatbot.CallTextToSpeech("Now I will briefly show you how MRI scan is done");
                Debug.Log("Demonstration");
                break;
            case 8:
                GameObject.Find("TableTop").GetComponent<TableMove>().OnTableMove();
                Debug.Log("8");
                break;
            case 9:
                _audio = GameObject.Find("Chassis").GetComponent<AudioSource>();
                _audio.Play(0);
                Invoke("stopAudio", 7);
                Debug.Log("9");
                break;
            case 10:
                StartCoroutine(ShowResultImage());
                Debug.Log("10");
                break;
            case 11:
                Debug.Log("11");
                chatbot.setStart(true);
                chatbot.CallTextToSpeech("You can try to ask me some questions.");
                chatbot.StartRecording();
                break;
        }
    }

    private void ParseSequences()
    {
        string[] stringSequences = SequenceManager.sequence.Split(',');
        sequence = new List<int>();
        foreach(string i in stringSequences)
        {
            sequence.Add(int.Parse(i));
        }
    }

    private void stopAudio()
    {
        _audio.Stop();
    }

    private IEnumerator ShowResultImage()
    {
        chatbot.CallTextToSpeech("This is how an MRI scan image looks like");
        resultPage.SetActive(true);
        yield return new WaitForSeconds(5);
        resultPage.SetActive(false);
    }
}
