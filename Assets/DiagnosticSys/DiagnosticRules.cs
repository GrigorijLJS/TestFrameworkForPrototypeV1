﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using System.Threading.Tasks;

namespace Prototype1v1
{
    public class DiagnosticRules
    {
        public PlayerMetrics playerMetricsObject
        {
            get;
            set;
        }

        public Indicators indicatorsObject
        {
            get;
            set;
        }

        //
		public bool game_time_limit_reached 
		{
			get;
			set;
		}

        //
		public bool task_time_limit_reached 
		{
			get;
			set;
		}



        //constructors 

        public DiagnosticRules()
        {
            //playerMetricsObject = new PlayerMetrics();

			//used to track which task time limit should be used next
			game_time_limit_reached = false;
			task_time_limit_reached = false;

            indicatorsObject = new Indicators();
        }



        //getters/setters

//        public void SetMetricsObject(PlayerMetrics new_value)
//        {
//            playerMetricsObject = new_value;
//        }


//        public bool GetGameTimeLimitReached()
//        {
//            return game_time_limit_reached;
//        }
//
//        public void SetGameTimeLimitReached(bool new_value)
//        {
//            game_time_limit_reached = new_value;
//        }
//
//        public bool GetTaskTimeLimitReached()
//        {
//            return task_time_limit_reached;
//        }
//
//        public void SetTaskTimeLimitReached(bool new_value)
//        {
//            task_time_limit_reached = new_value;
//        }


        //methods 

		/*public void CheckRulesForGameScore()
		{
			SuddenIncreaseOrDecreaseInGameScore();

		}*/
		
		/*public void SuddenIncreaseOrDecreaseInGameScore()
		{
			int previous_score=0;

			previous_score = playerMetricsObject.game_score;

			//if()


		}*/

        public void CheckThresholdsForMainIndicators()
        {
            foreach(var main_indicator in indicatorsObject.mainIndicators)
            {
                foreach(var main_indicator_threshold in indicatorsObject.mainIndicators_thresholds)
                {
                    if(main_indicator.Value==main_indicator_threshold)
                    {
                        indicatorsObject.StoreAuxiliaryIndicator(main_indicator.Key);
                    }
                }
            }
        }

		public void CheckThresholdsForGameScore(ref string score_indicator)
		{
			/*int i=0;
			foreach(var game_score_threshold in playerMetricsObject.game_score_triggers)
			{
				if(game_score_threshold==(playerMetricsObject.game_score-playerMetricsObject.game_score_previous_state))
				{
					if(!playerMetricsObject.game_score_thresholds_compared[i])
					{
						playerMetricsObject.game_score_thresholds_compared[i] = true;
						would_be_hint = "Score indicator " + (i+1) + " activated!";
					}
				}
				i++;
			}*/

            //the ID/name for the indicator
            string score_increase_or_decrease_indicator_ID = "";

            //check if the player lost or gained points
            if((playerMetricsObject.game_score-playerMetricsObject.game_score_previous_state)>0)
            {
                score_increase_or_decrease_indicator_ID="scoreIncrease";
            }
            else if((playerMetricsObject.game_score-playerMetricsObject.game_score_previous_state)<0)
            {
                score_increase_or_decrease_indicator_ID = "scoreDecrease";
            }

			//the absolute value of the change of the score
			int absolute_score=Math.Abs(playerMetricsObject.game_score-playerMetricsObject.game_score_previous_state);

			//loop through the inflation/deflation triggers (threholds) of the score
			for(int i=0; i<playerMetricsObject.game_score_triggers.Count;i++)
			{
                //if this is not the last inflation/deflation trigger
				if((i+1)<playerMetricsObject.game_score_triggers.Count)
				{
					if(absolute_score>=playerMetricsObject.game_score_triggers.ElementAt(i) &&
					   absolute_score<playerMetricsObject.game_score_triggers.ElementAt(i+1))
					{//check if the change to the score is between the current trigger and the next one
						
                        //the indicator printed in the game
                        score_indicator = "Score indicator " + (i) + " activated!";

                        //attach the value that activated the indicator
                        score_increase_or_decrease_indicator_ID += ";"+playerMetricsObject.game_score_triggers.ElementAt(i);
                        //and send the main indicator 
                        indicatorsObject.StoreMainIndicator(score_increase_or_decrease_indicator_ID);
					}

				}
                else //if it is the last inflation/deflation trigger
				{
					if(absolute_score>=playerMetricsObject.game_score_triggers.ElementAt(i))
                    {//just comapre the change to the trigger

                        //the indicator printed in the game
						score_indicator = "Score indicator " + (i) + " activated!";

                        //attach the value that activated the indicator
                        score_increase_or_decrease_indicator_ID += ";"+playerMetricsObject.game_score_triggers.ElementAt(i);
                        //and send the main indicator 
                        indicatorsObject.StoreMainIndicator(score_increase_or_decrease_indicator_ID);

						
					}
				}
			}
		}


        public void CheckRulesForActivityMetrics(ref ActivityMetrics temp_activity_container, ref string would_be_hint)
        {
            string time_on_activity_threshold_ID="timeOnActivityThreshold";
            //compare the time time-on-activity with the time thresholds 

            if (temp_activity_container.current_task_time_limit_index + 1 <=
               temp_activity_container.time_on_activity_thresholds.Count)
            {

                //CompareCurrentTaskTimeWithTaskTimeLimit();
                //-----------------
                if (temp_activity_container.time_on_activity.Elapsed
                    > temp_activity_container.time_on_activity_thresholds.ElementAt
                    (temp_activity_container.current_task_time_limit_index)
                    && temp_activity_container.time_on_activity_threshold_compared
                    [temp_activity_container.current_task_time_limit_index] != true)
                {

                    temp_activity_container.time_on_activity_threshold_compared[
                        temp_activity_container.current_task_time_limit_index] = true;
                }

                //-------------
            }


            if (temp_activity_container.current_task_time_limit_index + 1 <=
               temp_activity_container.time_on_activity_thresholds.Count)
            {
                if (temp_activity_container.time_on_activity_threshold_compared
                    [temp_activity_container.current_task_time_limit_index])
                {

                    would_be_hint = "Task Over, man! Task Over! " +
                        (temp_activity_container.current_task_time_limit_index+1)
                        + " out of " + temp_activity_container.time_on_activity_thresholds.Count;

                    //send the indicator and its index to be stored or have its occurence incremented (if it already exists)
                    indicatorsObject.StoreMainIndicator(time_on_activity_threshold_ID);

                    temp_activity_container.current_task_time_limit_index++;
                }

            }


            /*//compare number of attempts limit


            int i = 0;
            foreach (var number_of_attempts_limit in temp_activity_container.limits_to_number_of_tries_to_solve)
            {
                if (number_of_attempts_limit == temp_activity_container.number_of_tries_to_solve)
                {
                    if (!temp_activity_container.compared_limits_to_number_of_tries_to_solve[i])
                    {
                        temp_activity_container.compared_limits_to_number_of_tries_to_solve[i] = true;

                        //CheckRulesForHintMetrics(error.Key, ref would_be_hint);
                        would_be_hint = "Task: " + ", number_of_attempts_limit: "
                            + number_of_attempts_limit + ", number: " + temp_activity_container.number_of_tries_to_solve;

                    }
                }
                i++;
            }*/




        }
		
		

        public void CheckRulesForErrors(/*ref ActivityMetrics*/ string activity_ID, ref string would_be_hint)
		{
            ActivityMetrics activity_container = null;

            playerMetricsObject.gameActivitiesList.TryGetValue(activity_ID, out activity_container);

			//check the rules for the errors and their limits 
            foreach (var error in activity_container.EncouteredErrorsList)
			{
				int i=0;
				foreach(var error_limit in error.Value.error_number_thresholds)
				{
					if(error_limit==error.Value.GetNumberOfErrors())
					{
						//if(!error.Value.GetError_limit_compared(i))
						if(!error.Value.error_thresholds_compared[i])
						{
							error.Value.error_thresholds_compared[i] = true;
							//error.Value.SetError_limit_compared(i,true);

							//CheckRulesForHintMetrics(error.Key, ref would_be_hint);
							would_be_hint = "Error: "+error.Key.ToString()+", limit: "+error_limit+
								", number: "+error.Value.GetNumberOfErrors();

                            //send the indicator (per error type)
                            indicatorsObject.StoreMainIndicator(error.Key.ToString() + "Threshold");
						}
					}
					i++;
				}
			}


			
			//check the rules for the timestamps of the errors from previously 
			//and the average time in the game (measeured from previous playthroughs 

			//split the average time into 3 periods (early, middle, late)

			/*int hours = .Hours;
			int mins = playerMetricsObject.timeMetricsObject.average_time_since_beginning_of_game.Minutes;
			int secs = playerMetricsObject.timeMetricsObject.average_time_since_beginning_of_game.Seconds;*/


			//int average_time=(int) playerMetricsObject.timeMetricsObject.average_time_since_beginning_of_game.TotalSeconds;


			/*if(secs!=0)
			{
				average_time = secs;
			}
			if(mins!=0)
			{
				average_time += (mins*60);
			}
			if(hours!=0)
			{
				average_time += (hours*3600)
			}*/

			/*MIGHT NOT BE NEEDED
			 * int early_period_end = average_time/3;
			int middle_period_end = early_period_end*2;
			int late_period_end = average_time;

			foreach(var error_from_the_past in playerMetricsObject.activityMetricsObject.EncouteredErrorsList)
			{
				foreach(var error_timestamps_from_the_past in 
				        error_from_the_past.Value.error_time_stamps_from_previously)
				{
					if(error_timestamps_from_the_past.TotalSeconds<=early_period_end)
					{

					}
					else if(early_period_end<error_timestamps_from_the_past.TotalSeconds &&
					        error_timestamps_from_the_past.TotalSeconds<=middle_period_end)
					{

					}
					else if(middle_period_end<error_timestamps_from_the_past.TotalSeconds &&
					        error_timestamps_from_the_past.TotalSeconds<=late_period_end)
					{

					}
				}
			}*/


			/*for(int i=0;i<playerMetricsObject.EncouteredErrorsList.Count;i++)
			{
				for(int j=0;j<playerMetricsObject.EncouteredErrorsList.ElementAt(i).Value.error_number_limits.Count;j++)
				{
					if(playerMetricsObject.EncouteredErrorsList.ElementAt(i).Value.error_number_limits.ElementAt(j)
					   == playerMetricsObject.EncouteredErrorsList.ElementAt(i).Value.GetNumberOfErrors())
					{

						would_be_hint ="Error: "+playerMetricsObject.EncouteredErrorsList.ElementAt(i).Value 
							+" number:"+ playerMetricsObject.EncouteredErrorsList.ElementAt(i).Value.GetNumberOfErrors();
							
					}
				}
			}*/
		}

		public void CheckRulesForHintMetrics(string identifier, ref string would_be_hint)
		{
			foreach(var hint in playerMetricsObject.activityMetricsObject.givenHintsList)
			{
				if(identifier==hint.Value.hint_identifier)
				{
					hint.Value.HintGiven(playerMetricsObject.timeMetricsObject.time_since_beginning_of_game.Elapsed);

					int i=0;
					foreach(var hint_limit in hint.Value.hint_number_input_thresholds)
					{
						//if(hint_limit==hint.Value.GetHintCount())
						{
							if(!hint.Value.hint_limit_compared[i])
							{
								would_be_hint = "Hint: "+hint.Key.ToString()+", limit: "+hint_limit+
									", number: "+hint.Value.GetHintCount()+" ,hint identifiers: "+
										identifier+" "+hint.Value.hint_identifier;
								
								hint.Value.hint_limit_compared[i] = true;
								break;
							}
						}
						i++;
					}
				}
			}

		}


		public void CheckRulesForTimeMetrics(ref string would_be_hint)
		{
			if(playerMetricsObject.timeMetricsObject.current_game_time_limit_index+1<=
			   playerMetricsObject.timeMetricsObject.game_time_limits.Count)
			{
				CompareCurrentGameTimeWithGameTimeLimit();

				if (playerMetricsObject.timeMetricsObject.time_since_beginning_of_game.Elapsed
				    > playerMetricsObject.timeMetricsObject.game_time_limits.ElementAt
				    (playerMetricsObject.timeMetricsObject.current_game_time_limit_index) 
				    && playerMetricsObject.timeMetricsObject.GetGame_time_limit_compared
				    (playerMetricsObject.timeMetricsObject.current_game_time_limit_index)!= true)
				{
					
					playerMetricsObject.timeMetricsObject.SetGame_time_limit_compared
						(playerMetricsObject.timeMetricsObject.current_game_time_limit_index,true);
				}

				if(playerMetricsObject.timeMetricsObject.current_game_time_limit_index+1<=
				   playerMetricsObject.timeMetricsObject.game_time_limits.Count)
				{
					if (playerMetricsObject.timeMetricsObject.GetGame_time_limit_compared
					    (playerMetricsObject.timeMetricsObject.current_game_time_limit_index))
					{
						
						would_be_hint= "Game Over, man! Game Over! "
							+playerMetricsObject.timeMetricsObject.current_game_time_limit_index;
						
						playerMetricsObject.timeMetricsObject.current_game_time_limit_index++;
					}
					
				}
			}

		}

		public void CompareCurrentGameTimeWithGameTimeLimit()
		{
			//            if (playerMetricsObject.timeMetricsObject.GetCountTimeSinceBeginningOfGame()
			//                > playerMetricsObject.timeMetricsObject.GetGameTimeLimitInput())
			if(playerMetricsObject.timeMetricsObject.time_since_beginning_of_game.Elapsed
			   > playerMetricsObject.timeMetricsObject.game_time_limits.ElementAt
			   (playerMetricsObject.timeMetricsObject.current_game_time_limit_index)
			   && playerMetricsObject.timeMetricsObject.GetGame_time_limit_compared
			   (playerMetricsObject.timeMetricsObject.current_game_time_limit_index)!= true) 

			{
				
				playerMetricsObject.timeMetricsObject.SetGame_time_limit_compared
					(playerMetricsObject.timeMetricsObject.current_game_time_limit_index,true);
				//SetGameTimeLimitReached(true);
				
				//game_time_limit_reached=true;
				
				/*if(playerMetricsObject.timeMetricsObject.current_game_time_limit_index
				   <playerMetricsObject.timeMetricsObject.game_time_limits.Count)
				{
					//make sure that next time the following index will be used
					playerMetricsObject.timeMetricsObject.current_game_time_limit_index++;
				}*/
			}
		}


		
	

//		previous versions!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//		//
//		public void CheckRulesForActivityMetrics(ref string would_be_hint)
//		{
//			if(playerMetricsObject.activityMetricsObject.current_task_time_limit_index+1<=
//			   playerMetricsObject.activityMetricsObject.time_on_activity_thresholds.Count)
//			{
//				
//				//CompareCurrentTaskTimeWithTaskTimeLimit();
//				//-----------------
//				if (playerMetricsObject.activityMetricsObject.time_on_activity.Elapsed
//				    > playerMetricsObject.activityMetricsObject.time_on_activity_thresholds.ElementAt
//				    (playerMetricsObject.activityMetricsObject.current_task_time_limit_index) 
//				    && playerMetricsObject.activityMetricsObject.GetTask_time_limit_compared
//				    (playerMetricsObject.activityMetricsObject.current_task_time_limit_index)!= true)
//				{
//				
//					playerMetricsObject.activityMetricsObject.SetTask_time_limit_compared
//							(playerMetricsObject.activityMetricsObject.current_task_time_limit_index,true);
//				}
//
//				//-------------
//			}
//
//
//			if(playerMetricsObject.activityMetricsObject.current_task_time_limit_index+1<=
//			   playerMetricsObject.activityMetricsObject.time_on_activity_thresholds.Count)
//			{
//				if (playerMetricsObject.activityMetricsObject.GetTask_time_limit_compared
//				    (playerMetricsObject.activityMetricsObject.current_task_time_limit_index))
//				{
//					
//					would_be_hint= "Task Over, man! Task Over! "+
//						playerMetricsObject.activityMetricsObject.current_task_time_limit_index;
//					
//					playerMetricsObject.activityMetricsObject.current_task_time_limit_index++;
//				}
//				
//			}
//		}
//
//
//		//previous  versions!!!!!!!!!!!!!
//        public void CompareCurrentTaskTimeWithTaskTimeLimit()
//        {
//			/*if (playerMetricsObject.activityMetricsObject.time_on_activity.Elapsed
//			    > playerMetricsObject.activityMetricsObject.time_on_activity_thresholds.ElementAt
//			    (playerMetricsObject.activityMetricsObject.current_task_time_limit_index) 
//			    && playerMetricsObject.activityMetricsObject.time_on_activity_threshold_compared.ElementAt
//			    (playerMetricsObject.activityMetricsObject.current_task_time_limit_index) != true)*/
//			if (playerMetricsObject.activityMetricsObject.time_on_activity.Elapsed
//			    > playerMetricsObject.activityMetricsObject.time_on_activity_thresholds.ElementAt
//			    (playerMetricsObject.activityMetricsObject.current_task_time_limit_index) 
//			    && playerMetricsObject.activityMetricsObject.GetTask_time_limit_compared
//			    (playerMetricsObject.activityMetricsObject.current_task_time_limit_index)!= true)
//            {
//                //SetTaskTimeLimitReached(true);
//
//				//task_time_limit_reached=true;
//
//				/*if(playerMetricsObject.activityMetricsObject.current_task_time_limit_index
//				   <playerMetricsObject.activityMetricsObject.time_on_activity_thresholds.Count)*/
//				{
//					//make sure that next time the following index will be used
//
//					//this probably shoudn't be done here; maybe do this in ActivityMetrics
//
//
//					playerMetricsObject.activityMetricsObject.SetTask_time_limit_compared
//						(playerMetricsObject.activityMetricsObject.current_task_time_limit_index,true);
//
//					//moved in Timer for now
////					if(playerMetricsObject.activityMetricsObject.current_task_time_limit_index+1<=
////					   playerMetricsObject.activityMetricsObject.time_on_activity_thresholds.Count-1)
////						playerMetricsObject.activityMetricsObject.current_task_time_limit_index++;
//				}
////				else
////				{
////					playerMetricsObject.activityMetricsObject.time_on_activity_threshold_compared = true;
////				}
//            }
//        }


        
        public void CheckIfHintShouldBeGiven()
        {
            if(game_time_limit_reached)
            {
				//do something; indicate a hint should be given
                //????? and reset the flag???????????????//
            }
        }







    }
}
