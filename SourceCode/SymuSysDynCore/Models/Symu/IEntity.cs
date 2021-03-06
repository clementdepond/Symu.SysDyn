﻿#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Symu.Common.Core.Interfaces;

#endregion

namespace Symu.SysDyn.Core.Models.Symu
{
    /// <summary>
    ///     Interface for ModelNetwork entity that encapsulate a Model
    /// </summary>
    public interface IEntity
    {
        IAgentId Id { get; }
        string Name { get; }
        Model Model { get; }

        /// <summary>
        ///     Get the model graph of the entity and all the sub/inline models of the model
        /// </summary>
        /// <returns></returns>
        Model GetModelGraph();

        IEntity Clone();
    }
}