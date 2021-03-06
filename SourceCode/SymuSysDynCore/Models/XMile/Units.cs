﻿#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.SysDyn.Core.Parser;

#endregion

namespace Symu.SysDyn.Core.Models.XMile
{
    /// <summary>
    ///     Class for units
    ///     Each variable OPTIONALLY has its own units of measure, which are specified by combining other units
    ///     defined in the units namespace
    /// </summary>
    public class Units
    {
        /// <summary>
        ///     In some cases, equation are defined as = Some_Value  {units}
        /// </summary>
        /// <param name="eqn"></param>
        /// <returns></returns>
        public static Units CreateInstanceFromEquation(string eqn)
        {
            if (string.IsNullOrEmpty(eqn))
            {
                return null;
            }

            var units = StringUtils.GetStringInBraces(eqn);
            return string.IsNullOrEmpty(units) ? null : new Units {Eqn = units};
        }

        #region Xml attributes

        public string Name { get; } //todo implement Units from XMile

        public string Eqn { get; set; }

        #endregion
    }
}