using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using System.Threading.Tasks;

namespace Prototype1v1
{
    public class Utilities
    {
        
        

        //check if the string contains only numbers
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
