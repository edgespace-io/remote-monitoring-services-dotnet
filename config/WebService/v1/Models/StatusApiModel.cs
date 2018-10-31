﻿// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.Azure.IoTSolutions.UIConfig.WebService.v1.Models;
using Microsoft.Azure.IoTSolutions.UIConfig.WebService.Runtime;
using Newtonsoft.Json;

namespace Microsoft.Azure.IoTSolutions.UIConfig.WebService.v1.Models
{
    public sealed class StatusApiModel
    {
        private const string DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:sszzz";

        [JsonProperty(PropertyName = "Name", Order = 10)]
        public string Name => "Config";

        [JsonProperty(PropertyName = "IsHealthy", Order = 20)]
        public bool IsHealthy = true;

        [JsonProperty(PropertyName = "Message", Order = 25)]
        public string Message = "Alive and well!";

        [JsonProperty(PropertyName = "CurrentTime", Order = 30)]
        public string CurrentTime => DateTimeOffset.UtcNow.ToString(DATE_FORMAT);

        [JsonProperty(PropertyName = "StartTime", Order = 40)]
        public string StartTime => Uptime.Start.ToString(DATE_FORMAT);

        [JsonProperty(PropertyName = "UpTime", Order = 50)]
        public long UpTime => Convert.ToInt64(Uptime.Duration.TotalSeconds);

        /// <summary>
        /// Value generated at bootstrap by each instance of the service and
        /// used to correlate logs coming from the same instance. The value
        /// changes every time the service starts.
        /// </summary>
        [JsonProperty(PropertyName = "UID", Order = 60)]
        public string UID => Uptime.ProcessId;

        /// <summary>A property bag with details about the service</summary>
        [JsonProperty(PropertyName = "Properties", Order = 70)]
        public Dictionary<string, string> Properties = new Dictionary<string, string>();

        /// <summary>A property bag with details about the internal dependencies</summary>
        [JsonProperty(PropertyName = "Dependencies", Order = 80)]
        public Dictionary<string, StatusModel> Dependencies = new Dictionary<string, StatusModel>();

        [JsonProperty(PropertyName = "$metadata", Order = 1000)]
        public Dictionary<string, string> Metadata => new Dictionary<string, string>
        {
            { "$type", "Status;" + Version.NUMBER },
            { "$uri", "/" + Version.PATH + "/status" }
        };
    }
}
