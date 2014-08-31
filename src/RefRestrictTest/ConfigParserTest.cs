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
        public void ThrowExceptionWhenGettingRuleSetForAConfigFileWhereDirectoryDoesntExist()
        {
            // Arrange
            var badConfigFile = @"C:\IDoNotExist\InvalidConfig.xml";
            var project = "RefRestrict";

            // Act
            ConfigParser.GetRuleSetForProject(badConfigFile, project);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void ThrowExceptionWhenGettingRuleSetForAConfigFileWhereFileDoesntExist()
        {
            // Arrange
            var badConfigFile = @"C:\InvalidConfig.xml";
            var project = "RefRestrict";

            // Act
            ConfigParser.GetRuleSetForProject(badConfigFile, project);
        }

        [TestMethod]
        public void GetRuleSetWithNoRulesFromABlankConfigFile()
        {
            // Arrange
            var badConfigFile = @"TestConfigFiles\TestConfig_NoProjects.xml";
            var project = "RefRestrict";

            // Act
            var ruleset = ConfigParser.GetRuleSetForProject(badConfigFile, project);

            // Assert
            Assert.IsNotNull(ruleset);
            Assert.IsFalse(ruleset.IsRules);
        }

        [TestMethod]
        public void GetRuleSetWithNoRulesForAProjectWhichHasNoRules()
        {
            // Arrange
            var badConfigFile = @"TestConfigFiles\TestConfig_ProjectAlphaSingleRule.xml";
            var project = "ProjectBeta";

            // Act
            var ruleset = ConfigParser.GetRuleSetForProject(badConfigFile, project);

            // Assert
            Assert.IsNotNull(ruleset);
            Assert.IsFalse(ruleset.IsRules);
        }

        [TestMethod]
        public void GetRuleSetWithSingleRule()
        {
            // Arrange
            var badConfigFile = @"TestConfigFiles\TestConfig_ProjectAlphaSingleRule.xml";
            var project = "ProjectAlpha";

            // Act
            var ruleset = ConfigParser.GetRuleSetForProject(badConfigFile, project);

            // Assert
            Assert.IsNotNull(ruleset);
            Assert.IsTrue(ruleset.IsRules);
            Assert.AreEqual(1, ruleset.Rules.Count);
            
            var actRule = ruleset.Rules[0] as MultiRefRule;
            Assert.IsNotNull(actRule);
            Assert.AreEqual(RuleType.OnlyLocalReferences, actRule.Type);
            Assert.AreEqual(0, actRule.Refs.Count);
        }
    }
}
