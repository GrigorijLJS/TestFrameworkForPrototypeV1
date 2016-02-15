using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

//for stopwatch
using System.Diagnostics;

namespace Prototype1v1
{
    public class TimeMetrics //: IMetrics
    {
		//MIGHT NOT BE NEEDED
        //used to check if the player is actively doing something
		public List<TimeSpan> activityTimestamps 
		{
			get;
			set;
		}

        //keeps track of time from the start of a game/session to the end
		public Stopwatch time_since_beginning_of_game
		{
			get;
			set;
		}
        //storage of the time from the start of a game/session to the end
        //could be usefeul for the logging of data when the system closes
		public TimeSpan stored_time_since_beginning_of_game 
		{
			get;
			set;
		}


		//MIGHT NOT BE NEEDED
		//average time that the player has spent playing the game
		//calculated from previous times that the player has played the game
		public TimeSpan average_time_since_beginning_of_game 
		{
			get;
			set;
		}

		//MIGHT NOT BE NEEDED
        //a collection of time slices that the player spent doing a game specific action
        //???????maybe a dictionary with string as key for flexibility and having multiple actions
		public Dictionary<string, TimeSpan> time_since_action 
		{
			get;
			set;
		}

		//used to keep track of the index of the current limit for the game time
		public int current_game_time_limit_index 
		{
			get;
			set;
		}

		//!!!!!!!!!!!!!!!!!!!!CREATES ISSUES WHEN IT IS A PROPERTY
		/*public*/private List<bool> game_time_limit_compared = new List<bool>();
		/*{
			get;
			set;
		}*/
		
		public bool GetGame_time_limit_compared(int index)
		{
			return game_time_limit_compared.ElementAt(index);
		}
		
		public void SetGame_time_limit_compared(int an_index, bool new_value)
		{
			game_time_limit_compared.RemoveAt(an_index);
			game_time_limit_compared.Insert(an_index, new_value);
		}

        ////////
        ////////the following fields require input from users (not the players) or the games
        ////////

        //the game time limits based on the user of the hinting system or the game (needs input in minutes,
        //can take decimal numbers for inputing seconds for instance)
        //can be used to set time limits upon the players for how long the game should be played
		public List<TimeSpan> game_time_limits 
		{
			get;
			set;
		}

		//constructor
		public TimeMetrics()
		{
			activityTimestamps = new List<TimeSpan>();
			time_since_beginning_of_game = new Stopwatch();
			stored_time_since_beginning_of_game = new TimeSpan();
			time_since_action = new Dictionary<string, TimeSpan>();
			game_time_limits = new List<TimeSpan>();
			current_game_time_limit_index = 0;

			//this is hardcoded for now and MIGHT NOT BE NEEDED
			average_time_since_beginning_of_game = new TimeSpan(0,5,0);
			
		}

        ////////
        ////////
        ////////

        /// <summary>
        /// getters and setters for various class fields
        /// </summary>
        
//
//		public TimeSpan GetStoredCountTimeSinceBeginningOfGame()
//        {
//            return stored_time_since_beginning_of_game;
//        }
//
//        public void SetStoredCountTimeSinceBeginningOfGame(TimeSpan new_value)
//        {
//            stored_time_since_beginning_of_game = new_value;
//        }
//
//        public TimeSpan GetCountTimeSinceBeginningOfGame()
//        {
//            return time_since_beginning_of_game.Elapsed;
//        }
//
//        public TimeSpan GetGameTimeLimitInput()
//        {
//            return game_time_limit_input;
//        }
//
//        public void SetGameTimeLimitInput(TimeSpan new_value)
//        {
//            game_time_limit_input = new_value;
//        }

        /// <summary>
        /// methods
        /// </summary>

        //function used to keep track if the player is active
        //public void CheckForActivityAndStoreTimestamps()
        //{
        //    ZSRghj
        //}

        //start or stop the counter for the time since the player started playing the game
        //and also save the said time
        public void StartAndStopTimeCounterSinceBeginningOfGame(bool start_or_stop_flag)
        {

            if (start_or_stop_flag)
            {
                time_since_beginning_of_game.Reset();
                time_since_beginning_of_game.Start();
            }
            else
            {
                time_since_beginning_of_game.Stop();
				stored_time_since_beginning_of_game=time_since_beginning_of_game.Elapsed;
            }
        }

        //function for storing a game time limit, receives the limit in a string form (useful for GUI input)
        public void CheckAndStoreInputGameTimeLimit(string hours, string minutes, string seconds)
        {

            int temp_hours = 0;
            int temp_minutes = 0;
            int temp_seconds = 0;

            if((Int32.TryParse(hours, out temp_hours) && Int32.TryParse(minutes, out temp_minutes)
                && Int32.TryParse(seconds, out temp_seconds)))
            {
                //SetGameTimeLimitInput(new TimeSpan(temp_hours, temp_minutes, temp_seconds));
				game_time_limits.Add(new TimeSpan(temp_hours, temp_minutes, temp_seconds));
				game_time_limit_compared.Add(false);

            }

            ////if input is not empty
            //if (!string.IsNullOrWhiteSpace(input_time))
            //{
            //    int temp_time_int = 0;
            //    double temp_time_double = 0;

            //    //try to convert it to an integer (if it does not contain decimals)
            //    if (Int32.TryParse(input_time, out temp_time_int))
            //    {
            //        SetGameTimeLimitInput(TimeSpan.FromMinutes(temp_time_int));
            //    }//if there are decimals convert to double
            //    else if (Double.TryParse(input_time, out temp_time_double))
            //    {
            //        SetGameTimeLimitInput(TimeSpan.FromMinutes(temp_time_double));
            //    }
            //}

        }

        //overload for the above function for storing a game time limit, gets hours, mins and secs
        public void CheckAndStoreInputGameTimeLimit(int hours, int minutes, int seconds)
        {

            //SetGameTimeLimitInput(new TimeSpan(hours, minutes, seconds));
			game_time_limits.Add(new TimeSpan(hours, minutes, seconds));
			game_time_limit_compared.Add(false);
        }


    }
}
