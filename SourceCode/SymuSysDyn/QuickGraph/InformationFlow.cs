﻿#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.SysDyn.Model;

#endregion

namespace Symu.SysDyn.QuickGraph
{
    /// <summary>
    ///     Specific implementation of VariableEdge
    /// </summary>
    public sealed class InformationFlow : VariableEdge
    {
        public InformationFlow(Variable source, Variable target) : base(source, target)
        {
        }
    }
}