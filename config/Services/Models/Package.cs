// Copyright (c) Microsoft. All rights reserved.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Azure.IoTSolutions.UIConfig.Services.Models
{
    public class Package
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PackageType Type { get; set; }

        public string Content { get; set; }

        public string DateCreated { get; set; }
    }

    public enum PackageType
    {
        EdgeManifest
    }
}