using UnityEngine;
using System.Linq;



namespace Prototype1v1
{
	public class MainScript : MonoBehaviour
	{
		/*private ErrorMetrics errorEObj;
		private string errorE = "clicked E";

		private HintMetrics hintmetricEObj;

		private ErrorMetrics errorMObj;
		private string errorM = "clicked M";

		private HintMetrics hintEObj;
		private string hintE = "You clicked the button E too many times";*/


		
		public PlayerMetrics playerMetricsObject 
		{
			get;
			set;
		}
		
		public DiagnosticRules rulesObject 
		{
			get;
			set;
		}

        public Logger logObj
        {
            get;
            set;
        }

		public int the_score 
		{
			get;
			set;
		}

		void Awake()
		{
			playerMetricsObject = new PlayerMetrics();
			rulesObject = new DiagnosticRules();
            logObj = new Logger();

            //in order to print the data at the end the objects need to point to the same thing
            logObj.rulesObj = rulesObject;

			//playerMetricsObject.activityMetricsObject = taskObj;
			
			rulesObject.playerMetricsObject = playerMetricsObject;
            //playerMetricsObject.rulesObject = rulesObject;

			the_score=0;

		}
		
		// Use this for initialization
		void Start()
		{
			//			playerMetricsObject = new PlayerMetrics();
			//			rulesObject = new DiagnosticRules();
			//
			//			rulesObject.playerMetricsObject = playerMetricsObject;
			//playerMetricsObject.timeMetricsObject.StartAndStopTimeCounterSinceBeginningOfGame(true);


			//!!!!!!!!!!!!!!instances of all the errors and hints need to be done from the start
			//!!!!!!!!!!!!!in order to use some of their functionality (i.e. limits)
//			InitializeErrorsAndTheirTypes();
//			InitializeHintsAndTheirTypes();
			AddLimitsToTheGameScore();
		}


//
//		//if all the errors are not initialized the limits (as inputs from users) cannot be used
//		//since the errors do not exist at that stage; hence they need to be created from the start
//		private void InitializeErrorsAndTheirTypes()
//		{
//			//creation of objects 
//			errorEObj = new ErrorMetrics();
//			errorMObj = new ErrorMetrics();
//			playerMetricsObject.activityMetricsObject.EncouteredErrorsList.Add(errorE, errorEObj);
//			playerMetricsObject.activityMetricsObject.EncouteredErrorsList.Add(errorM, errorMObj);
//
//
//
//			//retriving the error object from the collection to add limits
//			//checks should be placed in case these fail
//			ErrorMetrics error_temp_container;
//			playerMetricsObject.activityMetricsObject.EncouteredErrorsList.TryGetValue(errorE, out error_temp_container);
//			error_temp_container.StoreInputThresholdsToNumberOfErrors(3);
//			error_temp_container.StoreInputThresholdsToNumberOfErrors(5);
//
//			error_temp_container = null;
//			
//			playerMetricsObject.activityMetricsObject.EncouteredErrorsList.TryGetValue(errorM, out error_temp_container);
//			error_temp_container.StoreInputThresholdsToNumberOfErrors(1);
//			error_temp_container.StoreInputThresholdsToNumberOfErrors(2);
//
//
//		}
//
//		//same as the errors, the hints need to be initialized so that the limits to 
//		//the number of times they have been given can be used
//		private void InitializeHintsAndTheirTypes()
//		{
//			
//			hintEObj = new HintMetrics();
//			hintEObj.hint_identifier = errorE;
//			playerMetricsObject.activityMetricsObject.givenHintsList.Add(hintE, hintEObj);
//			
//			HintMetrics hint_temp_container;
//			playerMetricsObject.activityMetricsObject.givenHintsList.TryGetValue(hintE, out hint_temp_container);
//			hint_temp_container.StoreInputThresholdsToNumberOfHints(1);
//			hint_temp_container.StoreInputThresholdsToNumberOfHints(2);
//		}


		private void AddLimitsToTheGameScore()
		{
			
			//adding limits to the game score
			playerMetricsObject.StoreInputThresholdsToGameScore(5);
			playerMetricsObject.StoreInputThresholdsToGameScore(10);
			//playerMetricsObject.StoreInputThresholdsToGameScore(30);
		}
		
		// Update is called once per frame
		void Update()
		{
			//Debug.LogError(playerMetricsObject.timeMetricsObject.GetCountTimeSinceBeginningOfGame());
			
			//CheckForStartOfTask();
			//Debug.LogError(playerMetricsObject.activityMetricsObject.GetTimeOnTask());
			
			CheckForErrors();

			SendTheGameScore();
			
			//			if (playerMetricsObject.timeMetricsObject.GetCountTimeSinceBeginningOfGame()>=
			//			    playerMetricsObject.timeMetricsObject.GetGameTimeLimitInput())
			//			{
			//				//OnApplicationQuit();
			//			}
		}

		void SendTheGameScore()
		{
			/*if(Input.GetKeyDown(KeyCode.DownArrow))
			{
				//playerMetricsObject.game_score+=10;
				the_score+=5;
				playerMetricsObject.CheckAndStoreInputScore(the_score);
				CheckTheGameScoreAndItsRules();
			}
			if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				//playerMetricsObject.game_score-=10;
				the_score+=10;
				playerMetricsObject.CheckAndStoreInputScore(the_score);
				CheckTheGameScoreAndItsRules();
			}
			if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				//playerMetricsObject.game_score-=5;
				the_score-=25;
				playerMetricsObject.CheckAndStoreInputScore(the_score);
				CheckTheGameScoreAndItsRules();
			}
			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				the_score+=25;
				playerMetricsObject.CheckAndStoreInputScore(the_score);
				//playerMetricsObject.game_score+=5;
				CheckTheGameScoreAndItsRules();
			}*/
		}
		public void CheckTheGameScoreAndItsRules()
		{
			string score_indicator = "";
			rulesObject.CheckThresholdsForGameScore(ref score_indicator);
			//rulesObject.SuddenIncreaseOrDecreaseInGameScore();
			Debug.Log("Score indicator: "+score_indicator+" ;score "+playerMetricsObject.game_score);
		}
		
		
		public void CheckForErrors()
		{
			//string error_indicator = "";

            //rulesObject.CheckRulesForErrors(ref error_indicator);

			if (Input.GetKeyDown(KeyCode.E) == true)
			{
				/*//the error has not been made so far
				if (!playerMetricsObject.EncouteredErrorsList.ContainsKey(errorE))
				{
					//					ErrorMetrics errorObj = new ErrorMetrics
					//						(playerMetricsObject.timeMetricsObject.GetCountTimeSinceBeginningOfGame());
					errorObj = new ErrorMetrics
						(playerMetricsObject.timeMetricsObject.time_since_beginning_of_game.Elapsed);
					playerMetricsObject.EncouteredErrorsList.Add(errorE, errorObj);
					
				}
				else
				{//the error has been made previously
					ErrorMetrics temp_container;
					playerMetricsObject.EncouteredErrorsList.TryGetValue(errorE, out temp_container);
					//					temp_container.ErrorMade(playerMetricsObject.timeMetricsObject.
					  GetCountTimeSinceBeginningOfGame());
					temp_container.ErrorMade(playerMetricsObject.timeMetricsObject.
					  time_since_beginning_of_game.Elapsed);
				}*/


//				ErrorMetrics temp_container;
//				playerMetricsObject.activityMetricsObject.EncouteredErrorsList.TryGetValue(errorE, out temp_container);
//				temp_container.ErrorMade(playerMetricsObject.timeMetricsObject.time_since_beginning_of_game.Elapsed);
//
//				
//				//print all the data for the error
//				temp_container = null;
//				playerMetricsObject.activityMetricsObject.EncouteredErrorsList.TryGetValue(errorE, out temp_container);
//				//string errorInfo = "# - " + temp_container1.GetTimeStamps().Count + " timestamps: ";
//				string errorInfo = errorE+ " | " + temp_container.GetNumberOfErrors() + " timestamps: ";
//				for (int i = 0; i < temp_container.GetNumberOfErrors(); i++)
//				{
//					errorInfo += temp_container.error_time_stamps.ElementAt(i) + "  ";
//				}
//				Debug.LogError(errorInfo);
//			}
//			else if (Input.GetKeyDown(KeyCode.M) == true)
//			{
//				ErrorMetrics temp_container;
//				playerMetricsObject.activityMetricsObject.EncouteredErrorsList.TryGetValue(errorM, out temp_container);
//				temp_container.ErrorMade(playerMetricsObject.timeMetricsObject.time_since_beginning_of_game.Elapsed);
//
//				//print all the data for the error
//				temp_container = null;
//				playerMetricsObject.activityMetricsObject.EncouteredErrorsList.TryGetValue(errorM, out temp_container);
//				//string errorInfo = "# - " + temp_container1.GetTimeStamps().Count + " timestamps: ";
//				string errorInfo = errorM + " | " + temp_container.GetNumberOfErrors() + " timestamps: ";
//				for (int i = 0; i < temp_container.GetNumberOfErrors(); i++)
//				{
//					errorInfo += temp_container.error_time_stamps.ElementAt(i) + "  ";
//				}
//				Debug.LogError(errorInfo);
			}
			
		}
		
		
		//void OnApplicationQuit()
		//{
		//    playerMetricsObject.timeMetricsObject.StartAndStopTimeCounterSinceBeginningOfGame(false);
		
		//}
	}
}
