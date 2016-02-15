using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using System.Threading.Tasks;

namespace Prototype1v1
{
    //class used in the given hints collection 
    //stores hint by type (the string in the dictionary in the class PlayerMetrics)
    public class HintMetrics// : IMetrics
    {
        /*//the number of hints (of a certain type) that were given
        //!!!!!!!!!!!!!!!!!!!can be replaced with list.count
        private int hintCount = 0;*/

        //the timestamps when this type of hint was given
        public List<TimeSpan> hint_time_stamps 
		{
			get;
			set;
		}

		public List<int> hint_number_input_thresholds 
		{
			get;
			set;
		}

		public List<bool> hint_limit_compared 
		{
			get;
			set;
		}

		//used to identify the reason why this hint was given
		//useful for look-up procedures
		public string hint_identifier
		{
			get;
			set;
		}


		//--------------
		//these two could be useful for later on
		public List<int> default_hint_number_limits 
		{
			get;
			set;
		}
		
		
		public List<bool> default_hint_limit_compared 
		{
			get;
			set;
		}
		//--------------

        //constructor
        public HintMetrics()
        {
			hint_limit_compared = new List<bool>();
			hint_number_input_thresholds = new List<int>();
			hint_time_stamps = new List<TimeSpan>();

			default_hint_limit_compared = new List<bool>();
			default_hint_number_limits = new List<int>();
        }

		public HintMetrics(TimeSpan timestamp)
			:this()
		{
			HintGiven(timestamp);
		}

        //getters/setters
        public int GetHintCount()
        {
            //!!!!!!!!!!!!!!!!!!!can be replaced with list.count
            return hint_time_stamps.Count;
        }


        

        //methods

        //a function used to store the time when the hint was given 
        public void HintGiven(TimeSpan timestamp)
        {
            if (hint_time_stamps!= null)
            {
                hint_time_stamps.Add(timestamp);

            }
        }

		//a function that stores the gradual limits to the number 
		//of times this hint was given
		public void StoreInputThresholdsToNumberOfHints(int limit)
		{
			hint_number_input_thresholds.Add(limit);
			hint_limit_compared.Add(false);
		}
		
		//function for adding 3 default limits to the number of hints
		//and a look-up list to check if any one of them has already been reached 
		public void InitializeTheDefaultLimitsToTheNumberOfHints()
		{
			default_hint_number_limits.Add(3);
			default_hint_limit_compared.Add(false);
			default_hint_number_limits.Add(6);
			default_hint_limit_compared.Add(false);
			default_hint_number_limits.Add(9);
			default_hint_limit_compared.Add(false);
		}



    }
}
