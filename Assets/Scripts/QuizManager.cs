using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QnA> QnA;
    public GameObject[] options;
    public int quesNo;

    public Text questionText;
    public Text score;
    public int totalQuestions = 0;
    public int scoreCount;

    public GameObject quizPanel;
    public GameObject gameOverPanel;


    private void Start()
    {
        totalQuestions = QnA.Count;
        gameOverPanel.SetActive(false);
        GenerateQuestion();
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<QuizAnswer>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<Text>().text = QnA[quesNo].answers[i];

            if(QnA[quesNo].correctAnswer == i+1)
            {
                options[i].GetComponent<QuizAnswer>().isCorrect = true;
            }
        }
    }

    void GenerateQuestion()
    {
        if (QnA.Count > 0) 
        {
            quesNo = Random.Range(0, QnA.Count);

            questionText.text = QnA[quesNo].question;
            SetAnswers();
        }
        else {
            Debug.Log("Out of questions");
            Invoke("GameOver", 0.8f);
        }
        

    }

    public void Correct()
    {
        scoreCount += 1;
        QnA.RemoveAt(quesNo);
        Invoke("GenerateQuestion", 0.5f);
        
    }

    public void Wrong()
    {
        QnA.RemoveAt(quesNo);
        Invoke("GenerateQuestion", 0.5f);
    }

    public void GameOver()
    {
        quizPanel.SetActive(false);
        gameOverPanel.SetActive(true);
        score.text = scoreCount + " / " + totalQuestions;
    }

    public void Retry()
    {
        Start();
    }
}
