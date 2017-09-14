﻿// Copyright (c) Microsoft. All rights reserved.

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Microsoft.Azure.IoTSolutions.DeviceTelemetry.Services.Exceptions;

namespace Microsoft.Azure.IoTSolutions.DeviceTelemetry.Services.Models
{
    public class Rule : IComparable<Rule>
    {
        private const string DATE_FORMAT = "yyyy-MM-dd'T'HH:mm:sszzz";

        public string ETag { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
        public bool Enabled { get; set; }
        public string Description { get; set; }
        public string GroupId { get; set; }
        public string Severity { get; set; }
        public List<Condition> Conditions { get; set; }

        public Rule()
        {
            this.ETag = string.Empty;
            this.Id = string.Empty;
            this.Name = string.Empty;
            this.DateCreated = DateTimeOffset.UtcNow.ToString(DATE_FORMAT);
            this.DateModified = DateTimeOffset.UtcNow.ToString(DATE_FORMAT);
            this.Enabled = false;
            this.Description = string.Empty;
            this.GroupId = string.Empty;
            this.Severity = string.Empty;
            this.Conditions = new List<Condition>();
        }

        public Rule(
            string name,
            bool enabled,
            string description,
            string groupId,
            string severity,
            List<Condition> conditions)
        {
            this.Name = name;
            this.Description = description;
            this.GroupId = groupId;
            this.Severity = severity;
            this.Conditions = conditions;

            this.ETag = string.Empty;
            this.Id = string.Empty;
            this.DateCreated = DateTimeOffset.UtcNow.ToString(DATE_FORMAT);
            this.DateModified = DateTimeOffset.UtcNow.ToString(DATE_FORMAT);
        }

        public Rule(
            string eTag,
            string id,
            string name,
            string dateCreated,
            string dateModified,
            bool enabled,
            string description,
            string groupId,
            string severity,
            List<Condition> conditions)
        {
            this.ETag = eTag;
            this.Id = id;
            this.Name = name;
            this.DateCreated = dateCreated;
            this.DateModified = dateModified;
            this.Enabled = enabled;
            this.Description = description;
            this.GroupId = groupId;
            this.Severity = severity;
            this.Conditions = conditions;
        }

        public Rule(JToken json)
        {
            try
            {
                JToken jsonRule = json;
                JArray jsonConditions = (JArray)jsonRule["Conditions"];

                this.Name = jsonRule["Name"].ToString();
                this.Enabled = jsonRule["Enabled"].ToObject<bool>();
                this.Description = jsonRule["Description"].ToString();
                this.GroupId = jsonRule["GroupId"].ToString();
                this.Severity = jsonRule["Severity"].ToString();

                // These values are not required when creating a new
                // rule as they are auto-generated by storage.
                this.ETag = jsonRule["ETag"]?.ToString() ?? string.Empty;
                this.Id = jsonRule["Id"]?.ToString() ?? string.Empty;
                this.DateCreated = jsonRule["DateCreated"]?.ToString() ?? DateTimeOffset.UtcNow.ToString(DATE_FORMAT);
                this.DateModified = jsonRule["DateModified"]?.ToString() ?? DateTimeOffset.UtcNow.ToString(DATE_FORMAT);                

                List<Condition> conditions = new List<Condition>();
                foreach (var condition in jsonConditions)
                {
                    conditions.Add(new Condition(
                        condition["Field"].ToString(),
                        condition["Operator"].ToString(),
                        condition["Value"].ToString()));
                }
                this.Conditions = conditions;
            }
            catch (Exception e)
            {
                throw new InvalidInputException("Invalid input for rule", e);
            }
        }

        public int CompareTo(Rule other)
        {
            if (other == null) return 1;

            return DateTimeOffset.Parse(other.DateCreated)
                .CompareTo(DateTimeOffset.Parse(this.DateCreated));
        }
    }
}
