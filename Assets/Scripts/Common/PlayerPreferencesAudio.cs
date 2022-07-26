using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Assets.Scripts.Common
{
    [JsonConverter(typeof(StringEnumConverter))]
    public static class PlayerPreferencesAudio
    {
        public static readonly string AmbientVolume = "AmbientVolume";
        public static readonly string EffectsVolume = "EffectVolume";
    }
}
