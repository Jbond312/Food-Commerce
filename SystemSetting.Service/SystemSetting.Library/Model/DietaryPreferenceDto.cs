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
  public class DietaryPreferenceDto {
    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets ContradictiveAllergen
    /// </summary>
    [DataMember(Name="contradictiveAllergen", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "contradictiveAllergen")]
    public List<string> ContradictiveAllergen { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DietaryPreferenceDto {\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  ContradictiveAllergen: ").Append(ContradictiveAllergen).Append("\n");
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
