﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace SteamWebAPI2.Models.DOTA2
{
    public class Hero
    {
        public string Name { get; set; }
        public int Id { get; set; }
        
        [JsonProperty(PropertyName = "localized_name")]
        public string LocalizedName { get; set; }
    }

    public class HeroResult
    {
        public IList<Hero> Heroes { get; set; }
    }

    internal class HeroResultContainer
    {
        public HeroResult Result { get; set; }
    }
}