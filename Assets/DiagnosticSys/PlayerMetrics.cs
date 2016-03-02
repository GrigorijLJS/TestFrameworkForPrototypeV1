using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using System.Threading.Tasks;


//for getting keyboard input
//using System.Windows.Input;

namespace Prototype1v1
{
    // the "main" class for storing the player metrics 

    public class PlayerMetrics 
    {
   
        //objects for the "subordinate" classes that contain their metrics by type
        public TimeMetrics timeMetricsObject
        {
            get;
            set;
        }

        public ActivityMetrics activityMetricsObject
        {
            get;
            set;
        }

        public DiagnosticRules rulesObject
        {
            get;
            set;
        }
        /*public ErrorMetrics errorMetricsObject
        {
            get;
            set;
        }

        public HintMetrics hintMetricsObject
        {
            get;
            set;
        }*/


        // TODO nothing


        /// <summary>
        /// fields 
        /// </summary>

        /*//used to store the errors (by type), # of times encountered and the timestamps when they occured
        // private Dictionary<string, ErrorMetrics> encouteredErrorsList;
		public Dictionary<string, ErrorMetrics> EncouteredErrorsList
		{
			// return encouteredErrorsList;
			get;
			set;
		}

        //used to store the hints given to the players (by type), # of times they were given 
        //and the timestamps when they were given
        //private Dictionary<string, HintMetrics> givenHintsList;
		public Dictionary<string, HintMetrics> givenHintsList
		{
			get;
			set;
		}*/

        //used to store the learning activities (in the game (by id/name), 
		//along with other activity related data
        public Dictionary<string, ActivityMetrics> gameActivitiesList
		{
			get;
			set;
		}

          /////////
         /////////the following fields require input from the users (not the players) or the games
        /////////

        /*//player name/nickname
        //used to identify the players
        private string player_name = "";*/
		public string player_name
		{
			get;
			private set;
		}

        //gender of the player
        //might be usefull for personalization; could be left empty and then it will not be used
        private string player_gender = "";

        //the player's score - based on the game (requires input from the users (or games) of the hintning system)
        //this could include score based on various terms such as: game points, correct actions performed, etc.
        //if left to be 0, it will not be used 
        //private int game_score = 0;
		public int game_score
		{
			get;
			set;
		}

		
		public int game_score_previous_state
		{
			get;
			set;
		}

		public List<int> game_score_triggers 
		{
			get;
			set;
		}

		public List<int> game_score_default_triggers 
		{
			get;
			set;
		}

		//may not be needed
		public List<bool> game_score_thresholds_compared 
		{
			get;
			set;
		}

		public int max_game_score
		{
			get;
			set;
		}

		public int min_game_score
		{
			get;
			set;
		}
        

        
        public PlayerMetrics()
        {
            /*EncouteredErrorsList = new Dictionary<string, ErrorMetrics>();
            givenHintsList = new Dictionary<string, HintMetrics>();*/
            gameActivitiesList = new Dictionary<string, ActivityMetrics>(); 

            timeMetricsObject = new TimeMetrics();
            activityMetricsObject = new ActivityMetrics();
            //rulesObject = new DiagnosticRules();

			game_score=0;
			game_score_previous_state=0;
			max_game_score=0;
			min_game_score=0;
			game_score_triggers = new List<int>();
			game_score_default_triggers = new List<int>();
			game_score_thresholds_compared = new List<bool>();

			game_score_default_triggers.Add(3);
			game_score_default_triggers.Add(23);
        }


        /*public int GetGameScore()
        {
            return game_score;
        }

        private void SetGameScore(int input_value)
        {
            game_score = input_value;
        }*/

        

        //public string PlayerNameEx
        //{
        //    get
        //    {
        //        return "mySelf";
        //    }
        //    private set
        //    {
        //        player_name = value;
        //    }
        //}
        /*public string GetPlayerName()
        {
            return player_name;
        }

        public void SetPlayerName(string input_value)
        {
            player_name = input_value;
        }*/

        public string GetPlayerGender()
        {
            return player_gender;
        }

        public void SetPlayerGender(string input_value)
        {
            player_gender = input_value;
        }

        

        /*public Dictionary<string, HintMetrics> GetGivenHintsList()
        {
            return givenHintsList;
        }

        public Dictionary<string, ActivityMetrics> GetTasksList()
        {
            return tasksList;
        }*/


		public void StoreInputThresholdsToGameScore(int limit)
		{
			game_score_triggers.Add(limit);
			game_score_thresholds_compared.Add(false);
		}

        /*public ErrorMetrics NewErrorMetricsObject()
        {
            return new ErrorMetrics();
        }*/

        

        
        //function to be used with GUI, gets strings and converts them to integer
        //get the input for the score from the game/user and store it
        public void CheckAndStoreInputScore(string input_score)
        {
            Utilities utilityObject = new Utilities();

            //if the input is not empty and contains only a number
            //if (!string.IsNullOrWhiteSpace(input_score) && utilityObject.IncludesDigitsOnly(input_score))
            //does not work in Unity
            if (!utilityObject.ConsistsOfWhiteSpace(input_score) && utilityObject.IncludesDigitsOnly(input_score))
            {
				//store the previous state of the score so it can be checked in the rules
				game_score_previous_state=game_score;
				game_score=Int32.Parse(input_score);
                //SetGameScore(Int32.Parse(input_score));//store it
            }
        }

        //overload for the function above, this one gets integers
        //get the input for the score from the game/user and store it
        public void CheckAndStoreInputScore(int input_score)
		{
			//store the previous state of the score so it can be checked in the rules
			game_score_previous_state=game_score;
			game_score=input_score;
            //SetGameScore(input_score);//store it
        }


        public string IdentifyAndStoreErrorForThisActivity(string current_activity_ID, string error_ID)
        {
            ActivityMetrics current_activity = null;
            ErrorMetrics temp_error_for_first=null;
            string would_be_hint="";

            gameActivitiesList.TryGetValue(current_activity_ID, out current_activity);

            current_activity.EncouteredErrorsList
                                        .TryGetValue(error_ID, out temp_error_for_first);
            temp_error_for_first.ErrorMade(current_activity.time_on_activity.Elapsed);

            string errorInfo = error_ID+" timestamps: "
                + temp_error_for_first.error_time_stamps.Count + "#  ";

            for (int i = 0; i < temp_error_for_first.error_time_stamps.Count; i++)
            {
                errorInfo += temp_error_for_first.error_time_stamps[i] + "  ";
            }

            //rulesObject.CheckRulesForErrors(ref current_activity, ref would_be_hint);

            return ("Hint: "+would_be_hint+" info for errors "+errorInfo);
        }

        
    }


}
