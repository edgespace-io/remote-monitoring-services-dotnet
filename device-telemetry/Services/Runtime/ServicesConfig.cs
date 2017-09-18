﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Azure.IoTSolutions.DeviceTelemetry.Services.Exceptions;

namespace Microsoft.Azure.IoTSolutions.DeviceTelemetry.Services.Runtime
{
    public interface IServicesConfig
    {
        StorageConfig MessagesConfig { get; set; }
        StorageConfig AlarmsConfig { get; set; }
        string RulesTemplatesFolder { get; set; }
        string DocumentDbConnString { get; set; }
        Uri DocumentDbUri { get; }
        string DocumentDbKey { get; }
        int DocumentDbThroughput { get; set; }
        string StorageAdapterApiUrl { get; set; }
        int StorageAdapterApiTimeout { get; set; }
    }

    public class ServicesConfig : IServicesConfig
    {
        private string rtf = string.Empty;

        public string RulesTemplatesFolder
        {
            get { return this.rtf; }
            set { this.rtf = this.NormalizePath(value); }
        }

        public string DocumentDbConnString { get; set; }

        public string StorageAdapterApiUrl { get; set; }

        public int StorageAdapterApiTimeout { get; set; }

        public StorageConfig MessagesConfig { get; set; }

        public StorageConfig AlarmsConfig { get; set; }

        public int DocumentDbThroughput { get; set; }

        public Uri DocumentDbUri => this.GetDocumentDbUri();

        public string DocumentDbKey => this.GetDocumentDbKey();

        private string NormalizePath(string path)
        {
            return path
                       .TrimEnd(Path.DirectorySeparatorChar)
                       .Replace(
                           Path.DirectorySeparatorChar + "." + Path.DirectorySeparatorChar,
                           Path.DirectorySeparatorChar.ToString()) + Path.DirectorySeparatorChar;
        }

        private Uri GetDocumentDbUri()
        {
            var match = Regex.Match(this.DocumentDbConnString,
                @"^AccountEndpoint=(?<endpoint>.*);AccountKey=(?<key>.*);$");

            Uri endpoint;

            if (!match.Success ||
                !Uri.TryCreate(match.Groups["endpoint"].Value,
                    UriKind.RelativeOrAbsolute,
                    out endpoint))
            {
                var message = "Invalid connection string for DocumentDB";
                throw new InvalidConfigurationException(message);
            }

            return endpoint;
        }

        private string GetDocumentDbKey()
        {
            var match = Regex.Match(this.DocumentDbConnString,
                @"^AccountEndpoint=(?<endpoint>.*);AccountKey=(?<key>.*);$");

            if (!match.Success)
            {
                var message = "Invalid connection string for DocumentDB";
                throw new InvalidConfigurationException(message);
            }

            return match.Groups["key"].Value;
        }
    }
}