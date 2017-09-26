using System;
using Newtonsoft.Json;

namespace VSTS.Models
{
    public class Token
    {
        [JsonProperty(PropertyName = "access_token")]
        public String AccessToken { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public String TokenType { get; set; }

        [JsonProperty(PropertyName = "expires_in")]
        public String ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "refresh_token")]
        public String RefreshToken { get; set; }
    }
}