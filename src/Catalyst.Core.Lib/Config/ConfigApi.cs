﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Catalyst.Abstractions.Dfs.CoreApi;
using Catalyst.Abstractions.FileSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Catalyst.Core.Lib.Config
{
    public sealed class ConfigApi : IConfigApi
    {
        private static readonly JObject DefaultConfiguration = JObject.Parse(@"{
  ""Addresses"": {
    ""API"": ""/ip4/127.0.0.1/tcp/5001"",
    ""Gateway"": ""/ip4/127.0.0.1/tcp/8080"",
    ""Swarm"": [
      ""/ip4/0.0.0.0/tcp/4001"",
      ""/ip6/::/tcp/4001""
    ]
  },
}");

        private JObject _configuration;
        private readonly IFileSystem _fileSystem;

        public ConfigApi(IFileSystem fileSystem) { _fileSystem = fileSystem; }

        public async Task<JObject> GetAsync(CancellationToken cancel = default(CancellationToken))
        {
            // If first time, load the configuration into memory.
            if (_configuration != null)
            {
                return _configuration;
            }
            
            var path = Path.Combine(_fileSystem.GetCatalystDataDir().ToString());
            if (File.Exists(path))
            {
                using (var reader = File.OpenText(path))
                using (var jtr = new JsonTextReader(reader))
                {
                    _configuration = await JObject.LoadAsync(jtr, cancel).ConfigureAwait(false);
                }
            }
            else
            {
                await ReplaceAsync(DefaultConfiguration).ConfigureAwait(false);
            }

            return _configuration;
        }

        public async Task<JToken> GetAsync(string key, CancellationToken cancel = default(CancellationToken))
        {
            JToken config = await GetAsync(cancel).ConfigureAwait(false);
            var keys = key.Split('.');
            foreach (var name in keys)
            {
                config = config[name];
                if (config == null)
                {
                    throw new KeyNotFoundException($"Configuration setting '{key}' does not exist.");
                }
            }

            return config;
        }

        public Task ReplaceAsync(JObject config)
        {
            _configuration = config ?? throw new ArgumentNullException(nameof(config));
            return SaveAsync();
        }

        public Task SetAsync(string key, string value, CancellationToken cancel = default)
        {
            return SetAsync(key, JToken.FromObject(value), cancel);
        }

        public async Task SetAsync(string key, JToken value, CancellationToken cancel = default)
        {
            var config = await GetAsync(cancel).ConfigureAwait(false);

            // If needed, create the setting owner keys.
            var keys = key.Split('.');
            foreach (var name in keys.Take(keys.Length - 1))
            {
                if (!(config[name] is JObject token))
                {
                    token = new JObject();
                    config[name] = token;
                }

                config = token;
            }

            config[keys.Last()] = value;
            await SaveAsync().ConfigureAwait(false);
        }

        async Task SaveAsync()
        {
            var path = Path.Combine(Constants.CatalystDataDir, "config");
            using (var fs = File.OpenWrite(path))
            using (var writer = new StreamWriter(fs))
            using (var jtw = new JsonTextWriter(writer)
            {
                Formatting = Formatting.Indented
            })
            {
                await _configuration.WriteToAsync(jtw).ConfigureAwait(false);
            }
        }
    }
}