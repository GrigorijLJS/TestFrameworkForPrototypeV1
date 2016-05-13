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
using System;
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

        public bool CheckCorrectnessOfAnswer(string playersAnswer, string correctAnswer, int new_points,
            ref string hint)
        {

            //split the strings into separate words
            string[] player_answers = playersAnswer.Split(' ');
            string[] correct_answers = correctAnswer.Split(' ');

            if (player_answers.Length > 1)
            {
                int number_of_correct_items = 0;
                foreach (string corr_answer in correct_answers)
                {
                    foreach (string play_answ in player_answers)
                    {
                        int distance_between_strings = ComputeDistance(corr_answer, play_answ);
                        float string_similarity=0;

                        //similarity is based on the formulas from 
                        //http://hpi.de/fileadmin/user_upload/fachgebiete/naumann/folien/SS13/DPDC/DPDC_12_Similarity.pdf
                        if(corr_answer.Length>play_answ.Length)
                        {
                            string_similarity = 1 - ((float)distance_between_strings / (float)corr_answer.Length);
                        }
                        else if(corr_answer.Length<play_answ.Length)
                        {
                            string_similarity = 1 - ((float)distance_between_strings / (float)play_answ.Length);
                        }
                        else
                        {
                            string_similarity = 1 - ((float)distance_between_strings / (float)play_answ.Length);
                        }

                        if (play_answ.Equals(corr_answer, StringComparison.OrdinalIgnoreCase))
                        {
                            number_of_correct_items++;
                        }
                    }
                }

                if (number_of_correct_items == correct_answers.Length && player_answers.Length == correct_answers.Length)
                {//correct answer, i.e. the strings player_answers and correct_answers contain the same items


                    hint = "Correct! You gained " + new_points
                        + " points!\n";
                    return true;
                }
                else
                {//incorrect answer and an error in classification 

                    //save half of the question's points and use them later to subtract points from the game score
                    // new_points = currentQuestion.questionScore;

                    hint = "Wrong answer! The correct answer was: "
                        + correctAnswer + ". You lost " + new_points
                        + " points! \n";
                    return false;
                }
            }
            else if (player_answers.Length == 1)
            {

                int distance_between_strings = ComputeDistance(correctAnswer, playersAnswer);
                float string_similarity = 0;

                //similarity is based on the formulas from 
                //http://hpi.de/fileadmin/user_upload/fachgebiete/naumann/folien/SS13/DPDC/DPDC_12_Similarity.pdf      
                if (correctAnswer.Length > playersAnswer.Length)
                {
                    string_similarity = 1 - ((float)distance_between_strings / (float)correctAnswer.Length);
                }
                else if (correctAnswer.Length < playersAnswer.Length)
                {
                    string_similarity = 1 - ((float)distance_between_strings / (float)playersAnswer.Length);
                }
                else
                {
                    string_similarity = 1 - ((float)distance_between_strings / (float)playersAnswer.Length);
                }

                if (playersAnswer.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase))
                {//correct answer, i.e. the strings are equal

                    hint = "Correct! You gained " + new_points
                        + " points!\n";
                    return true;
                }
                else
                {

                    hint = "Wrong answer! The correct answer was: "
                        + correctAnswer + ". You lost " + new_points
                        + " points! \n";
                    return false;
                }
            }
            else
            {
                //error
                return false;
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

        public void CheckThresholdsForMainIndicators()
        {
            foreach (var main_indicator in indicatorsObject.mainIndicators)
            {
                foreach (var main_indicator_threshold in indicatorsObject.mainIndicators_thresholds)
                {
                    if (main_indicator.Value == main_indicator_threshold)
                    {
                        indicatorsObject.StoreAuxiliaryIndicator(main_indicator.Key);

                        //AND SEND A "HINT" FOR THE AUX. INDICATORS! ALSO FOR THE MAIN ONES???
#warning add an indicator, well print it somewhere so it can be seen - like indicator_tralala
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
            if ((playerMetricsObject.game_score - playerMetricsObject.game_score_previous_state) > 0)
            {
                score_increase_or_decrease_indicator_ID = "scoreIncrease";
            }
            else if ((playerMetricsObject.game_score - playerMetricsObject.game_score_previous_state) < 0)
            {
                score_increase_or_decrease_indicator_ID = "scoreDecrease";
            }

            //the absolute value of the change of the score
            int absolute_score = Math.Abs(playerMetricsObject.game_score - playerMetricsObject.game_score_previous_state);

            //loop through the inflation/deflation triggers (threholds) of the score
            for (int i = 0; i < playerMetricsObject.game_score_triggers.Count; i++)
            {
                //if this is not the last inflation/deflation trigger
                if ((i + 1) < playerMetricsObject.game_score_triggers.Count)
                {
                    if (absolute_score >= playerMetricsObject.game_score_triggers.ElementAt(i) &&
                       absolute_score < playerMetricsObject.game_score_triggers.ElementAt(i + 1))
                    {//check if the change to the score is between the current trigger and the next one

                        //the indicator printed in the game
                        score_indicator = "Score indicator " + (i) + " activated!";

                        //attach the value that activated the indicator
                        score_increase_or_decrease_indicator_ID += ";" + playerMetricsObject.game_score_triggers.ElementAt(i);
                        //send the main indicator 
                        indicatorsObject.StoreMainIndicator(score_increase_or_decrease_indicator_ID);
                        //and check if an auxiliary indicator should be activated
                        CheckThresholdsForMainIndicators();
                    }

                }
                else //if it is the last inflation/deflation trigger
                {
                    if (absolute_score >= playerMetricsObject.game_score_triggers.ElementAt(i))
                    {//just comapre the change to the trigger

                        //the indicator printed in the game
                        score_indicator = "Score indicator " + (i) + " activated!";

                        //attach the value that activated the indicator
                        score_increase_or_decrease_indicator_ID += ";" + playerMetricsObject.game_score_triggers.ElementAt(i);
                        //and send the main indicator 
                        indicatorsObject.StoreMainIndicator(score_increase_or_decrease_indicator_ID);
                        //and check if an auxiliary indicator should be activated
                        CheckThresholdsForMainIndicators();


                    }
                }
            }
        }


        public void CheckRulesForActivityMetrics(ref ActivityMetrics temp_activity_container, ref string would_be_hint)
        {
            string time_on_activity_threshold_ID = "timeOnActivityThreshold";
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
                        (temp_activity_container.current_task_time_limit_index + 1)
                        + " out of " + temp_activity_container.time_on_activity_thresholds.Count;

                    //send the indicator and its index to be stored or have its occurence incremented (if it already exists)
                    indicatorsObject.StoreMainIndicator(time_on_activity_threshold_ID);
                    //and check if an auxiliary indicator should be activated
                    CheckThresholdsForMainIndicators();

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
                int i = 0;
                foreach (var error_limit in error.Value.error_number_thresholds)
                {
                    if (error_limit == error.Value.GetNumberOfErrors())
                    {
                        //if(!error.Value.GetError_limit_compared(i))
                        if (!error.Value.error_thresholds_compared[i])
                        {
                            error.Value.error_thresholds_compared[i] = true;
                            //error.Value.SetError_limit_compared(i,true);

                            //CheckRulesForHintMetrics(error.Key, ref would_be_hint);
                            would_be_hint = "Error: " + error.Key.ToString() + ", limit: " + error_limit +
                                ", number: " + error.Value.GetNumberOfErrors();

                            //send the indicator (per error type)
                            indicatorsObject.StoreMainIndicator(error.Key.ToString() + "Threshold");

                            //and check if an auxiliary indicator should be activated
                            CheckThresholdsForMainIndicators();
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
            foreach (var hint in playerMetricsObject.activityMetricsObject.givenHintsList)
            {
                if (identifier == hint.Value.hint_identifier)
                {
                    hint.Value.HintGiven(playerMetricsObject.timeMetricsObject.time_since_beginning_of_game.Elapsed);

                    int i = 0;
                    foreach (var hint_limit in hint.Value.hint_number_input_thresholds)
                    {
                        //if(hint_limit==hint.Value.GetHintCount())
                        {
                            if (!hint.Value.hint_limit_compared[i])
                            {
                                would_be_hint = "Hint: " + hint.Key.ToString() + ", limit: " + hint_limit +
                                    ", number: " + hint.Value.GetHintCount() + " ,hint identifiers: " +
                                        identifier + " " + hint.Value.hint_identifier;

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
            if (playerMetricsObject.timeMetricsObject.current_game_time_limit_index + 1 <=
               playerMetricsObject.timeMetricsObject.game_time_limits.Count)
            {
                CompareCurrentGameTimeWithGameTimeLimit();

                if (playerMetricsObject.timeMetricsObject.time_since_beginning_of_game.Elapsed
                    > playerMetricsObject.timeMetricsObject.game_time_limits.ElementAt
                    (playerMetricsObject.timeMetricsObject.current_game_time_limit_index)
                    && playerMetricsObject.timeMetricsObject.GetGame_time_limit_compared
                    (playerMetricsObject.timeMetricsObject.current_game_time_limit_index) != true)
                {

                    playerMetricsObject.timeMetricsObject.SetGame_time_limit_compared
                        (playerMetricsObject.timeMetricsObject.current_game_time_limit_index, true);
                }

                if (playerMetricsObject.timeMetricsObject.current_game_time_limit_index + 1 <=
                   playerMetricsObject.timeMetricsObject.game_time_limits.Count)
                {
                    if (playerMetricsObject.timeMetricsObject.GetGame_time_limit_compared
                        (playerMetricsObject.timeMetricsObject.current_game_time_limit_index))
                    {

                        would_be_hint = "Game Over, man! Game Over! "
                            + playerMetricsObject.timeMetricsObject.current_game_time_limit_index;

                        playerMetricsObject.timeMetricsObject.current_game_time_limit_index++;
                    }

                }
            }

        }

        public void CompareCurrentGameTimeWithGameTimeLimit()
        {
            //            if (playerMetricsObject.timeMetricsObject.GetCountTimeSinceBeginningOfGame()
            //                > playerMetricsObject.timeMetricsObject.GetGameTimeLimitInput())
            if (playerMetricsObject.timeMetricsObject.time_since_beginning_of_game.Elapsed
               > playerMetricsObject.timeMetricsObject.game_time_limits.ElementAt
               (playerMetricsObject.timeMetricsObject.current_game_time_limit_index)
               && playerMetricsObject.timeMetricsObject.GetGame_time_limit_compared
               (playerMetricsObject.timeMetricsObject.current_game_time_limit_index) != true)
            {

                playerMetricsObject.timeMetricsObject.SetGame_time_limit_compared
                    (playerMetricsObject.timeMetricsObject.current_game_time_limit_index, true);
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
            if (game_time_limit_reached)
            {
                //do something; indicate a hint should be given
                //????? and reset the flag???????????????//
            }
        }







    }
}




///////////////////////////old code "Rules"
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Prototype1v1
//{
//    public class Rules
//    {
//        public Metrics metricsObject;


//        //fields 

//        //
//        private bool game_time_limit_reached = false;

//        //
//        private bool task_time_limit_reached = false;



//        //constructors 

//        public Rules()
//        {
//            metricsObject = new Metrics();
//        }



//        //getters/setters

//        public void SetMetricsObject(Metrics new_value)
//        {
//            metricsObject = new_value;
//        }


//        public bool GetGameTimeLimitReached()
//        {
//            return game_time_limit_reached;
//        }

//        public void SetGameTimeLimitReached(bool new_value)
//        {
//            game_time_limit_reached = new_value;
//        }

//        public bool GetTaskTimeLimitReached()
//        {
//            return task_time_limit_reached;
//        }

//        public void SetTaskTimeLimitReached(bool new_value)
//        {
//            task_time_limit_reached = new_value;
//        }


//        //methods 


//        public void CompareCurrentTaskTimeWithTaskTimeLimit()
//        {
//            if (metricsObject.taskMetricsObject.GetTimeOnTask()
//                > metricsObject.taskMetricsObject.GetTaskTimeLimitInput())
//            {
//                SetTaskTimeLimitReached(true);
//            }
//        }


//        public void CompareCurrentGameTimeWithGameTimeLimit()
//        {
//            if (metricsObject.timeMetricsObject.GetCountTimeSinceBeginningOfGame()
//                > metricsObject.timeMetricsObject.GetGameTimeLimitInput())
//            {
//                SetGameTimeLimitReached(true);
//            }
//        }


//        public void CheckIfHintShouldBeGiven()
//        {
//            if(game_time_limit_reached)
//            {
//                //do something
//                //????? and reset the flag???????????????//
//            }
//        }







//    }
//}
