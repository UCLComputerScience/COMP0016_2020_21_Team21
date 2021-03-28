using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;


namespace Tests
{
    public class ARMRIRoomUIManagerTests
    {
        private ARMRIRoomUIManager AM;

        [OneTimeSetUp]
        public void Setup()
        {
            // AR MRI Room UI Manager
            AM = new GameObject().AddComponent<ARMRIRoomUIManager>();

            // Room Page Set Up
            AM.roomPage = new GameObject();

            // FAQ Page
            AM.faqPage = new GameObject();

            // Quiz Page
            AM.quizPage = new GameObject();

            // Text Chatbot
            AM.textChatbotPage = new GameObject();

            // To Room Button
            AM.toMainButton = new GameObject();

            // Cameras
            AM.ARCamera = new GameObject();
            AM.myCamera = new GameObject();

            // Result Page
            AM.resultPage = new GameObject();
        }

        [UnityTest]
        public IEnumerator ToMainRoomPageTest()
        {
            SetObjectsActive();

            AM.ToRoomPage();
            yield return null;

            Assert.IsTrue(ObjectsActiveTest(true, false, false, false, false, true, false, false));
        }

        [UnityTest]
        public IEnumerator ToQuizPageTest()
        {
            SetObjectsActive();

            AM.ToQuizPage();
            yield return null;

            Assert.IsTrue(ObjectsActiveTest(false, false, true, false, false, false, true, true));
        }

        [UnityTest]
        public IEnumerator ToTextChatbotPageTest()
        {
            SetObjectsActive();

            AM.ToTextChatbotPage();
            yield return null;

            Assert.IsTrue(ObjectsActiveTest(false, false, false, false, true, true, false, true));
        }

        [UnityTest]
        public IEnumerator ToFQAPageTest()
        {
            SetObjectsActive();

            AM.ToFQAPage();
            yield return null;

            Assert.IsTrue(ObjectsActiveTest(false, false, false, true, false, false, true, true));
        }

        private void SetObjectsActive()
        {
            AM.roomPage.SetActive(true);
            AM.resultPage.SetActive(true);
            AM.quizPage.SetActive(true);
            AM.faqPage.SetActive(true);
            AM.textChatbotPage.SetActive(true);
            AM.ARCamera.SetActive(true);
            AM.myCamera.SetActive(true);
            AM.toMainButton.SetActive(true);
        }

        private bool ObjectsActiveTest(bool roomPage,
                                       bool resultPage,
                                       bool quizPage,
                                       bool faqPage,
                                       bool textChatbotPage,
                                       bool ARCamera,
                                       bool myCamera,
                                       bool toMainButton)
        {
            return AM.roomPage.activeSelf == roomPage
                && AM.resultPage.activeSelf == resultPage
                && AM.quizPage.activeSelf == quizPage
                && AM.faqPage.activeSelf == faqPage
                && AM.textChatbotPage.activeSelf == textChatbotPage
                && AM.ARCamera.activeSelf == ARCamera
                && AM.myCamera.activeSelf == myCamera
                && AM.toMainButton.activeSelf == toMainButton;
        }
    }
}
