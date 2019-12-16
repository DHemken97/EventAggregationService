using System;
using System.Collections.Generic;
using System.Linq;
using EAS_Development_Interfaces.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CorePlugin.UnitTests
{
    [TestClass]
    public class CommandElementsTests
    {
        
        [DataTestMethod]
        [DataRow("Test Command","Test",new[] { "Command"},new char[0],default(Dictionary<string,string>))]
        public void CommandElementsShouldParseCommandProperly(string command, string op, string[] args, char[] flags,Dictionary<string,string> doubleFlags)
        {
            var sut = new CommandElements(command);
            Assert.AreEqual(sut.command, op);
            Assert.IsTrue(sut.Arguments.SequenceEqual(args));
            Assert.IsTrue(sut.Flags.OrderBy(flag => flag).SequenceEqual(args.OrderBy(flag => flag)));
            Assert.IsTrue(sut.DoubleFlags.SequenceEqual(doubleFlags));

        }
    }
}
