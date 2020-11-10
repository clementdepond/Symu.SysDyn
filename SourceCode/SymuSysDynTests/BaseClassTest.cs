﻿#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Models;
using Symu.SysDyn.Models.XMile;
using Symu.SysDyn.Parser;

#endregion

namespace SymuSysDynTests
{
    [TestClass]
    public abstract class BaseClassTest
    {
        protected const string TestFile =
            @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDynTests\Templates\Test.xmile";

        protected BaseClassTest()
        {
            Machine = new StateMachine(TestFile);
            XDoc = XDocument.Load(TestFile);
            Ns = XDoc.Root?.Attributes("xmlns").First().Value;
            XElement = XDoc.Root?.Descendants(Ns + "variables").First();
        }

        protected XmlParser Parser { get; } = new XmlParser(TestFile);

        /// <summary>
        ///     It is not the Machine.Models
        /// </summary>
        protected XMileModel Model { get; } = new XMileModel("1");

        /// <summary>
        ///     It is not the Machine.Models
        /// </summary>
        protected XMileModel RootModel => Machine.Models.RootModel;

        protected XDocument XDoc { get; }
        protected XNamespace Ns { get; }
        protected XElement XElement { get; set; }
        protected StateMachine Machine { get; }
    }
}