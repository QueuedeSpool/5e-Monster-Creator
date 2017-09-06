using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _5eMonsterCreator
{
    class InputParser
    {




        //This method is only designed to parse numbers and simple fractions.
        //Any other input will fail.
        public static bool tryParseCR(String _CR, out decimal _value)
        {
            double val;
            if (double.TryParse(_CR, out val))
            {
                _value = (decimal)val;
                return true;
            }
            else
            {
                String[] fraction = _CR.Split(new[] { '/' });
                if (fraction.Length == 2)
                {
                    double parsedNumerator, parsedDenominator;

                    if (double.TryParse(fraction[0], out parsedNumerator) && double.TryParse(fraction[1], out parsedDenominator))
                    {
                        if (parsedDenominator != 0)
                        {
                            _value = (decimal)(parsedNumerator / parsedDenominator);

                            return true;
                        }
                    }
                }
            }

            _value = -1;
            return false;
        }
    }
}
