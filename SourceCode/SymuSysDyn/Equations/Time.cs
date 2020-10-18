﻿#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Globalization;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    ///     TIME built in function
    /// </summary>
    public class Time : BuiltInFunction
    {
        public const string Value = "Time";

        public Time() : base(Value)
        {
        }

        public override string Prepare(string word, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            return sim.Time.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Check if it is a Time function
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsContainedIn(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var inputLower = input.ToLowerInvariant();

            return inputLower.Contains(Value.ToLowerInvariant()) &&
                   !inputLower.Contains("_" + Value.ToLowerInvariant()) &&
                   !inputLower.Contains(Value.ToLowerInvariant() + "_");
        }
    }
}