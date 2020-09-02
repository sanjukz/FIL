using Autofac.Extras.Moq;
using Dapper.FastCrud.Configuration.StatementOptions.Builders;
using FIL.Configuration.Api.Controllers;
using FIL.Configuration.Api.Repositories;
using FIL.Configuration.Api.Utilities;
using FIL.Configuration.Contracts.Models;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FIL.Configuration.Api.Tests.UnitTests
{
    [TestFixture]
    public class ConfigurationControllerTests
    {
        private AutoMock GetAutoMock()
        {
            return AutoMock.GetLoose();
        }

        [Test]
        public void GetConfigByKeyReturnsEmptyList()
        {
            using (var mock = GetAutoMock())
            {
                mock.Mock<IConfigurationKeyRepository>().Setup(r => r.FindByName(It.IsAny<string>()))
                    .Returns((ConfigurationKey)null);

                var controller = mock.Create<ConfigurationController>();
                var result = controller.GetConfigurationsByKey("i.dont.exist");
                result.ShouldBeEmpty();
            }
        }

        [Test]
        public void GetConfigByKeyReturnsConfigurations()
        {
            using (var mock = GetAutoMock())
            {
                mock.Mock<IConfigurationKeyRepository>().Setup(r => r.FindByName(It.IsAny<string>()))
                    .Returns(new ConfigurationKey
                    {
                        Name = "i.exist"
                    });
                mock.Mock<IConfigurationRepository>()
                    .Setup(r => r.GetConfigurationSettingsByKey(It.Is<ConfigurationKey>(k => k.Name.Equals("i.exist"))))
                    .Returns(new List<Contracts.Models.Configuration>
                    {
                        new Contracts.Models.Configuration()
                    });

                var controller = mock.Create<ConfigurationController>();
                var result = controller.GetConfigurationsByKey("i.exist");
                result.ShouldHaveSingleItem();
            }
        }

        [Test]
        public void GetAllReturnsDefaultIfEmpty()
        {
            using (var mock = GetAutoMock())
            {
                MockConfigurationSets(mock, new List<ConfigurationSet>
                {
                    new ConfigurationSet
                    {
                        Id = 0,
                        Name = "DEFAULT",
                        IsEnabled = true
                    }
                });

                var configs = new List<Contracts.Models.Configuration>
                {
                    new Contracts.Models.Configuration
                    {
                        ConfigurationSetId = 0,
                        ConfigurationKeyId = 1,
                        IsEnabled = true
                    }
                };

                MockConfigurationRepository(mock, 0, configs);
                MockConfigurationKeyRepository(mock, configs);

                mock.Provide(GetConfigRoot());
                var controller = mock.Create<ConfigurationController>();
                var result = controller.GetAll(string.Empty);
                result.ShouldHaveSingleItem();
            }
        }

        [Test]
        public void GetAllIgnoresDisabled()
        {
            using (var mock = GetAutoMock())
            {
                MockConfigurationSets(mock, new List<ConfigurationSet>
                {
                    new ConfigurationSet
                    {
                        Id = 0,
                        Name = "DEFAULT",
                        IsEnabled = true
                    }
                });

                var configs = new List<Contracts.Models.Configuration>
                {
                    new Contracts.Models.Configuration
                    {
                        ConfigurationSetId = 0,
                        ConfigurationKeyId = 1
                    }
                };

                MockConfigurationRepository(mock, 0, configs);
                MockConfigurationKeyRepository(mock, configs);

                mock.Provide(GetConfigRoot());
                var controller = mock.Create<ConfigurationController>();
                var result = controller.GetAll(string.Empty);
                result.ShouldBeEmpty();
            }
        }

        [Test]
        public void GetAllMergesWithParentAndDefault()
        {
            using (var mock = GetAutoMock())
            {
                MockConfigurationSets(mock, new List<ConfigurationSet>
                {
                    new ConfigurationSet
                    {
                        Id = 0,
                        Name = "DEFAULT",
                        IsEnabled = true
                    },
                    new ConfigurationSet
                    {
                        Id = 1,
                        Name = "PARENT",
                        IsEnabled = true
                    },
                    new ConfigurationSet
                    {
                        Id = 2,
                        Name = "CHILD",
                        IsEnabled = true,
                        ParentConfigurationSetId = 1
                    }
                });

                var defaultConfigs = new List<Contracts.Models.Configuration>
                {
                    new Contracts.Models.Configuration
                    {
                        ConfigurationSetId = 0,
                        ConfigurationKeyId = 1,
                        Value = "BASE",
                        IsEnabled = true
                    }
                };

                MockConfigurationRepository(mock, 0, defaultConfigs);

                var parentConfigs = new List<Contracts.Models.Configuration>
                {
                    new Contracts.Models.Configuration
                    {
                        ConfigurationSetId = 1,
                        ConfigurationKeyId = 2,
                        Value = "PARENT",
                        IsEnabled = true
                    },
                    new Contracts.Models.Configuration
                    {
                        ConfigurationSetId = 1,
                        ConfigurationKeyId = 3,
                        Value = "PARENT_NEW",
                        IsEnabled = true
                    }
                };

                MockConfigurationRepository(mock, 1, parentConfigs);

                var childConfigs = new List<Contracts.Models.Configuration>
                {
                    new Contracts.Models.Configuration
                    {
                        ConfigurationSetId = 2,
                        ConfigurationKeyId = 2,
                        Value = "PARENT_OVERRIDEN",
                        IsEnabled = true
                    },
                    new Contracts.Models.Configuration
                    {
                        ConfigurationSetId = 2,
                        ConfigurationKeyId = 4,
                        Value = "CHILD_ONLY",
                        IsEnabled = true
                    }
                };

                MockConfigurationRepository(mock, 2, childConfigs);
                MockConfigurationKeyRepository(mock, childConfigs.Union(parentConfigs).Union(defaultConfigs).Distinct(new ConfigurationEqualityComparerByKey()).ToList());

                var controller = mock.Create<ConfigurationController>();
                var result = controller.GetAll("CHILD");

                result.Count().ShouldBe(4);
                var config = result.Values.ToDictionary(k => k.ConfigurationKeyId);
                config[1].Value.ShouldBe("BASE");
                config[2].Value.ShouldBe("PARENT_OVERRIDEN");
                config[3].Value.ShouldBe("PARENT_NEW");
                config[4].Value.ShouldBe("CHILD_ONLY");
            }
        }

        [Test]
        public void GetAllMergesWithParentOverDefault()
        {
            using (var mock = GetAutoMock())
            {
                MockConfigurationSets(mock, new List<ConfigurationSet>
                {
                    new ConfigurationSet
                    {
                        Id = 0,
                        Name = "DEFAULT",
                        IsEnabled = true
                    },
                    new ConfigurationSet
                    {
                        Id = 1,
                        Name = "PARENT",
                        IsEnabled = true
                    },
                    new ConfigurationSet
                    {
                        Id = 2,
                        Name = "CHILD",
                        IsEnabled = true,
                        ParentConfigurationSetId = 1
                    }
                });

                var defaultConfigs = new List<Contracts.Models.Configuration>
                {
                    new Contracts.Models.Configuration
                    {
                        ConfigurationSetId = 0,
                        ConfigurationKeyId = 1,
                        Value = "BASE",
                        IsEnabled = true
                    }
                };

                MockConfigurationRepository(mock, 0, defaultConfigs);

                var parentConfigs = new List<Contracts.Models.Configuration>
                {
                    new Contracts.Models.Configuration
                    {
                        ConfigurationSetId = 1,
                        ConfigurationKeyId = 1,
                        Value = "PARENT",
                        IsEnabled = true
                    }
                };

                MockConfigurationRepository(mock, 1, parentConfigs);
                MockConfigurationKeyRepository(mock, parentConfigs.Union(defaultConfigs).Distinct(new ConfigurationEqualityComparerByKey()).ToList());

                var controller = mock.Create<ConfigurationController>();
                var result = controller.GetAll("CHILD");

                result.ShouldHaveSingleItem();
                result.Values.First().Value.ShouldBe("PARENT");
            }
        }

        [Test]
        public void GetAllReturnsEmptyConfigSetNotFound()
        {
            using (var mock = GetAutoMock())
            {
                mock.Provide(GetConfigRoot());
                var controller = mock.Create<ConfigurationController>();
                var result = controller.GetAll(string.Empty);

                result.ShouldBeEmpty();
            }
        }

        [Test]
        public void GetAllThrowsExceptionWhenDefaultRequestedAndDisabled()
        {
            using (var mock = GetAutoMock())
            {
                mock.Mock<IConfigurationSetRepository>().Setup(r => r.GetAll(It
                        .IsAny<Func<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<ConfigurationSet>,
                            IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<ConfigurationSet>>>()))
                    .Returns(new List<ConfigurationSet>
                    {
                        new ConfigurationSet
                        {
                            Name = "DEFAULT",
                            IsEnabled = false
                        }
                    });

                var controller = mock.Create<ConfigurationController>();

                Should.Throw<Exception>(() => controller.GetAll("DEFAULT"));
            }
        }

        private void MockConfigurationRepository(AutoMock mock, int configurationSetId, IList<Contracts.Models.Configuration> configurations)
        {
            mock.Mock<IConfigurationRepository>().Setup(r => r.GetAll(It.Is<int>(x => x == configurationSetId))).Returns(configurations);
        }

        private void MockConfigurationKeyRepository(AutoMock mock, IList<Contracts.Models.Configuration> configurations)
        {
            mock.Mock<IConfigurationKeyRepository>().Setup(r => r.GetKeys(It.IsAny<IEnumerable<int>>()))
                .Returns(configurations.Select(c => new ConfigurationKey
                {
                    Id = c.ConfigurationKeyId,
                    Name = c.ConfigurationKeyId.ToString()
                }));
        }

        private void MockConfigurationSets(AutoMock mock, IEnumerable<ConfigurationSet> configSets)
        {
            mock.Mock<IConfigurationSetRepository>().Setup(r => r.GetAll(It
                    .IsAny<Func<IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<ConfigurationSet>,
                        IRangedBatchSelectSqlSqlStatementOptionsOptionsBuilder<ConfigurationSet>>>()))
                .Returns(configSets);
        }

        private IConfiguration GetConfigRoot()
        {
            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>(Constants.DefaultConfigSetName, "DEFAULT")
                });
            return builder.Build();
        }

        private class ConfigurationEqualityComparerByKey : IEqualityComparer<Contracts.Models.Configuration>
        {
            public bool Equals(Contracts.Models.Configuration x, Contracts.Models.Configuration y)
            {
                return x.ConfigurationKeyId.Equals(y.ConfigurationKeyId);
            }

            public int GetHashCode(Contracts.Models.Configuration obj)
            {
                return obj.ConfigurationKeyId.GetHashCode();
            }
        }
    }
}