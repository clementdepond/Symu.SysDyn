﻿#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Models;

#endregion

namespace SymuSysDynTests.Models
{
    [TestClass]
    public class UnitsTests
    {
        /// <summary>
        ///     Non passing tests
        /// </summary>
        [TestMethod]
        public void CreateInstanceFromEquationTest()
        {
            Assert.IsNull(Units.CreateInstanceFromEquation(null));
            Assert.IsNull(Units.CreateInstanceFromEquation(string.Empty));
            Assert.IsNull(Units.CreateInstanceFromEquation("test"));
        }

        /// <summary>
        ///     Passing test
        /// </summary>
        [TestMethod]
        public void CreateInstanceFromEquationTest1()
        {
            var test = Units.CreateInstanceFromEquation("test {unit1/unit2}");
            Assert.IsNotNull(test);
            Assert.AreEqual("unit1/unit2", test.Eqn);
        }
    }
}