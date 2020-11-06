﻿#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Functions;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class IfThenElseTests
    {
        [TestMethod]
        public void ParseTest()
        {
            var function = "IF x1 THEN x2 ELSE x3";
            IfThenElse.Parse(string.Empty, ref function, out var parameters, out var args);
            CheckOkTest(function, parameters, args);
        }

        [TestMethod]
        public void ParseTest1()
        {
            var function = "If x1 Then x2 Else x3";
            IfThenElse.Parse(string.Empty, ref function, out var parameters, out var args);
            CheckOkTest(function, parameters, args);
        }

        [TestMethod]
        public void ParseTest2()
        {
            var function = "";
            IfThenElse.Parse(string.Empty, ref function, out var parameters, out var args);
            CheckKoTest(parameters, args);
        }

        [TestMethod]
        public void ParseTest3()
        {
            var function = "IF x1 THEN x2";
            IfThenElse.Parse(string.Empty, ref function, out var parameters, out var args);
            CheckKoTest(parameters, args);
        }

        [TestMethod]
        public void ParseTest4()
        {
            var function = "IF x1 ELSE x3";
            IfThenElse.Parse(string.Empty, ref function, out var parameters, out var args);
            CheckKoTest(parameters, args);
        }

        private static void CheckOkTest(string function, IReadOnlyList<IEquation> parameters, IReadOnlyList<float> args)
        {
            Assert.AreEqual(3, parameters.Count);
            Assert.AreEqual("_X1", parameters[0].Variables.First());
            Assert.AreEqual("_X2", parameters[1].Variables.First());
            Assert.AreEqual("_X3", parameters[2].Variables.First());
            Assert.AreEqual(3, args.Count);
            Assert.AreEqual(0, args[0]);
            Assert.AreEqual(0, args[1]);
            Assert.AreEqual(0, args[2]);
            Assert.AreEqual("if(_X1,_X2,_X3)", function);
        }

        private static void CheckKoTest(IReadOnlyCollection<IEquation> parameters, IReadOnlyCollection<float> args)
        {
            Assert.AreEqual(0, parameters.Count);
            Assert.AreEqual(0, args.Count);
        }


        [TestMethod]
        public void ParseTest5()
        {
            var function = "IF (1) THEN (2) ELSE (3)";
            IfThenElse.Parse(string.Empty, ref function, out _, out _);
            Assert.AreEqual("if(1,2,3)", function);
        }


        [TestMethod]
        public void ParseTest6()
        {
            var function = "IF(1)THEN(2)ELSE(3)";
            IfThenElse.Parse(string.Empty, ref function, out _, out _);
            Assert.AreEqual("if(1,2,3)", function);
        }

        [TestMethod]
        public void ParseTest7()
        {
            var function = "IF (1+1) THEN (2+2) ELSE (3+3)";
            IfThenElse.Parse(string.Empty, ref function, out _, out _);
            Assert.AreEqual("if(2,4,6)", function);
        }
    }
}