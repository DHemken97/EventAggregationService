using System;
using System.Collections.Generic;
using System.Linq;
using EAS_Development_Interfaces;
using EAS_Development_Interfaces.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CorePlugin.UnitTests
{
    [TestClass]
    public class CommandElementsTests
    {
        
        [DataTestMethod]
        [DataRow("Test Command","Test",new[] { "Command"},new char[0])]
        [DataRow("Test Command With -f -l -a -g -s --Enabled:True","Test",new[] { "Command","With"},new[] { 'f', 'l', 'a', 'g', 's' })]

        public void CommandElementsShouldParseCommandProperly(string command, string op, string[] args, char[] flags)
        {
            var sut = new CommandElements(command);
            Assert.AreEqual(sut.command, op);
            Assert.IsTrue(sut.Arguments.SequenceEqual(args));
            Assert.AreEqual(sut.Flags.OrderBy(flag => flag).ToJson(), flags.OrderBy(flag => flag).ToJson());

        }
        [TestMethod]
        public void CommandElementsShouldParseDoubleFlagsProperly()
        {
            var sut = new CommandElements("Test Command With -f -l -a -g -s --Enabled:True --Name:Test");

            var expected = new Dictionary<string, string>
            {
                { "Enabled","True"},
                { "Name","Test"}

            }.ToJson();
            var result = sut.DoubleFlags.ToJson();
            Assert.AreEqual(expected, result);

        }
    }
}
