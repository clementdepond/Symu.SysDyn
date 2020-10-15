﻿#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.SysDyn.Parser;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Model
{
    /// <summary>
    ///     Core building block of a model, also called level or state. Stocks accumulate change.
    ///     Use Stock when past event influence present event. Stocks are a kind of memory, storing the results of past actions.
    ///     Their value at the start of the simulation must be set as either a constant or with an initial equation.
    ///     The initial equation is evaluated only once, at the beginning of the simulation.
    /// </summary>
    public class Stock : Variable, IComparable
    {
        public Stock(string name, string eqn, List<string> inflow, List<string> outflow) : base(name, eqn)
        {
            Eqn = eqn;
            Inflow = StringUtils.CleanNames(inflow);
            Outflow = StringUtils.CleanNames(outflow);
            SetChildren();
        }
        public Stock(string name, string eqn, List<string> inflow, List<string> outflow, GraphicalFunction graph, Range range, Range scale) : base(name, eqn, graph, range, scale)
        {
            Eqn = eqn;
            Inflow = StringUtils.CleanNames(inflow);
            Outflow = StringUtils.CleanNames(outflow);
            SetChildren();
        }

        /// <summary>
        /// stock(t) = stock(t - dt) + dt*(inflows(t - dt) – outflows(t - dt))
        /// Re compute SetChildren with the new equation
        /// </summary>
        public void SetStockEquation()
        {
            var equation = Name ;
            var inflows = AggregateFlows(Inflow, Simulation.ManagedEquation.Plus);
            var outflows = AggregateFlows(Outflow, Simulation.ManagedEquation.Minus);
            if (inflows.Length > 0 || outflows.Length > 0)
            {
                equation += Simulation.ManagedEquation.Plus + "DT" + Simulation.ManagedEquation.Multiplication + Simulation.ManagedEquation.LParenthesis + inflows;
                if (outflows.Length > 0)
                {
                    equation += Simulation.ManagedEquation.Minus + outflows;
                }
                equation += Simulation.ManagedEquation.RParenthesis;
            }

            Equation = ManagedEquation.Initialize(equation);

            SetChildren();
        }

        private static string AggregateFlows(IReadOnlyList<string> list, string @operator)
        {
            var flow = string.Empty;
            for (var i = 0; i < list.Count; i++)
            {
                if (i == 0)
                {
                    flow += list[i];
                }
                else
                {
                    flow += @operator + list[i];
                }
            }

            return flow;
        }

        #region Xml attributes

        public List<string> Inflow { get; set; }
        public List<string> Outflow { get; set; }

        #endregion

        #region IComparable

        public override bool Equals(object that)
        {
            if (that is Variable stock)
            {
                return Name.Equals(stock.Name);
            }

            return ReferenceEquals(this, that);
        }

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        public int CompareTo(object obj)
        {
            var that = obj as Variable;
            return string.Compare(Name, that?.Name, StringComparison.Ordinal);
        }

        #endregion
    }
}