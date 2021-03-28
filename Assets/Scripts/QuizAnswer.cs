using UnityEngine;
using UnityEngine.UI;


public class QuizAnswer : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;
    public Color startColor;
    public Button option1;
    public Button option2;

    private void Start()
    {
        startColor = GetComponent<Image>().color;
        
    }

    public void Answer()
    {

        if (isCorrect)
        {
            GetComponent<Image>().color = Color.green;
            Debug.Log("Correct answer");
            quizManager.Correct();
        }
        else
        {
            GetComponent<Image>().color = Color.red;
            Debug.Log("Wrong answer");
            quizManager.Wrong();
        }
    
        Invoke("OriginalColour", 0.5f);
    }

    // change the color of options back to the original colour
    public void OriginalColour()
    {
        GetComponent<Image>().color = startColor;
    }
}
