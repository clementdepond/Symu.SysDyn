﻿#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Engine;

#endregion

namespace Symu.SysDyn.Tests.Simulation
{
    [TestClass]
    public class SimSpecsTests
    {
        private SimSpecs _sim = new SimSpecs(0, 10);
        private bool _triggered;

        [TestMethod]
        public void SimSpecsTest()
        {
            _sim = new SimSpecs(10, 100);
            Assert.AreEqual(10, _sim.Start);
            Assert.AreEqual<uint>(0, _sim.Step);
            Assert.AreEqual(100, _sim.Stop);
        }

        [TestMethod]
        public void ClearTest()
        {
            _sim = new SimSpecs(10, 100, 0.5F);
            _sim.Clear();
            Assert.AreEqual<uint>(20, _sim.Step);
        }

        [TestMethod]
        public void PauseTest()
        {
            Assert.IsFalse(_sim.Pause);
            _sim.PauseInterval = 0;
            Assert.IsFalse(_sim.Pause);
            _sim.PauseInterval = 10;
            Assert.IsTrue(_sim.Pause);
        }


        /// <summary>
        ///     DT = 1, no pause
        /// </summary>
        [TestMethod]
        public void RunTest()
        {
            while (_sim.Run())
            {
            }

            Assert.AreEqual(10, _sim.Time);
        }

        /// <summary>
        ///     DT = 1, pause 5
        /// </summary>
        [TestMethod]
        public void RunTest1()
        {
            _sim.Pause = true;
            _sim.PauseInterval = 5;
            while (_sim.Run())
            {
            }

            Assert.AreEqual(5, _sim.Time);
            Assert.AreEqual<uint>(5, _sim.Step);
            while (_sim.Run())
            {
            }

            Assert.AreEqual(10, _sim.Time);
            Assert.AreEqual<uint>(10, _sim.Step);
        }

        /// <summary>
        ///     DT > 1
        /// </summary>
        [TestMethod]
        public void RunTest2()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _sim.DeltaTime = 2);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => _sim.DeltaTime = 0);
        }

        /// <summary>
        ///     DT inferior 1, no pause
        /// </summary>
        [TestMethod]
        public void RunTest3()
        {
            _sim.DeltaTime = 0.25F;
            while (_sim.Run())
            {
            }

            Assert.AreEqual(10, _sim.Time);
            Assert.AreEqual<uint>(40, _sim.Step);
        }

        /// <summary>
        ///     DT inferior 1, pause 5
        /// </summary>
        [TestMethod]
        public void RunTest4()
        {
            _sim.Pause = true;
            _sim.PauseInterval = 5;
            _sim.DeltaTime = 0.25F;
            while (_sim.Run())
            {
            }

            Assert.AreEqual(5, _sim.Time);
            Assert.AreEqual<uint>(20, _sim.Step);
            while (_sim.Run())
            {
            }

            Assert.AreEqual(10, _sim.Time);
            Assert.AreEqual<uint>(40, _sim.Step);
        }

        /// <summary>
        ///     Start = stop
        /// </summary>
        [TestMethod]
        public void RunTest5()
        {
            _sim = new SimSpecs(0, 0);
            while (_sim.Run())
            {
            }

            Assert.AreEqual(0, _sim.Time);
            Assert.AreEqual<uint>(0, _sim.Step);
        }

        /// <summary>
        ///     Start = stop , DT inferior 1
        /// </summary>
        [TestMethod]
        public void RunTest6()
        {
            _sim = new SimSpecs(0, 0) {DeltaTime = 0.25F};
            while (_sim.Run())
            {
            }

            Assert.AreEqual(0, _sim.Time);
            Assert.AreEqual<uint>(0, _sim.Step);
        }

        /// <summary>
        ///     DT == 1
        /// </summary>
        [TestMethod]
        public void TimeManagementTest()
        {
            _sim.OnTimer += OnTimer;
            _sim.Step = 5;
            _sim.OnTimerEvent();
            Assert.IsTrue(_triggered);
        }

        /// <summary>
        ///     DT inferior 1
        /// </summary>
        [TestMethod]
        public void TimeManagementTest1()
        {
            _sim.OnTimer += OnTimer;
            _sim.DeltaTime = 0.5F;
            _sim.Step = 5;
            _sim.OnTimerEvent();
            Assert.IsFalse(_triggered);
            _sim.Step = 6;
            _sim.OnTimerEvent();
            Assert.IsTrue(_triggered);
        }

        /// <summary>
        ///     Timer has a new value, we store the results
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTimer(object sender, EventArgs e)
        {
            _triggered = true;
        }
    }
}