﻿#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     STEP built in function
    ///     STEP: Generate a step increase(or decrease) at the given time
    ///     Parameters: 2: (height, start time); step up/ down at start time
    /// </summary>
    /// <example>STEP(6, 3) steps from 0 to 6 at time 3(and stays there)</example>
    public class Step : BuiltInFunction
    {
        public const string Value = "Step";

        public Step(string function) : base(function)
        {
            Height = Parameters[0].Variables.Any() ? Parameters[0].Variables.First() : Parameters[0].OriginalEquation.Trim();
            StartTime = Parameters[1].Variables.Any() ? Parameters[1].Variables.First() : Parameters[1].OriginalEquation.Trim();
        }

        public string Height { get; }
        public string StartTime { get; }

        public override float Evaluate(Variables variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }
            //Height can be either a literal or a numeric
            var height = Parameters[0].Variables.Any() ? Convert.ToSingle(Expression.Parameters[Height]) : Convert.ToSingle(Height, CultureInfo.InvariantCulture);

            var startTime = Parameters[1].Variables.Any() ? Convert.ToUInt16(Expression.Parameters[StartTime]) : Convert.ToUInt16(StartTime, CultureInfo.InvariantCulture);

            return sim.Time >= startTime ? height : 0;
        }
    }
}