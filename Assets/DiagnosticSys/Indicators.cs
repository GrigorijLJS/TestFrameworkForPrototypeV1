using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using System.Threading.Tasks;

namespace Prototype1v1
{
    public class Indicators
    {
        public Dictionary<string, int> mainIndicators
        {
            get;
            set;
        }

        public List<int> mainIndicators_thresholds
        {
            get;
            set;
        }

        public List<bool> mainIndicators_threshold_compared
        {
            get;
            set;
        }

        public List<int> mainIndicators_default_thresholds
        {
            get;
            set;
        }

        public List<bool> mainIndicators_threshold_default_compared
        {
            get;
            set;
        }
        public Dictionary<string, int> auxiliaryIndicators
        {
            get;
            set;
        }


        /*//the IDs of the indicators that are to be used in the rules
        public string score_increase_indicator_ID 
        {
            get;
            set;
        }

        public string score_decrease_indicator_ID
        {
            get;
            set;
        }


        public string time_on_activity_indicator_ID
        {
            get;
            set;
        }

        public string error_Recall_indicator_ID
        {
            get;
            set;
        }

        public string error_Recognize_indicator_ID
        {
            get;
            set;
        }

        public string error_Implement_indicator_ID
        {
            get;
            set;
        }

        public string error_Classify_indicator_ID
        {
            get;
            set;
        }*/



        public Indicators()
        {
            mainIndicators = new Dictionary<string, int>();
            auxiliaryIndicators = new Dictionary<string, int>();

            mainIndicators_threshold_compared = new List<bool>();
            mainIndicators_thresholds = new List<int>();
            mainIndicators_default_thresholds = new List<int>();
            mainIndicators_threshold_default_compared = new List<bool>();

            //initialization of the indicators in the dictionaries
            /*mainIndicators.Add();
            auxiliaryIndicators.Add();*/

            /*score_increase_indicator_ID = "scoreIncreaseTrigger";
            score_decrease_indicator_ID = "scoreDecreaseTrigger";*/

            //time_on_activity_indicator_ID = "activityTimeThreshold";

            /*error_Classify_indicator_ID = "errorClassifyThreshold";
            error_Implement_indicator_ID = "errorImplementThreshold";
            error_Recall_indicator_ID = "errorRecallThreshold";
            error_Recognize_indicator_ID = "errorRecognizeThreshold";*/
            
        }

        //gets the name of the main indiactor that was activated, stores it or increments the number of occurrences
        public void StoreMainIndicator(string main_indicator_ID)
        {
            //string[] separated_items = main_indicator_ID_and_trigger_value.Split(';');

            if (mainIndicators.ContainsKey(main_indicator_ID))
            {
                // the indicator already exists in the collection, so increment the occurences by 1
                mainIndicators[main_indicator_ID] += 1;
            }
            else
            {
                //the indicator does not exist, so add it and set the number of occurences to 1
                mainIndicators.Add(main_indicator_ID, 1);
            }
        }

        //gets the name of the auxiliary indiactor that was activated, stores it or increments the number of occurrences
        public void StoreAuxiliaryIndicator(string aux_indicator_ID)
        {
            if (auxiliaryIndicators.ContainsKey(aux_indicator_ID))
            {
                // the indicator already exists in the collection, so increment the occurences by 1
                auxiliaryIndicators[aux_indicator_ID] += 1;
            }
            else
            {
                //the indicator does not exist, so add it and set the number of occurences to 1
                auxiliaryIndicators.Add(aux_indicator_ID, 1);
            }
        }
    }
}
