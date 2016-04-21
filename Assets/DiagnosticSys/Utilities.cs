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
