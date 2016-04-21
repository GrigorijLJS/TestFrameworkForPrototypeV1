/*Copyright 2016 OUNL

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
This project has received funding from the European Union’s Horizon
2020 research and innovation programme under grant agreement No 644187.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

/*public class MultipleChoiceQ : MonoBehaviour 
{*/

	/*public string question;
	public string answers [];
	public int correctAnswer;

	//the topic of the question; could be used in the "hinting"
	//ex. 3 errors in recognizing from the same topic - mention the topic
	public string topic;*/


	/*
	public struct Question {
		public string questionText;
		public string answerA;
		public string answerB;
		public string answerC;
		public string answerD;
		public int correctAnswerID;
	}
	
	[XmlRoot]
	public class QuestionData {
		[XmlArray("Questions")]
		[XmlArrayItem("Question")]
		public List<Question>
			questions = new List<Question>();
		
		public static QuestionData LoadFromText(string text) {
			try {
				XmlSerializer serializer = new XmlSerializer(typeof(QuestionData));            
				return serializer.Deserialize(new StringReader(text)) as QuestionData;
			} catch (Exception e) {
				UnityEngine.Debug.LogError("Exception loading question data: " + e);
				return null;
			}
		}
	}

public class QuestionManager : MonoBehaviour {
	[SerializeField]
	private TextAsset questionDataXMLFile;
	private QuestionData questionDataForFirstActivity;
	private Question currentQuestion;
	
	void Start() {
		questionDataForFirstActivity = QuestionData.LoadFromText(questionDataXMLFile.text);
	}
	
	// Call this when you want a new question
	public void SetNewQuestion(ref string question) {
		// gets a random question
		//System.Random r = new System.Random();
		int q = UnityEngine.Random.Range(0, questionDataForFirstActivity.questions.Count - 1);
		currentQuestion = questionDataForFirstActivity.questions[q];
		
		// add code here to set text values of your Question GameObject
		// e.g. GetComponent<SomeScript>().Text = currentQuestion.questionText;
		question = currentQuestion.questionText;
	}
	
	// Use this to see if user selected correct answer
	public bool CorrectAnswerSelected(int selectedAnswerID) {
		return selectedAnswerID == currentQuestion.correctAnswerID;
	}
}*/

//}
