using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefRestrict;

namespace RefRestrictTest
{
    [TestClass]
    public class ConfigParserTest
    {
        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void GetRuleSetForAConfigFileWhereDirectoryDoesntExist()
        {
            // Arrange
            var badConfigFile = @"C:\IDoNotExist\InvalidConfig.xml";
            var project = "RefRestrict";

            // Act
            ConfigParser.GetRuleSetForProject(badConfigFile, project);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void GetRuleSetForAConfigFileWhereFileDoesntExist()
        {
            // Arrange
            var badConfigFile = @"C:\InvalidConfig.xml";
            var project = "RefRestrict";

            // Act
            ConfigParser.GetRuleSetForProject(badConfigFile, project);
        }
    }
}
