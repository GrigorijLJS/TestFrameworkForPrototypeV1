using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype1v1
{
    public class Logger
    {
        public DiagnosticRules rulesObj
        {
            get;
            set;
        }

        //a function used for writing all the data from the metrics to a .txt file when the game is finished
        public void WriteLog()
        {
            using (System.IO.StreamWriter output_file =
            new System.IO.StreamWriter(@"C:\Users\GSV\Desktop\diagnosticSystemLog.txt"))
            {
                string temp_logger = "";
                output_file.WriteLine("---------!!! DATA LOG FOR THE DIAGNOSTIC SYSTEM !!!---------\n\n");
                output_file.WriteLine("player name: "+rulesObj.playerMetricsObject.player_name);
                output_file.WriteLine("game score:" + rulesObj.playerMetricsObject.game_score);

                for( int i=0;i<rulesObj.playerMetricsObject.game_score_default_triggers.Count;i++)
                {
                    temp_logger+=rulesObj.playerMetricsObject.game_score_default_triggers.ElementAt(i)+" ";
                }
                output_file.WriteLine("\ngame score default triggers:" + temp_logger);
                temp_logger = "";

                for (int i = 0; i < rulesObj.playerMetricsObject.game_score_triggers.Count; i++)
                {
                    temp_logger += rulesObj.playerMetricsObject.game_score_triggers.ElementAt(i) + " ";
                }
                output_file.WriteLine("\ngame score input triggers:" + temp_logger);
                temp_logger = "";

                output_file.WriteLine("\ntime spent in-game: " + rulesObj.playerMetricsObject.
                    timeMetricsObject.stored_time_since_beginning_of_game);

                output_file.WriteLine("\n\n--------game activities data:");
                foreach (var activity in rulesObj.playerMetricsObject.gameActivitiesList)
                {
                    output_file.WriteLine("\n\nactivity name: "+activity.Key.ToString());
                    output_file.WriteLine("Errors per type:");
                    foreach(var error in activity.Value.EncouteredErrorsList)
                    {
                        output_file.WriteLine("\nError: " + error.Key.ToString());
                        for (int i = 0; i < error.Value.error_number_thresholds.Count; i++)
                        {
                            temp_logger += error.Value.error_number_thresholds.ElementAt(i) + " ";
                        }
                        output_file.Write("thresholds: "+temp_logger);
                        temp_logger = "";

                        for (int i = 0; i < error.Value.error_time_stamps.Count; i++)
                        {
                            temp_logger += error.Value.error_time_stamps.ElementAt(i) + " ";
                        }
                        output_file.WriteLine("number of occurrences for this error: " + error.Value.error_time_stamps.Count
                        + "; timestamps: " + temp_logger);
                        temp_logger = "";
                    }

                    temp_logger = "\ntime(s) spent on this activity: ";
                    foreach(var timestamps in activity.Value.list_of_times_on_activity)
                    {
                        temp_logger += timestamps.ToString()+" ";
                    }
                    output_file.WriteLine(temp_logger);
                    temp_logger = "";

                    
                    temp_logger = "input time thresholds: ";
                    foreach (var thresholds in activity.Value.time_on_activity_thresholds)
                    {
                        temp_logger += thresholds.ToString() + " ";
                    }
                    output_file.WriteLine(temp_logger);
                    temp_logger = "";

                    temp_logger = "default time thresholds: ";
                    foreach (var thresholds in activity.Value.default_time_on_activity_thresholds)
                    {
                        temp_logger += thresholds.ToString() + " ";
                    }
                    output_file.WriteLine(temp_logger);
                    temp_logger = "";
                }
                output_file.WriteLine("--------game activities data end\n\n");

                output_file.WriteLine("\n\n--------indicator data:");
                temp_logger = "main indicators activated: ";
                foreach (var main_indicator in rulesObj.indicatorsObject.mainIndicators)
                {
                    temp_logger += main_indicator.Key.ToString() + " occurred " + main_indicator.Value.ToString()+"  ";
                }
                output_file.WriteLine(temp_logger);
                temp_logger = "";
                temp_logger = "aux indicators activated: ";
                foreach (var aux_indicator in rulesObj.indicatorsObject.auxiliaryIndicators)
                {
                    temp_logger += aux_indicator.Key.ToString() + " occurred " + aux_indicator.Value.ToString() + "  ";
                }
                output_file.WriteLine(temp_logger);
                temp_logger = "";
            }

        }
    }
}
