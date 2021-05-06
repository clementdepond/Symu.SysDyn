using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symu.SysDyn.Core.Equations;

namespace Symu.SysDyn.Core.Equations
{
    class Macro
    {
        private static Equation equation;

        public static async Task<string> ParseAsync(string input)
        {
            if (!input.Contains("MACRO EXPRESSION("))
            {
                return input;
            }

            input.Replace("MACRO EXPRESSION(", "");
            input.Replace(")", "");
            var param = GetParameter(input);
            var factory = await EquationFactory.CreateInstanceAsync(string.Empty, param[0], null);
            var factory2 = await EquationFactory.CreateInstanceAsync(string.Empty, param[1], null);

            return factory.Equation.ToString().Replace(param[1],factory2.Value.ToString());
        }

        public static string[] GetParameter(string resultat)
        {
            string[] array = new string[2];
            var i = 0;
            string x = resultat.ToString();
            while (x != ",")
            {
                i += 1;
                x = resultat[i].ToString();
            }

            array[0] = resultat.Substring(0, i);
            array[1] = resultat.Substring(i + 1, resultat.Length - (i + 1));

            return array;
        }
    }
}
