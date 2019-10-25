using LocalAppVeyor.Engine.Configuration;
using LocalAppVeyor.Engine.UnitTests.TestUtilities;
using Moq;
using Xunit;

namespace LocalAppVeyor.Engine.UnitTests.Engine
{
    public partial class EngineTests
    {
        [WindowsOnlyFact]
        public void Windows_ShouldRunInitializationPowershellScript()
        {
            var buildConfiguration = new BuildConfiguration
            {
                InitializationScript =
                {
                    new ScriptLine(ScriptType.PowerShell, "Write-Host 'This is a test'")
                }
            };

            var jobResult = new LocalAppVeyor.Engine.Engine(_engineConfiguration, buildConfiguration).ExecuteJob(0);

            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is a test")), Times.Once);
            Assert.True(jobResult.IsSuccessfulExecution);
        }

        [WindowsOnlyFact]
        public void Windows_ShouldRunInitializationPowershellScriptWithMultipleLines()
        {
            var buildConfiguration = new BuildConfiguration
            {
                InitializationScript =
                {
                    new ScriptLine(ScriptType.PowerShell, "Write-Host 'This is a test - first line'"),
                    new ScriptLine(ScriptType.PowerShell, "Write-Host 'This is a test - second line'")
                }
            };

            var jobResult = new LocalAppVeyor.Engine.Engine(_engineConfiguration, buildConfiguration).ExecuteJob(0);

            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is a test - first line")), Times.Once);
            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is a test - second line")), Times.Once);
            _outputterMock.Verify(outputter => outputter.WriteError(It.IsAny<string>()), Times.Never);
            Assert.True(jobResult.IsSuccessfulExecution);
        }

        [WindowsOnlyFact]
        public void Windows_ShouldRunInitializationBatchScript()
        {
            var buildConfiguration = new BuildConfiguration
            {
                InitializationScript =
                {
                    new ScriptLine(ScriptType.Batch, "echo This is a test")
                }
            };

            var jobResult = new LocalAppVeyor.Engine.Engine(_engineConfiguration, buildConfiguration).ExecuteJob(0);

            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is a test")), Times.Once);
            _outputterMock.Verify(outputter => outputter.WriteError(It.IsAny<string>()), Times.Never);
            Assert.True(jobResult.IsSuccessfulExecution);
        }

        [WindowsOnlyFact]
        public void Windows_ShouldRunInitializationBatchScriptWithMultipleLines()
        {
            var buildConfiguration = new BuildConfiguration
            {
                InitializationScript =
                {
                    new ScriptLine(ScriptType.Batch, "echo This is a test - first line"),
                    new ScriptLine(ScriptType.Batch, "echo This is a test - second line")
                }
            };

            var jobResult = new LocalAppVeyor.Engine.Engine(_engineConfiguration, buildConfiguration).ExecuteJob(0);

            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is a test - first line")), Times.Once);
            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is a test - second line")), Times.Once);
            _outputterMock.Verify(outputter => outputter.WriteError(It.IsAny<string>()), Times.Never);
            Assert.True(jobResult.IsSuccessfulExecution);
        }

        [WindowsOnlyFact]
        public void Windows_ShouldRunInitializationScriptWithMixedPowershellAndBatchScripts()
        {
            var buildConfiguration = new BuildConfiguration
            {
                InitializationScript =
                {
                    new ScriptLine(ScriptType.Batch, "echo This is 1 batch test"),
                    new ScriptLine(ScriptType.PowerShell, "Write-Host 'This is 1 powershell test'"),
                    new ScriptLine(ScriptType.Batch, "echo This is 2 batch test"),
                    new ScriptLine(ScriptType.PowerShell, "Write-Host 'This is 2 powershell test'"),
                    new ScriptLine(ScriptType.PowerShell, "Write-Host 'This is 3 powershell test'"),
                }
            };

            var jobResult = new LocalAppVeyor.Engine.Engine(_engineConfiguration, buildConfiguration).ExecuteJob(0);

            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is 1 batch test")), Times.Once);
            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is 1 powershell test")), Times.Once);
            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is 2 batch test")), Times.Once);
            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is 2 powershell test")), Times.Once);
            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is 3 powershell test")), Times.Once);
            _outputterMock.Verify(outputter => outputter.WriteError(It.IsAny<string>()), Times.Never);
            Assert.True(jobResult.IsSuccessfulExecution);
        }

        [WindowsOnlyFact]
        public void Windows_ShouldRunInitializationScriptPowershellBlockScript()
        {
            var buildConfiguration = new BuildConfiguration
            {
                InitializationScript =
                {
                    new ScriptLine(ScriptType.PowerShell, @"
Write-Host 'This is line one'
Write-Host 'And this is line two'")
                }
            };

            var jobResult = new LocalAppVeyor.Engine.Engine(_engineConfiguration, buildConfiguration).ExecuteJob(0);

            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "This is line one")), Times.Once);
            _outputterMock.Verify(outputter => outputter.Write(It.Is<string>(m => m == "And this is line two")), Times.Once);
            _outputterMock.Verify(outputter => outputter.WriteError(It.IsAny<string>()), Times.Never);
            Assert.True(jobResult.IsSuccessfulExecution);
        }
    }
}