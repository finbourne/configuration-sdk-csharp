/*
 * FINBOURNE ConfigurationService API
 *
 * Contact: info@finbourne.com
 * Generated by: https://github.com/openapitools/openapi-generator.git
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using OpenAPIDateConverter = Finbourne.Configuration.Sdk.Client.OpenAPIDateConverter;

namespace Finbourne.Configuration.Sdk.Model
{
    /// <summary>
    /// The information required to update a configuration item
    /// </summary>
    [DataContract(Name = "UpdateConfigurationItem")]
    public partial class UpdateConfigurationItem : IEquatable<UpdateConfigurationItem>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateConfigurationItem" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected UpdateConfigurationItem() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateConfigurationItem" /> class.
        /// </summary>
        /// <param name="value">The new value of the configuration item (required).</param>
        /// <param name="description">The new description of the configuration item.</param>
        /// <param name="blockReveal">The requested new state of BlockReveal.</param>
        public UpdateConfigurationItem(string value = default(string), string description = default(string), bool blockReveal = default(bool))
        {
            // to ensure "value" is required (not null)
            if (value == null)
            {
                throw new ArgumentNullException("value is a required property for UpdateConfigurationItem and cannot be null");
            }
            this.Value = value;
            this.Description = description;
            this.BlockReveal = blockReveal;
        }

        /// <summary>
        /// The new value of the configuration item
        /// </summary>
        /// <value>The new value of the configuration item</value>
        [DataMember(Name = "value", IsRequired = true, EmitDefaultValue = true)]
        public string Value { get; set; }

        /// <summary>
        /// The new description of the configuration item
        /// </summary>
        /// <value>The new description of the configuration item</value>
        [DataMember(Name = "description", EmitDefaultValue = true)]
        public string Description { get; set; }

        /// <summary>
        /// The requested new state of BlockReveal
        /// </summary>
        /// <value>The requested new state of BlockReveal</value>
        [DataMember(Name = "blockReveal", EmitDefaultValue = true)]
        public bool BlockReveal { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class UpdateConfigurationItem {\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  BlockReveal: ").Append(BlockReveal).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public virtual string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return this.Equals(input as UpdateConfigurationItem);
        }

        /// <summary>
        /// Returns true if UpdateConfigurationItem instances are equal
        /// </summary>
        /// <param name="input">Instance of UpdateConfigurationItem to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UpdateConfigurationItem input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Value == input.Value ||
                    (this.Value != null &&
                    this.Value.Equals(input.Value))
                ) && 
                (
                    this.Description == input.Description ||
                    (this.Description != null &&
                    this.Description.Equals(input.Description))
                ) && 
                (
                    this.BlockReveal == input.BlockReveal ||
                    this.BlockReveal.Equals(input.BlockReveal)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hashCode = 41;
                if (this.Value != null)
                {
                    hashCode = (hashCode * 59) + this.Value.GetHashCode();
                }
                if (this.Description != null)
                {
                    hashCode = (hashCode * 59) + this.Description.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.BlockReveal.GetHashCode();
                return hashCode;
            }
        }

        /// <summary>
        /// To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            // Value (string) maxLength
            if (this.Value != null && this.Value.Length > 2000000)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Value, length must be less than 2000000.", new [] { "Value" });
            }

            // Value (string) minLength
            if (this.Value != null && this.Value.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Value, length must be greater than 1.", new [] { "Value" });
            }

            // Value (string) pattern
            Regex regexValue = new Regex(@"(?s).*", RegexOptions.CultureInvariant);
            if (false == regexValue.Match(this.Value).Success)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Value, must match a pattern of " + regexValue, new [] { "Value" });
            }

            // Description (string) maxLength
            if (this.Description != null && this.Description.Length > 255)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Description, length must be less than 255.", new [] { "Description" });
            }

            // Description (string) minLength
            if (this.Description != null && this.Description.Length < 0)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Description, length must be greater than 0.", new [] { "Description" });
            }

            yield break;
        }
    }
}
