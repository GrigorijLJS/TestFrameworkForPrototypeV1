using UnityEngine;
using System.Collections;

//for UI Text, input fields etc.
using UnityEngine.UI;



namespace Prototype1v1
{

    public class Timer : MonoBehaviour
    {

        //public int time;
		private Text timer;
		private Text hint;

        //private DiagnosticRules rulesObject = new DiagnosticRules();

		public MainScript mainScriptObj = new MainScript();


        public GameObject text_object;

		public GameObject hintTextObj;

		public GameObject QuestionManager;

		public string would_be_hint = "";
		public string the_question= "";


		private string current_activity_command = "";

		private string ongoing_learning_activity_name = "";

		//private bool last_learning_activity_limit_reached = false;

        //float time = 0;

		
		private ActivityMetrics quiz_activityObj = new ActivityMetrics();
		private string quiz_activity_name = "QuizActivity";
		
		private ActivityMetrics classification_activityObj = new ActivityMetrics();
		private string classification_activity_name = "ClassificationActivity";

		private ActivityMetrics temp_activity_container;

        public InputField inputField;
        public InputField inputField2; 

		
		private QuestionManager QManagObj = new QuestionManager();
		private bool new_question = false;
		private int new_points = 0;


        string activity_timer_start = "start";
        string activity_timer_pause = "pause";
        string activity_timer_unpause = "unpause";
        string activity_timer_stop = "stop";

        void Start()
        {
//            //so the objects of metrics are the same
//			mainScriptObj.rulesObject.playerMetricsObject = mainScriptObj.playerMetricsObject;

            timer = text_object.GetComponent<Text>();
			hint = hintTextObj.GetComponent<Text>();


			
			mainScriptObj = GameObject.Find("GameSystem").GetComponent<MainScript>();

			mainScriptObj.playerMetricsObject.timeMetricsObject.StartAndStopTimeCounterSinceBeginningOfGame(true);

			/*mainScriptObj.playerMetricsObject.timeMetricsObject.CheckAndStoreInputGameTimeLimit(0, 0, 3);
			mainScriptObj.playerMetricsObject.timeMetricsObject.CheckAndStoreInputGameTimeLimit(0, 0, 5);*/



			//first activity
			if(!mainScriptObj.playerMetricsObject.gameActivitiesList.ContainsKey(quiz_activity_name))
			{
				
				mainScriptObj.playerMetricsObject.gameActivitiesList.Add(quiz_activity_name,quiz_activityObj);
			}

			if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name, out temp_activity_container))
			{
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 2);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 4);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 6);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 8);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 10);
				
				/*temp_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(2);
				temp_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(3);
				temp_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(4);*/

                ErrorMetrics temp_err_cont;
                temp_activity_container.EncouteredErrorsList.TryGetValue(temp_activity_container.errorRecognizeName
                    , out temp_err_cont);
                temp_err_cont.StoreInputThresholdsToNumberOfErrors(1);
                temp_err_cont = null;

                temp_activity_container.EncouteredErrorsList.TryGetValue(temp_activity_container.errorRecallName
                    , out temp_err_cont);
                temp_err_cont.StoreInputThresholdsToNumberOfErrors(2);
                temp_err_cont = null;

                temp_activity_container.EncouteredErrorsList.TryGetValue(temp_activity_container.errorImplementName
                    , out temp_err_cont);
                temp_err_cont.StoreInputThresholdsToNumberOfErrors(3);
                temp_err_cont = null;
			}

			//second activity
			if(!mainScriptObj.playerMetricsObject.gameActivitiesList.ContainsKey(classification_activity_name))
			{
				
				mainScriptObj.playerMetricsObject.gameActivitiesList.Add(classification_activity_name,classification_activityObj);
			}
			
			if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue
                (classification_activity_name, out temp_activity_container))
			{
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 3);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 5);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 7);


                ErrorMetrics temp_error_cont;
                temp_activity_container.EncouteredErrorsList.TryGetValue(temp_activity_container.errorClassifyName
                    , out temp_error_cont);
                temp_error_cont.StoreInputThresholdsToNumberOfErrors(4);
                temp_error_cont = null;

                temp_activity_container.EncouteredErrorsList.TryGetValue(temp_activity_container.errorImplementName
                    , out temp_error_cont);
                temp_error_cont.StoreInputThresholdsToNumberOfErrors(3);
                temp_error_cont = null;

				/*temp_learning_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(2);
				temp_learning_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(3);
				temp_learning_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(4);*/
			}

			temp_activity_container=null;

            //disable the second input field for now
            inputField2.DeactivateInputField();
            inputField2.interactable = false;

			//get a new question
			QManagObj.NewQuestionForTheFirstActivity(ref the_question);

            //first task
            if (mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name,
                   out temp_activity_container) &&
                   temp_activity_container.activity_completed == false)
            {
                CheckForStartOfActivity(ref temp_activity_container, activity_timer_start);
                //temp_learning_activity_container=null;
                ongoing_learning_activity_name = quiz_activity_name;
            }

            if (ongoing_learning_activity_name == quiz_activity_name)
            {
                inputField.onEndEdit.AddListener(delegate
                {
                    new_points = 0;

                    //if the following int is 0, the answer is correct, if it is 1 error in recognizing happened
                    //if it is 2 error in recall was made, and 3 is for error in implementing 
                    int corectAnswerOrTypeOfError = QManagObj.CheckCorrectnessOfAnswer(inputField.text, ref new_points);

                    if (corectAnswerOrTypeOfError == 0)
                    {
                        //the player gets some points
                        mainScriptObj.the_score += new_points;

                        //update the diagnostic system about the score changes
                        mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                        //and check the rules for the score
                        mainScriptObj.CheckTheGameScoreAndItsRules();

                        DebugInput(inputField.text);
                        inputField.text = "";
                    }
                    else
                    {//incorrect answer
                        //the player loses some points
                        mainScriptObj.the_score -= new_points;

                        //update the diagnostic system about the score changes
                        mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                        //and check the rules for the score
                        mainScriptObj.CheckTheGameScoreAndItsRules();

                        DebugInput("WAAAAAAAAAAH!!!" + inputField.text);
                        //mainScriptObj.CheckForErrors();

                        //check the type of the error
                        //if it is 1 error in recognizing happened, ID: errorInRecognizing
                        //if it is 2 error in recall was made, ID: errorInRecalling 

                        ErrorMetrics temp_error_for_first;
                        if (corectAnswerOrTypeOfError == 1)
                        {

                            temp_activity_container.EncouteredErrorsList
                                .TryGetValue(temp_activity_container.errorRecognizeName, out temp_error_for_first);
                            temp_error_for_first.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                            string errorInfo = "ErrorInRecognizing timestamps: " +
                                temp_error_for_first.error_time_stamps.Count + "#  ";
                            for (int i = 0; i < temp_error_for_first.error_time_stamps.Count; i++)
                            {
                                errorInfo += temp_error_for_first.error_time_stamps[i] + "  ";
                            }
                            Debug.LogError(errorInfo);


                            mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container,ref would_be_hint);
                            hint.text = would_be_hint;
                            //string errorinfo = errore + " | " + temp_container.getnumberoferrors() + " timestamps: ";
                            //for (int i = 0; i < temp_container.getnumberoferrors(); i++)
                            //{
                            //    errorinfo += temp_container.error_time_stamps.elementat(i) + "  ";
                            //}
                            //debug.logerror(errorinfo);
                        }
                        else if (corectAnswerOrTypeOfError==2)
                        {
                            temp_activity_container.EncouteredErrorsList
                                .TryGetValue(temp_activity_container.errorRecallName, out temp_error_for_first);
                            temp_error_for_first.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                            string errorInfo = "ErrorInRecalling timestamps: " 
                                + temp_error_for_first.error_time_stamps.Count + "#  ";
                            for (int i = 0; i < temp_error_for_first.error_time_stamps.Count; i++)
                            {
                                errorInfo += temp_error_for_first.error_time_stamps[i] + "  ";
                            }
                            Debug.LogError(errorInfo);

                            mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                            hint.text = would_be_hint;
                        }
                        else if (corectAnswerOrTypeOfError == 3)
                        {
                            temp_activity_container.EncouteredErrorsList
                                .TryGetValue(temp_activity_container.errorImplementName, out temp_error_for_first);
                            temp_error_for_first.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                            string errorInfo = "errorInImplementing timestamps: "
                                + temp_error_for_first.error_time_stamps.Count + "#  ";
                            for (int i = 0; i < temp_error_for_first.error_time_stamps.Count; i++)
                            {
                                errorInfo += temp_error_for_first.error_time_stamps[i] + "  ";
                            }
                            Debug.LogError(errorInfo);

                            mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                            hint.text = would_be_hint;
                        }

                        inputField.text = "";
                    }
                    //get a new question
                    if (QManagObj.NewQuestionForTheFirstActivity(ref the_question))
                    {
                        //reset the question var to empty
                        //the_question = "";

                        //wait a bit and start the new activity

                        //move the first input field to a place where it cannot be seen 
                        Vector3 temp = new Vector3(1000.0f, 0, 0);
                        inputField.transform.position += temp;

                        the_question = "";
                        the_question = "New activity will start soon! Please wait...\n\n\n\n\n";

                        if (mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name,
                           out temp_activity_container) &&
                           temp_activity_container.activity_completed == false)//if the first activity is not finished
                        {//but the requirements are met


                            CheckForStartOfActivity(ref temp_activity_container, activity_timer_stop);//end the first activity
                            temp_activity_container = null;//and empty the container
                        }

                        //deactivate the process of getting input during the "transfer"
                        //from one activity to the other
                        inputField.DeactivateInputField();
                        inputField.interactable = false;

                        //wait a bit
                        StartCoroutine(WaitFunction());
                    }

                });
            }
            /*else if (ongoing_learning_activity_name==classification_activity_name)
            {

                inputField2.onEndEdit.AddListener(delegate
                {
                    DebugInput("Yaaaar!");
                });
            }*/
        }

        void SecondActivity()
        {
            if (mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name,
                   out temp_activity_container) &&
                   temp_activity_container.activity_completed)//if the first activity is finished
            {//start preparing for the second one


                DebugInput("Chaos sighted!");
                CheckForStartOfActivity(ref temp_activity_container, activity_timer_stop);//end the first activity
                temp_activity_container = null;//empty the container

                //and extract the second activity
                mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(classification_activity_name,
                   out temp_activity_container);
                temp_activity_container.StartStopOrPauseTimeOnActivityCounter(activity_timer_start);

                CheckForStartOfActivity(ref temp_activity_container, activity_timer_start);
                //temp_learning_activity_container=null;
                ongoing_learning_activity_name = classification_activity_name;

                //StartCoroutine(WaitFunction());
                //System.Threading.Thread.Sleep(3000);
                //Invoke("WaitFunction", 5.0f);

                /*inputField.ActivateInputField();
                inputField.interactable = true;*/

                //enable the second input field
                inputField2.ActivateInputField();
                inputField2.interactable = true;

                QManagObj.NewQuestionForTheSecondActivity(ref the_question);

                inputField2.onEndEdit.AddListener(delegate
                {
                    new_points = 0;

                    //if the following int is 0, the answer is correct, if it is 1 error in recognizing happened
                    //if it is 2 error in recall was made
                    int corectAnswerOrTypeOfError = QManagObj.CheckCorrectnessOfAnswerForSecond
                        (inputField2.text, ref new_points);

                    if (corectAnswerOrTypeOfError == 0)
                    {
                        //the player gets some points
                        mainScriptObj.the_score += new_points;

                        //update the diagnostic system about the score changes
                        mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                        //and check the rules for the score
                        mainScriptObj.CheckTheGameScoreAndItsRules();

                        DebugInput(inputField2.text);
                        inputField2.text = "";
                    }
                    else
                    {//incorrect answers
                        if (corectAnswerOrTypeOfError == 1)
                        {
                            //the player loses some points
                            mainScriptObj.the_score -= new_points;

                            //update the diagnostic system about the score changes
                            mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                            //and check the rules for the score
                            mainScriptObj.CheckTheGameScoreAndItsRules();

                            DebugInput("Red 'unz Go Fasta!!!!" + inputField2.text);
                            //mainScriptObj.CheckForErrors();

                            //check the type of the error

                            ErrorMetrics temp_error_for_second;
                            temp_activity_container.EncouteredErrorsList.TryGetValue(
                                temp_activity_container.errorClassifyName, out temp_error_for_second);
                            temp_error_for_second.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                            string errorInfo = "errorInClassifying timestamps: " +
                                temp_error_for_second.error_time_stamps.Count + "#  ";
                            for (int i = 0; i < temp_error_for_second.error_time_stamps.Count; i++)
                            {
                                errorInfo += temp_error_for_second.error_time_stamps[i] + "  ";
                            }
                            Debug.LogError(errorInfo);

                            mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                            hint.text = would_be_hint;

                            inputField2.text = "";
                        }
                        else if(corectAnswerOrTypeOfError == 2)
                        {
                            //the player loses some points
                            mainScriptObj.the_score -= new_points;

                            //update the diagnostic system about the score changes
                            mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                            //and check the rules for the score
                            mainScriptObj.CheckTheGameScoreAndItsRules();

                            DebugInput("Red 'unz Go Fasta!!!!" + inputField2.text);
                            //mainScriptObj.CheckForErrors();

                            //check the type of the error

                            ErrorMetrics temp_error_for_second;
                            temp_activity_container.EncouteredErrorsList.TryGetValue(
                                temp_activity_container.errorImplementName, out temp_error_for_second);
                            temp_error_for_second.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                            string errorInfo = "errorInImplementing timestamps: " +
                                temp_error_for_second.error_time_stamps.Count + "#  ";
                            for (int i = 0; i < temp_error_for_second.error_time_stamps.Count; i++)
                            {
                                errorInfo += temp_error_for_second.error_time_stamps[i] + "  ";
                            }
                            Debug.LogError(errorInfo);

                            mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                            hint.text = would_be_hint;

                            inputField2.text = "";
                        }
                    }
                    //get a new question
                    if (QManagObj.NewQuestionForTheSecondActivity(ref the_question))
                    {
                        
                        if (mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(classification_activity_name,
                           out temp_activity_container) &&
                           temp_activity_container.activity_completed == false)//if the second activity is not finished
                        {//but the requirements are met

                            CheckForStartOfActivity(ref temp_activity_container, activity_timer_stop);//end the second activity
                            temp_activity_container = null;//and empty the container


                            inputField2.DeactivateInputField();
                            inputField2.interactable = false;

                            //stop the counting of time in-game
                            mainScriptObj.playerMetricsObject.timeMetricsObject.StartAndStopTimeCounterSinceBeginningOfGame(false);

                            //give final output
                            the_question = " ---   GAME OVER! THANK YOU FOR PLAYING!   --- " +
                                "\n\n\nYour score is: " + mainScriptObj.the_score+"\n\n\n\n";

                            mainScriptObj.logObj.WriteLog();
                        }
                    }
                });

                //end the game and show the score?????
                
            }
        }

        IEnumerator WaitFunction()
        {
            //wait for several seconds
            yield return new WaitForSeconds(5);

            ongoing_learning_activity_name = classification_activity_name;
            //then start the second activity
            SecondActivity();

        }


		private void DebugInput(string the_text)
		{
			Debug.Log ("Input: " + the_text);

		}
		
		
		
		void Update()
        {
			timer.text = the_question+ "\nTime since start of game "+
				mainScriptObj.playerMetricsObject.timeMetricsObject.time_since_beginning_of_game.Elapsed.ToString();

			//CheckForStartOfActivity(ref temp_learning_activity_container);

			//if(ongoing_learning_activity_name=="")
            //{
            //    if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name, 
            //       out temp_activity_container) && 
            //       temp_activity_container.activity_completed==false)
            //    {
            //        CheckForStartOfActivity(ref temp_activity_container);
            //        //temp_learning_activity_container=null;
            //    }
            //    else if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(classification_activity_name, 
            //       out temp_activity_container) && 
            //       temp_activity_container.activity_completed==false)
            //    {
            //        CheckForStartOfActivity(ref temp_activity_container);

            //        if(new_question)
            //        {
            //            new_question=false;
            //            QManagObj.NewQuestionForTheFirstActivity(ref the_question);
            //        }
            //        //temp_learning_activity_container=null;
            //    }
            //}
			/*else
			{
				CheckForStartOfActivity(ref temp_learning_activity_container);
			}*/
				
//            timer.text = "Time since start " +
//				mainScriptObj.playerMetricsObject.timeMetricsObject.GetCountTimeSinceBeginningOfGame().ToString()
//					+"\ntime-on-task "+ mainScriptObj.playerMetricsObject.activityMetricsObject.GetTimeOnTask().ToString();

			//if(mainScriptObj.playerMetricsObject.learningActivitiesList.TryGetValue(learning_activity_name_A, out temp_learning_activity_container))
			if(temp_activity_container!=null)
            {

				timer.text +="\ntime-on-activity "+
					temp_activity_container.time_on_activity.Elapsed.ToString();
				
				mainScriptObj.rulesObject.CheckRulesForActivityMetrics(ref temp_activity_container,
				   ref would_be_hint);
			
			}

			
			//mainScriptObj.rulesObject.CheckRulesForTimeMetrics(ref would_be_hint);
			
			//mainScriptObj.rulesObject.CheckRulesForErrors(ref would_be_hint);

			hint.text = would_be_hint;


           
        }

		/*void PickOneLearningActivity(ref temp_learning_activity_container)
		{

		}*/

//		void CheckRulesForActivityMetrics()
//		{
//			/* //is limit reached for the time since start
//			mainScriptObj.rulesObject.CompareCurrentGameTimeWithGameTimeLimit();
//			//if(mainScriptObj.rulesObject.GetGameTimeLimitReached())
//			if(mainScriptObj.rulesObject.game_time_limit_reached)
//            {
//                timer.text = "Game Over, man! Game Over!";
//				mainScriptObj.rulesObject.game_time_limit_reached=false;
//                //return;
//            }*/
//			
//			//			if(!last_task_limit_reached)
//			{
//				//is limit reached for time-on-task
//				//!!!!!!!!HAVE A CHECK AROUND THIS SO IT CAN BE SKIPPED IF NOT NEEDED
//				if(mainScriptObj.playerMetricsObject.activityMetricsObject.current_task_time_limit_index+1<=
//				   mainScriptObj.playerMetricsObject.activityMetricsObject.time_on_activity_thresholds.Count)
//				{
//					
//					mainScriptObj.rulesObject.CompareCurrentTaskTimeWithTaskTimeLimit();
//				}
//				
//				//			if (mainScriptObj.rulesObject.GetTaskTimeLimitReached())
//				/*if (mainScriptObj.playerMetricsObject.activityMetricsObject.GetTask_time_limit_compared
//				    (mainScriptObj.playerMetricsObject.activityMetricsObject.current_task_time_limit_index))*/
//				{
//					
//					//mainScriptObj.rulesObject.task_time_limit_reached=false;
//					//return;
//					
//					//this might need to be moved; not appropriate here?
//					if(mainScriptObj.playerMetricsObject.activityMetricsObject.current_task_time_limit_index+1<=
//					   mainScriptObj.playerMetricsObject.activityMetricsObject.time_on_activity_thresholds.Count)
//					{
//						if (mainScriptObj.playerMetricsObject.activityMetricsObject.GetTask_time_limit_compared
//						    (mainScriptObj.playerMetricsObject.activityMetricsObject.current_task_time_limit_index))
//						{
//							
//							hint.text = "Task Over, man! Task Over! #"+mainScriptObj.
//								playerMetricsObject.activityMetricsObject.current_task_time_limit_index;
//							
//							mainScriptObj.playerMetricsObject.activityMetricsObject.current_task_time_limit_index++;
//						}
//						
//					}
//					else 
//					{
//						last_task_limit_reached=true;
//					}
//				}
//			}
//		}

		void CheckForStartOfActivity(ref ActivityMetrics temp_learning_activity_container, string activity_command)
        {
			//ActivityMetrics temp_container;

            //if (Input.GetKeyDown(KeyCode.T) == true)
            {

				/*if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue
				   (learning_activity_name_A, out temp_learning_activity_container))*/
				//if(temp_learning_activity_container!=null)
				{
					//if (current_activity_command=="" && Input.GetKeyDown(KeyCode.T) == true)
                    if(activity_command==activity_timer_start)
					{//start the counting of time-on-activity
						//temp_learning_activity_container.IncrementNumberOfTriesToSolve();
						temp_learning_activity_container.StartStopOrPauseTimeOnActivityCounter(activity_timer_start);
						//Debug.LogError(mainScriptObj.playerMetricsObject.activityMetricsObject.GetTimeOnTask());
						//Debug.LogError(mainScriptObj.playerMetricsObject.activityMetricsObject.time_on_activity.Elapsed.ToString());
						current_activity_command = activity_timer_start;
					}
					/*if(current_activity_command==activity_timer_start &&
					   temp_learning_activity_container.time_on_activity.IsRunning)*/
                    if (activity_command == activity_timer_pause && 
                        temp_learning_activity_container.time_on_activity.IsRunning)//if the activity is ongoing and the 
					{//time counter is running 
						//if (Input.GetKeyDown(KeyCode.P) == true)
						{
							current_activity_command = activity_timer_pause;
							temp_learning_activity_container.StartStopOrPauseTimeOnActivityCounter(activity_timer_pause);
						}

					}
					//if(current_activity_command==activity_timer_pause)
                    if (activity_command == activity_timer_unpause)
					{        
						//if(Input.GetKeyDown(KeyCode.U) == true)
						{
							temp_learning_activity_container.StartStopOrPauseTimeOnActivityCounter(activity_timer_unpause);
							current_activity_command = activity_timer_start;
						}
					}
					//if(Input.GetKeyDown(KeyCode.A) == true)
                    if (activity_command == activity_timer_stop)
					{//stop the counting of time-on-activity
						current_activity_command = "";
						temp_learning_activity_container.StartStopOrPauseTimeOnActivityCounter(activity_timer_stop);
						new_question=true;
					}
				}
                
            }
        }



		
    }
}