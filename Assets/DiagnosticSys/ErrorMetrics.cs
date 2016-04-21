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

//this seems to create issues
//using System.Threading.Tasks;

namespace Prototype1v1
{
    //class used in the errors collection
    //stores error by type (the string in the dictionary in the class PlayerMetrics)
    public class ErrorMetrics// : IMetrics
    {
        /*//the number of errors (of a certain type) that were made by the player
        //!!!!!!!!!!!!!!!!!!!can be replaced with list.count
        private int errorCount = 0;*/

        //a collection of timestamps when this type of error was made
        public List<TimeSpan> error_time_stamps 
		{
			get;
			set;
		}

		public List<int> error_number_thresholds 
		{
			get;
			set;
		}

		//!!!!!!!!!!!!!!!!!!!!CREATES ISSUES WHEN IT IS A PROPERTY
		//private List<bool> error_thresholds_compared = new List<bool>();
		/*{
			get;
			set;
		}*/

		/*static*/public List<bool> error_thresholds_compared 
		{
			get;
			set;
		}

		//contains the timestamps for this error that have happened 
		//on previous occasions when the player has played the game
		public List<TimeSpan> error_time_stamps_from_previously 
		{
			get;
			set;
		}
		//error_thresholds_compared[1]=true;

		/*public bool GetError_limit_compared(int index)
		{
			return error_thresholds_compared.ElementAt(index);
		}
		
		public void SetError_limit_compared(int an_index, bool new_value)
		{
			error_thresholds_compared.RemoveAt(an_index);
			error_thresholds_compared.Insert(an_index, new_value);
		}*/

		public List<int> default_error_number_thresholds 
		{
			get;
			set;
		}

		
		public List<bool> default_error_thresholds_compared 
		{
			get;
			set;
		}

        /*might not be needed
         * //!!!!!!!!!!!!!!!!!maybe just a bool that says that this error (name of error is in metrics anyway)
        //!!!!!!!!!!!!!!!!!!!! is persistent; needs some default limit but it can also be suplied by users
        //!!!!!!!!!!!!!!!!!!!!!!
        private List<TimeSpan> persistent_errors = new List<TimeSpan>();*/

        //constructor
        public ErrorMetrics()
        {
			error_time_stamps = new List<TimeSpan>();
			error_number_thresholds = new List<int>();
			error_thresholds_compared = new List<bool>();

			default_error_thresholds_compared = new List<bool>();
			default_error_number_thresholds = new List<int>();

			/*//harcoded for now
			error_time_stamps_from_previously = new List<TimeSpan>();
			error_time_stamps_from_previously.Add(new TimeSpan(0,1,0));
			error_time_stamps_from_previously.Add(new TimeSpan(0,1,30));
			error_time_stamps_from_previously.Add(new TimeSpan(0,0,30));*/
        }

        public ErrorMetrics(TimeSpan timestamp)
            :this()
        {
            ErrorMade(timestamp);
        }

        //a function that returns the number of occurences of this error
        public int GetNumberOfErrors()
        {
            return error_time_stamps.Count;
        }

        /*public List<TimeSpan> GetTimeStamps()
        {
            return error_time_stamps;
        }*/


        //public bool IsTheErrorPersistent()
        //{
        //    return persistent_errors;
        //}



        //methods


        //a function used to store the time when the error was made 
        public void ErrorMade(TimeSpan timestamp)
        {
            error_time_stamps.Add(timestamp);
        }

		//a function that stores the gradual limits to the number 
		//of times this error was encountered
		public void StoreInputThresholdsToNumberOfErrors(int limit)
		{
			error_number_thresholds.Add(limit);
			error_thresholds_compared.Add(false);
		}

		//function for adding 3 default limits to the number of errors
		//and a look-up list to check if any one of them has already been reached 
		public void InitializeTheDefaultThresholdsToTheNumberOfErrors()
		{
			default_error_number_thresholds.Add(3);
			default_error_thresholds_compared.Add(false);
			default_error_number_thresholds.Add(6);
			default_error_thresholds_compared.Add(false);
			default_error_number_thresholds.Add(9);
			default_error_thresholds_compared.Add(false);
		}


    }
}
