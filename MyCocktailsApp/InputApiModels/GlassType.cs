﻿namespace MyCocktailsApp.InputApiModels
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;

    [JsonConverter(typeof(StringEnumConverter))]
    public enum GlassType
    {
        [EnumMember(Value = "Highball glass")]
        Highball = 10,

        [EnumMember(Value = "Cocktail glass")]
        Cocktail = 20,

        [Description("Old-fashioned")]
        [EnumMember(Value = "Old-fashioned glass")]
        OldFashioned = 30,

        [Description("Copper Mug")]
        [EnumMember(Value = "Copper Mug")]
        CopperMug = 40,

        [EnumMember(Value = "Whiskey glass")]
        Whiskey = 50
    }
}
