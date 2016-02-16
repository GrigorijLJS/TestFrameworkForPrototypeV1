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

		private bool last_learning_activity_limit_reached = false;

        //float time = 0;

		
		private ActivityMetrics quiz_activityObj = new ActivityMetrics();
		private string quiz_activity_name = "QuizActivity";
		
		private ActivityMetrics classification_activityObj = new ActivityMetrics();
		private string classification_activity_name = "ClassificationActivity";

		private ActivityMetrics temp_activity_container;

		public InputField inputField; 

		
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

			mainScriptObj.playerMetricsObject.timeMetricsObject.CheckAndStoreInputGameTimeLimit(0, 0, 3);
			mainScriptObj.playerMetricsObject.timeMetricsObject.CheckAndStoreInputGameTimeLimit(0, 0, 5);

			//!!!!!!!!!!!!!!!!!!!
			//the activity needs to be added to the collection if the limits are to be used

			//activity A
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
				
				temp_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(2);
				temp_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(3);
				temp_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(4);
			}

			//activity B
			if(!mainScriptObj.playerMetricsObject.gameActivitiesList.ContainsKey(classification_activity_name))
			{
				
				mainScriptObj.playerMetricsObject.gameActivitiesList.Add(classification_activity_name,classification_activityObj);
			}
			
			if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(classification_activity_name, out temp_activity_container))
			{
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 3);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 5);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 7);
				
				/*temp_learning_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(2);
				temp_learning_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(3);
				temp_learning_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(4);*/
			}

			temp_activity_container=null;

			//get a new question
			QManagObj.NewQuestionForTheFirstActivity(ref the_question);

            //first task
            if (mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name,
                   out temp_activity_container) &&
                   temp_activity_container.activity_completed == false)
            {
                CheckForStartOfActivity(ref temp_activity_container, activity_timer_start);
                //temp_learning_activity_container=null;
            }

			inputField.onEndEdit.AddListener(delegate {
				new_points=0;

                //if the following int is 0, the answer is correct, if it is 1 error in recognizing happened
                //if it is 2 error in recall was made
                int corectAnswerOrTypeOfError=QManagObj.CheckCorrectnessOfAnswer(inputField.text, ref new_points);

				if(corectAnswerOrTypeOfError==0)
				{
					//the player gets some points
					mainScriptObj.the_score+=new_points;

					//update the diagnostic system about the score changes
					mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
					//and check the rules for the score
					mainScriptObj.CheckTheGameScoreAndItsRules();

					DebugInput(inputField.text);
					inputField.text="";
				}
				else
				{//incorrect answer
					//the player loses some points
					mainScriptObj.the_score-=new_points;

					//update the diagnostic system about the score changes
					mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
					//and check the rules for the score
					mainScriptObj.CheckTheGameScoreAndItsRules();

                    DebugInput("WAAAAAAAAAAH!!!" + inputField.text);
					//mainScriptObj.CheckForErrors();

                    //check the type of the error
                    //if it is 1 error in recognizing happened, ID: errorInRecognizing
                    //if it is 2 error in recall was made, ID: errorInRecalling 

					ErrorMetrics temp_container;
                    if(corectAnswerOrTypeOfError==1)
                    {

                        mainScriptObj.playerMetricsObject.activityMetricsObject.EncouteredErrorsList
                            .TryGetValue("errorInRecognizing", out temp_container);
                        temp_container.ErrorMade(mainScriptObj.playerMetricsObject
                                                 .timeMetricsObject.time_since_beginning_of_game.Elapsed);

                        string errorInfo = "ErrorInRecognizing timestamps: " + temp_container.error_time_stamps.Count + "#  ";
                        for (int i = 0; i < temp_container.error_time_stamps.Count; i++)
                        {
                            errorInfo += temp_container.error_time_stamps[i] + "  ";
                        }
                        Debug.LogError(errorInfo);

                        //string errorinfo = errore + " | " + temp_container.getnumberoferrors() + " timestamps: ";
                        //for (int i = 0; i < temp_container.getnumberoferrors(); i++)
                        //{
                        //    errorinfo += temp_container.error_time_stamps.elementat(i) + "  ";
                        //}
                        //debug.logerror(errorinfo);
                    }
                    else
                    {
                        mainScriptObj.playerMetricsObject.activityMetricsObject.EncouteredErrorsList
                            .TryGetValue("errorInRecalling", out temp_container);
                        temp_container.ErrorMade(mainScriptObj.playerMetricsObject
                                                 .timeMetricsObject.time_since_beginning_of_game.Elapsed);

                        string errorInfo = "ErrorInRecalling timestamps: " + temp_container.error_time_stamps.Count+"#  ";
                        for (int i = 0; i < temp_container.error_time_stamps.Count; i++)
                        {
                            errorInfo += temp_container.error_time_stamps[i] + "  ";
                        }
                        Debug.LogError(errorInfo);
                    }

					inputField.text="";
				}
                //get a new question
                if(QManagObj.NewQuestionForTheFirstActivity(ref the_question))
                {
                    //reset the question var to empty
                    //the_question = "";
                    //new activity
                    SecondTask(ref the_question);
                }

			});
        }

        void SecondTask(ref string the_question)
        {
            if (mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name,
                   out temp_activity_container) &&
                   temp_activity_container.activity_completed == false)//if the first activity is not finished
            {//but the requirements are met
                DebugInput("Chaos sighted!");
                CheckForStartOfActivity(ref temp_activity_container, activity_timer_stop);//end the first activity
                temp_activity_container = null;//and empty the container


                StartCoroutine(WaitFunction());

                //start the second task
                if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(classification_activity_name,
                   out temp_activity_container))
                {
                    CheckForStartOfActivity(ref temp_activity_container, activity_timer_start);//start the second activity timer
                    QManagObj.NewQuestionForTheSecondActivity(ref the_question);
                }
                
            }
        }

        IEnumerator WaitFunction()
        {
            Debug.Log("Before Waiting 2 seconds");
            yield return new WaitForSeconds(2);
            Debug.Log("After Waiting 2 Seconds");
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
			{

				timer.text +="\ntime-on-task "+
					temp_activity_container.time_on_activity.Elapsed.ToString();
				
				mainScriptObj.rulesObject.CheckRulesForActivityMetrics(ref temp_activity_container,
				   ref would_be_hint);
			
			}

			
			//mainScriptObj.rulesObject.CheckRulesForTimeMetrics(ref would_be_hint);
			
			mainScriptObj.rulesObject.CheckRulesForErrors(ref would_be_hint);

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
					{//start the counting of time-on-task
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
					{//stop the counting of time-on-task
						current_activity_command = "";
						temp_learning_activity_container.StartStopOrPauseTimeOnActivityCounter(activity_timer_stop);
						new_question=true;
					}
				}
                
            }
        }



		
    }
}