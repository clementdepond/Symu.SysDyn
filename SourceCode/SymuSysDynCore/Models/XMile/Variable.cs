﻿#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Equations;
using Symu.SysDyn.Core.Parser;

#endregion

namespace Symu.SysDyn.Core.Models.XMile
{
    /// <summary>
    ///     Default implementation of IVariable
    /// </summary>
    public class Variable : IVariable
    {
        public Variable()
        {
        }

        /// <summary>
        ///     Constructor for root model
        /// </summary>
        /// <param name="name"></param>
        public Variable(string name) : this(name, string.Empty)
        {
        }

        public Variable(string name, string model)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Model = model;
            Name = StringUtils.CleanName(name);
            FullName = StringUtils.FullName(Model, Name);
        }

        #region IVariable Members

        public float Value { get; set; }

        public Equation Equation { get; set; }

        public GraphicalFunction GraphicalFunction { get; set; }

        /// <summary>
        ///     The variable has been updated
        /// </summary>
        public bool Updated { get; set; }

        /// <summary>
        ///     The variable is being updating
        /// </summary>
        public bool Updating { get; set; }

        /// <summary>
        ///     Children are Equation's Variables except itself
        /// </summary>
        /// <remarks>Could be a computed property, but for performance, it is setted once</remarks>
        public List<string> Children { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return FullName;
        }

        public async Task Update(VariableCollection variables, SimSpecs simulation)
        {
            if (Updated)
            {
                return;
            }

            var eval = await Equation.Evaluate(this, variables, simulation);
            AdjustValue(eval);
            Updated = true;
        }

        public void Initialize()
        {
            Updated = Equation == null;
        }

        public virtual async Task<IVariable> Clone()
        {
            var clone = new Variable(Name, Model);
            await CopyTo(clone);
            return clone;
        }

        public bool Equals(string fullName)
        {
            return fullName == FullName;
        }

        public async Task<bool> TryOptimize(bool setInitialValue, SimSpecs sim)
        {
            if (Equation == null)
            {
                return true;
            }

            // No variables or itself
            var canBeOptimized = !Children.Any() && Equation.CanBeOptimized(FullName);

            if (!canBeOptimized)
            {
                return Equation == null;
            }

            if (setInitialValue)
            {
                if (Equation.Variables.Count == 1 && Equation.Variables[0] == FullName)
                {
                    Equation.Replace(FullName, Value.ToString(CultureInfo.InvariantCulture));
                }

                var value = await Equation.InitialValue(Model);
                AdjustValue(value);
            }

            Equation = null;

            return true;
        }

        /// <summary>
        ///     Model name
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        ///     Module and Connect are using FullName = ModelName.VariableName
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        ///     Adjust Value when a graphical function is defined
        /// </summary>
        /// <param name="value"></param>
        public void AdjustValue(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException();
            }

            // Graphical function
            Value = GraphicalFunction?.GetOutput(value) ?? value;
            if (NonNegative != null)
            {
                Value = NonNegative.GetOutputInsideRange(Value);
            }
        }

        /// <summary>
        ///     Find all children of a variable
        ///     Except itself
        /// </summary>
        /// <returns></returns>
        public void SetChildren()
        {
            Children = Equation?.Variables.Where(x => x != FullName).ToList() ?? new List<string>();
        }

        #endregion

        public static async Task<T> CreateVariable<T>(string name, string model, string eqn) where T : IVariable, new()
        {
            var variable = new T
            {
                Model = model,
                Name = StringUtils.CleanName(name),
                Units = Units.CreateInstanceFromEquation(eqn),
                NonNegative = new NonNegative(false)
            };
            variable.FullName = StringUtils.FullName(variable.Model, variable.Name);
            var factory = await EquationFactory.CreateInstance(model, eqn);
            variable.Equation = factory.Equation;
            variable.Value = factory.Value;
            variable.Initialize();
            variable.SetChildren();
            return variable;
        }

        public static async Task<T> CreateVariable<T>(string name, string model, string eqn, GraphicalFunction graph,
            Range range, Range scale,
            NonNegative nonNegative, VariableAccess access) where T : IVariable, new()
        {
            var variable = new T
            {
                Model = model,
                Name = StringUtils.CleanName(name),
                GraphicalFunction = graph,
                Range = range,
                Scale = scale,
                Units = Units.CreateInstanceFromEquation(eqn),
                NonNegative = nonNegative,
                Access = access
            };
            variable.FullName = StringUtils.FullName(variable.Model, variable.Name);

            var factory = await EquationFactory.CreateInstance(model, eqn);
            variable.Equation = factory.Equation;
            variable.Value = factory.Value;
            variable.AdjustValue(factory.Value);
            variable.Initialize();
            variable.SetChildren();
            return variable;
        }

        public static async Task<T> CreateInstance<T>(string name, XMileModel model, string eqn)
            where T : IVariable, new()
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var variable = await CreateVariable<T>(name, model.Name, eqn);
            model.Variables.Add(variable);
            return variable;
        }

        protected async Task CopyTo(IVariable copy)
        {
            if (copy == null)
            {
                throw new ArgumentNullException(nameof(copy));
            }

            copy.GraphicalFunction = GraphicalFunction;
            copy.Range = Range;
            copy.Scale = Scale;
            copy.Units = Units;
            if (Equation != null)
            {
                copy.Equation = await Equation.Clone(Model);
            }

            copy.Value = Value;
            copy.NonNegative = NonNegative;
            copy.Access = Access;
            copy.StoreResult = StoreResult;
            copy.Children = new List<string>();
            copy.Children.AddRange(Children);
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is Variable variable
                   && variable.FullName == FullName;
        }

        #region Xml attributes

        public string Name { get; set; }

        public Units Units { get; set; }

        /// <summary>
        ///     Input range
        /// </summary>
        public Range Range { get; set; } = new Range();

        /// <summary>
        ///     Output scale
        /// </summary>
        public Range Scale { get; set; } = new Range();

        public NonNegative NonNegative { get; set; }

        public VariableAccess Access { get; set; }

        /// <summary>
        ///     If true store the result in the ResultCollection
        /// </summary>
        public bool StoreResult { get; set; } = true;

        #endregion
    }
}