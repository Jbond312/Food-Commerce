using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace SystemSetting.Library.Model
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
  public class SystemSettingsDto {
    /// <summary>
    /// Gets or Sets Allergens
    /// </summary>
    [DataMember(Name="allergens", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "allergens")]
    public List<string> Allergens { get; set; }

    /// <summary>
    /// Gets or Sets DietaryPreferences
    /// </summary>
    [DataMember(Name="dietaryPreferences", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "dietaryPreferences")]
    public List<DietaryPreferenceDto> DietaryPreferences { get; set; }

    /// <summary>
    /// Gets or Sets CookCategories
    /// </summary>
    [DataMember(Name="cookCategories", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cookCategories")]
    public List<string> CookCategories { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SystemSettingsDto {\n");
      sb.Append("  Allergens: ").Append(Allergens).Append("\n");
      sb.Append("  DietaryPreferences: ").Append(DietaryPreferences).Append("\n");
      sb.Append("  CookCategories: ").Append(CookCategories).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
