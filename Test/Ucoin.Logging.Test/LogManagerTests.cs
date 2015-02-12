using Moq;
using Xunit;
using System;
using FluentAssertions;
using System.Diagnostics;
using Ucoin.Framework.Logging;
using Ucoin.Framework.Logging.Simple;
using Ucoin.Framework.Logging.Configuration;

namespace Ucoin.Logging.Test
{
    public class LogManagerTests
    {
        public LogManagerTests()
        {
            LogManager.Reset();         
        }

        [Fact]
        public void logging_adapter_property_test()
        {
            ILoggerAdapter adapter = new NoOpLoggerAdapter();
            LogManager.Adapter = adapter;

            LogManager.Adapter.Should().Be(adapter);
            Assert.Throws<ArgumentNullException>(delegate { LogManager.Adapter = null; });
        }

        [Fact]
        public void logging_manager_reset_test()
        {
            LogManager.Reset();
            LogManager.ConfigurationReader.Should().BeOfType<DefaultConfigurationReader>();
            Assert.Throws<ArgumentNullException>(delegate { LogManager.Reset(null); });

            var mockConfigReader = new Mock<IConfigurationReader>();
            mockConfigReader
                .Setup(b => b.GetSection(LogManager.COMMON_LOGGING_SECTION))
                .Returns(new TraceLoggerAdapter());
            LogManager.Reset(mockConfigReader.Object);
            LogManager.Adapter.Should().BeOfType<TraceLoggerAdapter>();
        }

        [Fact]
        public void logging_manager_configure_from_configuration_reader_test()
        {
            var mockConfigReader = new Mock<IConfigurationReader>();

            mockConfigReader
                .Setup(b => b.GetSection(LogManager.COMMON_LOGGING_SECTION))
                .Returns(null);
            LogManager.Reset(mockConfigReader.Object);
            var logger = LogManager.GetLogger<LogManagerTests>();
            logger.Should().BeOfType<NoOpLogger>();

            mockConfigReader
                .Setup(b => b.GetSection(LogManager.COMMON_LOGGING_SECTION))
                .Returns(new TraceLoggerAdapter());
            LogManager.Reset(mockConfigReader.Object);
            logger = LogManager.GetLogger(typeof(LogManagerTests));
            logger.Should().BeOfType<TraceLogger>();

            mockConfigReader
               .Setup(b => b.GetSection(LogManager.COMMON_LOGGING_SECTION))
               .Returns(new LogSetting(typeof(ConsoleOutLoggerAdapter), null));
            LogManager.Reset(mockConfigReader.Object);
            logger = LogManager.GetLogger(typeof(LogManagerTests));
            logger.Should().BeOfType<ConsoleOutLogger>();

            mockConfigReader
              .Setup(b => b.GetSection(LogManager.COMMON_LOGGING_SECTION))
              .Returns(new object());
            LogManager.Reset(mockConfigReader.Object);
            var exception = Assert.Throws<ConfigurationException>(delegate
            {
                logger = LogManager.GetLogger(typeof(LogManagerTests));
            });
            var exceptMsg = string.Format(
               "ConfigurationReader {0} returned unknown settings instance of type System.Object",
                mockConfigReader.Object.GetType().FullName);
            exception.Message.Should().Be(exceptMsg);
        }

        [Fact]
        public void ConfigureFromStandaloneConfig()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <loggerAdapter type='Ucoin.Framework.Logging.Simple.ConsoleOutLoggerAdapter, Ucoin.Framework.Logging'>
                  </loggerAdapter>
                </logging>";
            var logger = GetLog(xml);
            logger.Should().BeOfType<ConsoleOutLogger>();
        }


        [Fact]
        public void InvalidAdapterType()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <loggerAdapter type='Ucoin.Framework.Logging.Simple.NonExistentAdapter, Ucoin.Framework.Logging'>
                  </loggerAdapter>
                </logging>";
            Assert.Throws<ConfigurationException>(delegate { GetLog(xml); });
        }

        [Fact]
        public void AdapterDoesNotImplementInterface()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <loggerAdapter type='Ucoin.Logging.Test.StandaloneConfigurationReader, Ucoin.Logging.Test'>
                  </loggerAdapter>
                </logging>";
            Assert.Throws<ConfigurationException>(delegate { GetLog(xml); });
        }

        [Fact]
        public void AdapterDoesNotHaveCorrectCtors()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                  <loggerAdapter type='Ucoin.Logging.Test.MissingCtorLoggerAdapter, Ucoin.Logging.Test'>
                  </loggerAdapter>
                </logging>";
            Assert.Throws<ConfigurationException>(delegate { GetLog(xml); });
        }

        [Fact]
        public void AdapterDoesNotHaveCorrectCtorsWithArgs()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <logging>
                    <loggerAdapter type='Ucoin.Logging.Test.MissingCtorLoggerAdapter, Ucoin.Logging.Test'>
                        <arg key='level' value='DEBUG' />
                    </loggerAdapter>
                </logging>";
            Assert.Throws<ConfigurationException>(delegate { GetLog(xml); });
        }

        [Fact]
        public void InvalidXmlSection()
        {
            const string xml =
                @"<?xml version='1.0' encoding='UTF-8' ?>
                <foo>
                    <logging>
                      <loggerAdapter type='Ucoin.Logging.Test.MissingCtorLoggerAdapter, Ucoin.Logging.Test'>
                            <arg key='level' value='DEBUG' />
                      </loggerAdapter>
                    </logging>
                </foo>";
            var logger = GetLog(xml);
            var noOpLogger = logger as NoOpLogger;
            noOpLogger.Should().NotBeNull();
        }

        private static ILogger GetLog(string xml)
        {
            var configReader = new StandaloneConfigurationReader(xml);
            LogManager.Reset(configReader);
            return LogManager.GetLogger(typeof(LogManagerTests));
        }
    }
}