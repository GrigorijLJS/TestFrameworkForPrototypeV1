using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;


//for stopwatch
using System.Diagnostics;

namespace Prototype1v1
{
    public class ActivityMetrics //: IMetrics
    {
        public Utilities utilitiesObject
        {
            get;
            set;
        }


        //used to store the errors (by type), # of times encountered and the timestamps when they occured
        // private Dictionary<string, ErrorMetrics> encouteredErrorsList;
        public Dictionary<string, ErrorMetrics> EncouteredErrorsList
        {
            // return encouteredErrorsList;
            get;
            set;
        }

        public string errorRecognizeName
        {
            get;
            private set;
        }

        public string errorRecallName
        {
            get;
            private set;
        }
        public string errorImplementName
        {
            get;
            private set;
        }
        public string errorClassifyName
        {
            get;
            private set;
        }
        //used to store the hints given to the players (by type), # of times they were given 
        //and the timestamps when they were given
        //private Dictionary<string, HintMetrics> givenHintsList;
        public Dictionary<string, HintMetrics> givenHintsList
        {
            get;
            set;
        }


        //this is not needed since the list of tasks is in PlayerMetrics now, where the ids/names are stored 
        ////the id/name of the task
        //private string task_id = "";

		//flag used to indicate if the learning activity is completed
		public bool activity_completed 
		{
			get;
			set;
		}


        //number of tries to solve the task (if it is possible)
        public int number_of_tries_to_solve 
		{
			get;
			set;
		}

		public List<int> limits_to_number_of_tries_to_solve 
		{
			get;
			set;
		}

		public List<bool> compared_limits_to_number_of_tries_to_solve 
		{
			get;
			set;
		}

        //the timer used to measure time on a separate task/turn/quest
		public Stopwatch time_on_activity 
		{
			get;
			set;
		}

        //collection for storing the time used on this task/turn/quest 
		public List<TimeSpan> list_of_times_on_activity
		{
			get;
			set;
		}

        //task dificulty
        //!!!!!!!!!!or maybe the game should have the scale, and just provide the task difficulty and
        //!!!!!!!!!! the difficulty scale (from one to five, 1-10) to the diagnostic system
        private enum difficulty
        {
            very_easy,
            easy,
            medium,
            hard,
            very_hard
        };

        //stores the names/types of other activities that need to be completed before this activity can be started
        public List<string> prerequisite_game_activities 
        {
            get;
            set;
        }

        //task score
        //!!!!!!!!!!!!!!!!!!!
        private int task_score
        {
            get;
            set;
        }



		//used to keep track of the index of the current limit for the task time
		public int current_task_time_limit_index 
		{
			get;
			set;
		}


		public List<bool> time_on_activity_threshold_compared
		{
			get;
			set;
		}
		/*
		public bool GetTask_time_limit_compared(int index)
		{
			return time_on_activity_threshold_compared.ElementAt(index);
		}
		
		public void SetTask_time_limit_compared(int an_index, bool new_value)
		{
			time_on_activity_threshold_compared.RemoveAt(an_index);
			time_on_activity_threshold_compared.Insert(an_index, new_value);
		}*/




          ////////
         ////////the following fields require input from users (not the players) or the games
        ////////

        //the task time limit based on the user of the hinting system or the game (needs input in minutes,
        //can take decimal numbers for inputing seconds for instance)
        //can be used by the users to set time limit upon the players for tasks/turns/etc. in games
		public List<TimeSpan> time_on_activity_thresholds 
		{
			get;
			set;
		}
          ////////
         ////////
        ////////

        public List<TimeSpan> default_time_on_activity_thresholds
        {
            get;
            set;
        }

		public ActivityMetrics()
		{
			number_of_tries_to_solve = 0;
			limits_to_number_of_tries_to_solve = new List<int>();
			compared_limits_to_number_of_tries_to_solve = new List<bool>();

			time_on_activity = new Stopwatch();
			list_of_times_on_activity = new List<TimeSpan>();

			time_on_activity_thresholds = new List<TimeSpan>();
			time_on_activity_threshold_compared = new List<bool>();

			current_task_time_limit_index=0;
            //time_on_activity_threshold_compared = new List<bool>();

            EncouteredErrorsList = new Dictionary<string, ErrorMetrics>();
            givenHintsList = new Dictionary<string, HintMetrics>();

			activity_completed = false;

			//initialize the hardcoded errors and their names/IDs
			ErrorMetrics errorInRecognizing = new ErrorMetrics();
            errorRecognizeName = "errorInRecognizing";
            ErrorMetrics errorInRecalling = new ErrorMetrics();
            errorRecallName = "errorInRecalling";
            ErrorMetrics errorInClassifying = new ErrorMetrics();
            errorClassifyName="errorInClassifying";
            ErrorMetrics errorInImplementing = new ErrorMetrics();
            errorImplementName="errorInImplementing";

			EncouteredErrorsList.Add(errorRecognizeName,errorInRecognizing);
			EncouteredErrorsList.Add(errorRecallName,errorInRecalling);
            EncouteredErrorsList.Add(errorClassifyName, errorInClassifying);
            EncouteredErrorsList.Add(errorImplementName, errorInImplementing);
        }


        /// <summary>
        /// getters and setters for various class fields
        /// </summary>
//        public TimeSpan GetTaskTimeLimitInput()
//        {
//            return task_time_limit_input;
//        }
//
//        public void SetTaskTimeLimitInput(TimeSpan new_value)
//        {
//            task_time_limit_input = new_value;
//        }
//
//        //public string GetTaskID()
//        //{
//        //    return task_id;
//        //}
//
//        //public void SetTaskID(string new_value)
//        //{
//        //    task_id = new_value;
//        //}
//
//        public List<TimeSpan> GetListOfTimesOnTask()
//        {
//            return list_of_times_on_activity;
//        }
//
//
//        public int GetNumberOfTriesToSolve()
//        {
//            return number_of_tries_to_solve;
//        }
//
//        public TimeSpan GetTimeOnTask()
//        {
//            return time_on_activity.Elapsed;
//        }




        /// <summary>
        /// methods
        /// </summary>

        //function to increment the number of tries to solve the task
        public void IncrementNumberOfTriesToSolve()
        {
            number_of_tries_to_solve++;
        }

        //!!!!THE NEW VERSION IS FURTHER DOWN
        ////get the input for the time limit from the game/user and store it
        //public void CheckAndStoreInputTimeThreshold(string input_time)
        //{
        //    //Utilities utilityObject = new Utilities();

        //    //if input is not empty
        //    //if (!string.IsNullOrWhiteSpace(input_time)) does not work in Unity
        //    if (!utilitiesObject.ConsistsOfWhiteSpace(input_time))
        //    {
        //        int temp_time_int = 0;
        //        double temp_time_double = 0;

        //        //try to convert it to an integer (if it does not contain decimals)
        //        if (Int32.TryParse(input_time, out temp_time_int))
        //        {
        //            SetTaskTimeLimitInput(TimeSpan.FromMinutes(temp_time_int));
        //        }//if there are decimals convert to double
        //        else if (Double.TryParse(input_time, out temp_time_double))
        //        {
        //            SetTaskTimeLimitInput(TimeSpan.FromMinutes(temp_time_double));
        //        }
        //    }
        //}

		//function used to store the limits to the number of retires for this task
		public void StoreInputLimitToNumberOfTriesToSolveThisTask(int limit)
		{
			limits_to_number_of_tries_to_solve.Add(limit);
			compared_limits_to_number_of_tries_to_solve.Add(false);
		}

        //get the input for the time limit from the game/user and store it
        //this one gets three strings that represent the limit
        public void CheckAndStoreInputTimeThreshold(string hours, string minutes, string seconds)
        {
            int temp_hours = 0;
            int temp_minutes = 0;
            int temp_seconds = 0;

            //sanity check
            if ((Int32.TryParse(hours, out temp_hours) && Int32.TryParse(minutes, out temp_minutes)
                && Int32.TryParse(seconds, out temp_seconds)))
            {
                //SetTaskTimeLimitInput(new TimeSpan(temp_hours, temp_minutes, temp_seconds));
				time_on_activity_thresholds.Add(new TimeSpan(temp_hours, temp_minutes, temp_seconds));
				time_on_activity_threshold_compared.Add(false);
            }
        }

        //overload for the above function for storing a task time limit, gets hours, mins and secs as ints
        public void CheckAndStoreInputTimeThreshold(int hours, int minutes, int seconds)
        {
			time_on_activity_thresholds.Add(new TimeSpan(hours, minutes, seconds));
			time_on_activity_threshold_compared.Add(false);
            //SetTaskTimeLimitInput(new TimeSpan(hours, minutes, seconds));
        }


        //start, pause, unpause or stop the counter for the time-on-activity
        //and also save the time spent in a list, after the activity is done
        public void StartStopOrPauseTimeOnActivityCounter(string start_pause_or_stop_flag)
        {
            //if(GetTaskID()!="")
            //{
            //    //store the name of the task if not already present
            //    SetTaskID(task_name);
            //}

            //start the timer
			if (start_pause_or_stop_flag=="start")
            {
                //time_on_activity.Reset();//just in case there is still counting of time
                time_on_activity.Start();
            }
			else if(start_pause_or_stop_flag=="pause")
			{//pause the timer (can be made to continue from where it was paused)
				time_on_activity.Stop();
			}
			else if(start_pause_or_stop_flag=="unpause")
			{//unpause the timer
				time_on_activity.Start();
			}
			else if(start_pause_or_stop_flag=="stop")
            {//or stop it completely and store the time
                time_on_activity.Stop();
				list_of_times_on_activity.Add(time_on_activity.Elapsed);
				time_on_activity.Reset();

				//the learning activity is "completed"
				activity_completed=true;
            }
        }

        



    }
}
