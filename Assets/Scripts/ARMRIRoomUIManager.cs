﻿using UnityEngine;
using UnityEngine.SceneManagement;


public class ARMRIRoomUIManager : MonoBehaviour
{
    [Header("AR MRI Room")]
    public GameObject roomPage;

    [Header("Text Chatbot")]
    public GameObject textChatbotPage;

    [Header("Quiz")]
    public GameObject quizPage;

    [Header("FAQ")]
    public GameObject faqPage;

    [Header("MRI scan result")]
    public GameObject resultPage;

    [Header("Cameras")]
    public GameObject ARCamera;
    public GameObject myCamera;

    [Header("Buttons")]
    public GameObject toMainButton;

    void Start()
    {
        ToRoomPage();
    }

    public void ToMenuPage()
    {
        SceneManager.LoadScene("Log in");
    }

    public void ToRoomPage()
    {
        CleanPages();
        SwitchToARCamera();
        roomPage.SetActive(true);
        toMainButton.SetActive(false);
    }

    public void ToQuizPage()
    {
        CleanPages();
        SwitchToNormalCamera();
        quizPage.SetActive(true);
        toMainButton.SetActive(true);
    }

    public void ToTextChatbotPage()
    {
        CleanPages();
        SwitchToARCamera();
        textChatbotPage.SetActive(true);
        toMainButton.SetActive(true);
    }
    public void ToFQAPage()
    {
        CleanPages();
        SwitchToNormalCamera();
        faqPage.SetActive(true);
        toMainButton.SetActive(true);
    }

    private void CleanPages()
    {
        roomPage.SetActive(false);
        textChatbotPage.SetActive(false);
        quizPage.SetActive(false);
        faqPage.SetActive(false);
        resultPage.SetActive(false);
    }

    private void SwitchToNormalCamera()
    {
        ARCamera.SetActive(false);
        myCamera.SetActive(true);
    }

    private void SwitchToARCamera()
    {
        ARCamera.SetActive(true);
        myCamera.SetActive(false);
    }
}