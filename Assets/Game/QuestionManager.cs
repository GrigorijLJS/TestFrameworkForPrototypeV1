//based on these resources
//http://wiki.unity3d.com/index.php?title=Saving_and_Loading_Data:_XmlSerializer
//http://answers.unity3d.com/questions/639381/too-much-prefab-for-question-quiz-games.html

using System;
using System.IO;
using UnityEngine;
using System.Collections;

public class QuestionManager : MonoBehaviour {
	[SerializeField]
	/*private*/public TextAsset questionDataXMLFile;
	private QuestionData questionData;
	private Question currentQuestion;

	private int question_index;


	public QuestionManager()
	{
		question_index = 0;
		//questionData = QuestionData.LoadFromText(Application.dataPath+"/Game/questionDataXMLFile.xml");
		//questionData = QuestionData.LoadFromText(questionDataXMLFile.xml);
	}

	void Start() 
    {
		//questionData = QuestionData.LoadFromText(questionDataXMLFile.text);	
		//questionData = QuestionData.LoadFromText(Path.Combine(Application.dataPath,"questionDataXMLFile.xml"));
	}
	
	// Call this when you want a new question
	public bool NewQuestionOrNewActivity(ref string the_question) 
	{
		//the path to the XML file with the data about the questions
		questionData = QuestionData.Load(Path.Combine(Application.dataPath,"questionData.xml"));

		// gets a random question
		//int q = Random.Range(0, questionData.questions.Count);

        if ((question_index + 1) <= questionData.questions.Count)
        {
            //get a question
            currentQuestion = questionData.questions[question_index];

            // add code here to set text values of your Question GameObject
            // e.g. GetComponent<SomeScript>().Text = currentQuestion.questionText;
            the_question ="Question: " +currentQuestion.questionText + " \n\nChoices: " + currentQuestion.answer1 +
                "     "+currentQuestion.answer2
                + "     " + currentQuestion.answer3 + "     " + currentQuestion.answer4 + "     " + currentQuestion.answer5 
                + "     " +currentQuestion.answer6 + "     " + currentQuestion.answer7 + "     " + currentQuestion.answer8 
                + "     " +currentQuestion.answer9 + "     " + currentQuestion.answer10 + "\n\n"+"correct answer: " 
                + currentQuestion.correctAnswer+"\n";
        }

        if ((question_index + 1) <= questionData.questions.Count)
        {//still continue with this activity
            question_index++;
            return false;
        }
        /*else if ((question_index + 1) == questionData.questions.Count)
        {//still continue with this activity
            return false;
        }*/
        else if ((question_index + 1) > questionData.questions.Count)
        {//new activity
            return true;
        }
        else//new activity
            return true;
	}
	
	/*//Use this to see if user selected correct answer
	public bool CorrectAnswerSelected(int selectedAnswerID) {
		return selectedAnswerID == currentQuestion.correctAnswer;
	}*/


	public int CheckCorrectnessOfAnswer(string selectedAnswer, ref int new_points) 
	{
		//save the question's points and use them later to add/subtract points from the game score
		new_points=currentQuestion.questionScore;

		if(selectedAnswer.Equals(currentQuestion.correctAnswer, StringComparison.OrdinalIgnoreCase))
		{//correct answer, i.e. the strings are equal
			return 0;
		}
		else
		{//incorrect answer

			/*//error in recognizing happens if the two strings partly match???
            if (currentQuestion.correctAnswer.Contains(selectedAnswer))
                return true;
            else if (selectedAnswer.Contains(currentQuestion.correctAnswer))
                return true;
			//error in recall if the two strings are totally different??? 
			return false;*/

            //calculate the difference in the strings, i.e. the "distance" between them
            int differenceInStrings = ComputeDistance(currentQuestion.correctAnswer, selectedAnswer);

            int n = 0;//used to check if the answer is a number

            if (int.TryParse(selectedAnswer, out n) && int.TryParse(currentQuestion.correctAnswer, out n))
            {//if the answers (both correct one the one written by the player) are numbers
                if (differenceInStrings > 0 && differenceInStrings < 3)
                {
                    return 1;//error in recognizing happens if the two strings partly match 
                    //this case is to avoid cases like: correct answer is "23" and the writen answer is "Ys"
                    //the difference is 2 but the given answer is not a number at all - hence not close to the
                    //actual answer

                }
                else
                {
                    //otherwise it is an error in recall
                    return 2;
                }
            }
            else
            {//if the answer is not a number, i.e. its textual
                if (differenceInStrings > 0 && differenceInStrings <= 3 && selectedAnswer.Length > 3)
                {
                    return 1;//error in recognizing happens if the two strings partly match and the 
                    //lenght of the answer is bigger than 3

                }
                else
                {
                    //otherwise it is an error in recall
                    return 2;
                }
            }
            
		}
	}


    //based on the solution from: http://stackoverflow.com/questions/6944056/c-sharp-compare-string-similarity
    //from the comments: "The Damereau-Levenshein Distance algorithm calculates the number of letter additions,
    //subtractions, substitutions, and transpositions (swaps) necessary to convert one string to another. The 
    //lower the score, the more similar they are"
    public static int ComputeDistance(string correctAnswer, string selectedAnswer)
    {
        if (string.IsNullOrEmpty(correctAnswer))
        {
            if (string.IsNullOrEmpty(selectedAnswer))
                return 0;
            return selectedAnswer.Length;
        }

        if (string.IsNullOrEmpty(selectedAnswer))
        {
            return correctAnswer.Length;
        }

        int correctAnswerLength = correctAnswer.Length;
        int selectedAnswerLength = selectedAnswer.Length;
        int[,] differenceBetweenStrings = new int[correctAnswerLength + 1, selectedAnswerLength + 1];

        // initialize the top and right of the table to 0, 1, 2, ...
        for (int i = 0; i <= correctAnswerLength; differenceBetweenStrings[i, 0] = i++) ;
        for (int j = 1; j <= selectedAnswerLength; differenceBetweenStrings[0, j] = j++) ;

        for (int i = 1; i <= correctAnswerLength; i++)
        {
            for (int j = 1; j <= selectedAnswerLength; j++)
            {
                int cost = (selectedAnswer[j - 1] == correctAnswer[i - 1]) ? 0 : 1;
                int min1 = differenceBetweenStrings[i - 1, j] + 1;
                int min2 = differenceBetweenStrings[i, j - 1] + 1;
                int min3 = differenceBetweenStrings[i - 1, j - 1] + cost;
                differenceBetweenStrings[i, j] = Math.Min(Math.Min(min1, min2), min3);
            }
        }
        return differenceBetweenStrings[correctAnswerLength, selectedAnswerLength];
    }


}