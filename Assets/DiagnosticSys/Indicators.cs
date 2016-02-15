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

        public Dictionary<string, int> auxiliaryIndicators
        {
            get;
            set;
        }

        public Indicators()
        {
            mainIndicators = new Dictionary<string, int>();
            auxiliaryIndicators = new Dictionary<string, int>();

            //initialize the indicators in the dictionaries
            //mainIndicators.Add();
            //auxiliaryIndicators.Add();
        }

        public void StoreMainIndicator(string type_of_main_indicator)
        {
            
        }

        public void StoreAuxiliaryIndicator(string type_of_aux_indicator)
        {

        }
    }
}
