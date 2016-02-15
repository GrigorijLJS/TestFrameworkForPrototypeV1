using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using System.Threading.Tasks;

namespace Prototype1v1
{
    public class Utilities
    {

        /// <summary>
        /// methods
        /// these are utility functions 
        /// </summary> 
        /// 

        //a function used for writing all the data from the metrics to a .txt file
        //used as a log file
        //!!!!!!!!!!!!!!!!!!!!!!!!change the path to one that makes actual sense or serialization
        //!!!!!!!!!!!!!!!!!!!!maybe should be in the utility class
        public void WriteLog()
        {
            //using (System.IO.StreamWriter output_file =
            //new System.IO.StreamWriter(@"C:\test.txt"))
            //{
            //    //foreach (string line in lines)
            //    //{
            //    //    output_file.WriteLine(line);
            //    //}
            //    output_file.WriteLine("Log of the metrics:\n\n");
            //}
        }

        //check if the string contains only numbers
        //used for the 
        public bool IncludesDigitsOnly(string input_string)
        {
            foreach (char c in input_string)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        //a function to replace string.IsNullOrWhiteSpace
        //since I cannot use that string utility within Unity
        public bool ConsistsOfWhiteSpace(string input_string)
        {
            if(input_string == null)
            {
                return false;
            }
            foreach (char c in input_string)
            {
                if (c != ' ') return false;
            }
            return true;

        }
    }
}
