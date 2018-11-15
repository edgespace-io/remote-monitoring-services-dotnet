﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.IoTSolutions.UIConfig.Services.Models;

namespace Microsoft.Azure.IoTSolutions.UIConfig.Services.External
{
    public class ConfigTypeList
    {
        private HashSet<String> customConfigTypes = new HashSet<String>();

        public string[] Items
        {
            get
            {
                return customConfigTypes.ToArray<String>();
            }
            set
            {
                Array.ForEach<String>(value, (c => customConfigTypes.Add(c)));
            }
        }

        internal void add(string customConfig)
        {
            customConfigTypes.Add(customConfig.Trim());
        }

    }
}