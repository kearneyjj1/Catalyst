#region LICENSE

/**
* Copyright (c) 2019 Catalyst Network
*
* This file is part of Catalyst.Node <https://github.com/catalyst-network/Catalyst.Node>
*
* Catalyst.Node is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 2 of the License, or
* (at your option) any later version.
*
* Catalyst.Node is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with Catalyst.Node. If not, see <https://www.gnu.org/licenses/>.
*/

#endregion

using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Catalyst.Common.Config;
using Catalyst.Common.UnitTests.TestUtils;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SharpRepository.Repository;
using Xunit;
using Xunit.Abstractions;

namespace Catalyst.Node.Core.UnitTests.Config
{
    public class StoredItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public interface IComponentWithRepository
    {
        IRepository<StoredItem> StringRepository { get; }
    }

    public class ComponentWithRepository : IComponentWithRepository
    {
        public IRepository<StoredItem> StringRepository { get; }

        public ComponentWithRepository(IRepository<StoredItem> stringRepository)
        {
            StringRepository = stringRepository;
        }
    }

    public sealed class ConfigurableRepoIntegrationTests : ConfigFileBasedTest
    {
        public ConfigurableRepoIntegrationTests(ITestOutputHelper output) : base(output) { }

        private async Task ModuleCanSaveAndRetrieveValuesFromRepository(FileInfo moduleFile)
        {
            var alteredComponentsFile = await CreateAlteredConfigForModule(moduleFile);

            var config = new ConfigurationBuilder()
               .AddJsonFile(alteredComponentsFile)
               .AddJsonFile(Path.Combine(Constants.ConfigSubFolder, Constants.NetworkConfigFile(Network.Dev)))
               .Build();

            ConfigureContainerBuilder(config);

            var container = ContainerBuilder.Build();
            using (container.BeginLifetimeScope())
            {
                var component = container.Resolve<IComponentWithRepository>();

                var guid = Guid.NewGuid().ToString();
                var storedItem = new StoredItem {Name = guid, Value = 10};
                
                component.StringRepository.Add(storedItem);

                component.StringRepository.TryFind(s => s.Name == guid, out var retrieveItem);

                retrieveItem.Name.Should().Be(storedItem.Name);
                retrieveItem.Value.Should().Be(storedItem.Value);
            }
        }

        private async Task<string> CreateAlteredConfigForModule(FileInfo mempoolConfigFile)
        {
            var originalContent = await File.ReadAllTextAsync(mempoolConfigFile.FullName);
            var newContent =
                originalContent.Replace("\"Config/Modules/moduleWithRepository.json\"",
                    JsonConvert.ToString(mempoolConfigFile.FullName));
            var newJsonPath = Path.Combine(FileSystem.GetCatalystHomeDir().FullName,
                $"components.{mempoolConfigFile.Name}");
            File.WriteAllText(newJsonPath, newContent);
            return newJsonPath;
        }

        private async Task<string> PointXmlConfigToLocalTestFolder(string mempoolConfigFile)
        {
            var originalContent = await
                File.ReadAllTextAsync(Path.Combine(Constants.ConfigSubFolder, Constants.ModulesSubFolder,
                    mempoolConfigFile));
            var newContent =
                originalContent.Replace("[@replace-this@]", FileSystem.GetCatalystHomeDir().Name);
            var jsonTestingFile = Path.Combine(FileSystem.GetCatalystHomeDir().FullName, mempoolConfigFile);
            File.WriteAllText(jsonTestingFile, newContent);
            return jsonTestingFile;
        }

        [Fact]
        [Trait(Traits.TestType, Traits.IntegrationTest)]
        public async Task Module_with_InMemoryRepo_can_save_and_retrieve()
        {
            var fi = new FileInfo(Path.Combine(Constants.ConfigSubFolder, Constants.ModulesSubFolder,
                "moduleWithRepository.inmemory.json"));
            await ModuleCanSaveAndRetrieveValuesFromRepository(fi);
        }

        [Fact]
        [Trait(Traits.TestType, Traits.IntegrationTest)]
        public async Task Module_with_XmlRepo_can_save_and_retrieve()
        {
            var mempoolConfigFile = "moduleWithRepository.xml.json";
            var resultFile = await PointXmlConfigToLocalTestFolder(mempoolConfigFile);
            await ModuleCanSaveAndRetrieveValuesFromRepository(new FileInfo(resultFile));
        }
    }
}