using Microsoft.VisualStudio.TestTools.UnitTesting;
using RefRestrict;

namespace RefRestrictTest
{
    public partial class ProjectFileParserTest
    {
        [TestClass]
        public class ReportGeneratorTests
        {
            [TestMethod]
            public void CreateEmptyReportWhenRulesAdheredToForPackageReference()
            {
                // Arrange
                var configFile = @"TestConfigFiles\TestConfig_IncludePackageReference.xml";
                var project = @"TestConfigFiles\RR.WebServer.csproj";
                var projectInfo = ProjectFileParser.GetProjectInfo(project);
                var ruleSet = ConfigParser.GetRuleSetForProject(configFile, projectInfo.Name);

                //Act
                var results = RefAnaylser.GenerateReport(ruleSet, projectInfo);

                //Assert
                Assert.IsTrue(results.Entries.Count == 0);

            }

            [TestMethod]
            public void CreateNonEmptyReportWhenRulesAdheredToForPackageReference()
            {
                // Arrange
                var configFile = @"TestConfigFiles\TestConfig_ExcludePackageReference.xml";
                var project = @"TestConfigFiles\RR.WebServer.csproj";
                var projectInfo = ProjectFileParser.GetProjectInfo(project);
                var ruleSet = ConfigParser.GetRuleSetForProject(configFile, projectInfo.Name);

                //Act
                var results = RefAnaylser.GenerateReport(ruleSet, projectInfo);

                //Assert
                Assert.IsTrue(results.Entries.Count != 0);

            }
        }
    }
}
