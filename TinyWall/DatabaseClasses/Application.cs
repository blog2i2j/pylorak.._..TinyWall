﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace pylorak.TinyWall.DatabaseClasses
{
    [DataContract(Namespace = "TinyWall")]
    internal class Application
    {
        // Application name
        [DataMember(EmitDefaultValue = false)]
        internal string Name { get; set; } = string.Empty;

        internal string LocalizedName
        {
            get
            {
                try
                {
                    string ret = Resources.Exceptions.ResourceManager.GetString(Name);
                    return string.IsNullOrEmpty(ret) ? Name : ret;
                }
                catch
                {
                    return Name;
                }
            }
        }

        // Executables that belong to this application
        [DataMember(EmitDefaultValue = false)]
        internal List<SubjectIdentity> Components { get; set; } = new List<SubjectIdentity>();

        public override string ToString()
        {
            return this.Name;
        }

        [DataMember(Name = "Flags", EmitDefaultValue = false)]
        private Dictionary<string, string?>? Flags;

        public void SetFlag(string flag, string? value = null)
        {
            if (Flags is null)
                Flags = new Dictionary<string, string?>();

            Flags[flag.ToUpperInvariant()] = value;
        }
        public bool HasFlag(string flag)
        {
            if (Flags == null)
                return false;

            return Flags.ContainsKey(flag.ToUpperInvariant());
        }
        public string? GetFlagValue(string flag)
        {
            if (Flags == null)
                throw new KeyNotFoundException();

            return Flags[flag];
        }

        [OnDeserialized()]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (Components == null)
                Components = new List<SubjectIdentity>();
        }
    }
}
