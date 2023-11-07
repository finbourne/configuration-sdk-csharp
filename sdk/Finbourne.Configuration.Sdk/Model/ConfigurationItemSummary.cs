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
    /// A single configuration object
    /// </summary>
    [DataContract(Name = "ConfigurationItemSummary")]
    public partial class ConfigurationItemSummary : IEquatable<ConfigurationItemSummary>, IValidatableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationItemSummary" /> class.
        /// </summary>
        [JsonConstructorAttribute]
        protected ConfigurationItemSummary() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationItemSummary" /> class.
        /// </summary>
        /// <param name="key">The key which identifies the configuration item (required).</param>
        /// <param name="value">The value of the configuration item (required).</param>
        /// <param name="valueType">The type of the configuration item&#39;s value (required).</param>
        /// <param name="isSecret">Defines whether or not the value is a secret. (required).</param>
        /// <param name="links">links.</param>
        public ConfigurationItemSummary(string key = default(string), string value = default(string), string valueType = default(string), bool isSecret = default(bool), List<Link> links = default(List<Link>))
        {
            // to ensure "key" is required (not null)
            if (key == null)
            {
                throw new ArgumentNullException("key is a required property for ConfigurationItemSummary and cannot be null");
            }
            this.Key = key;
            // to ensure "value" is required (not null)
            if (value == null)
            {
                throw new ArgumentNullException("value is a required property for ConfigurationItemSummary and cannot be null");
            }
            this.Value = value;
            // to ensure "valueType" is required (not null)
            if (valueType == null)
            {
                throw new ArgumentNullException("valueType is a required property for ConfigurationItemSummary and cannot be null");
            }
            this.ValueType = valueType;
            this.IsSecret = isSecret;
            this.Links = links;
        }

        /// <summary>
        /// The key which identifies the configuration item
        /// </summary>
        /// <value>The key which identifies the configuration item</value>
        [DataMember(Name = "key", IsRequired = true, EmitDefaultValue = true)]
        public string Key { get; set; }

        /// <summary>
        /// The value of the configuration item
        /// </summary>
        /// <value>The value of the configuration item</value>
        [DataMember(Name = "value", IsRequired = true, EmitDefaultValue = true)]
        public string Value { get; set; }

        /// <summary>
        /// The type of the configuration item&#39;s value
        /// </summary>
        /// <value>The type of the configuration item&#39;s value</value>
        [DataMember(Name = "valueType", IsRequired = true, EmitDefaultValue = true)]
        public string ValueType { get; set; }

        /// <summary>
        /// Defines whether or not the value is a secret.
        /// </summary>
        /// <value>Defines whether or not the value is a secret.</value>
        [DataMember(Name = "isSecret", IsRequired = true, EmitDefaultValue = true)]
        public bool IsSecret { get; set; }

        /// <summary>
        /// The reference to the configuration item
        /// </summary>
        /// <value>The reference to the configuration item</value>
        [DataMember(Name = "ref", IsRequired = true, EmitDefaultValue = true)]
        public string Ref { get; private set; }

        /// <summary>
        /// Returns false as Ref should not be serialized given that it's read-only.
        /// </summary>
        /// <returns>false (boolean)</returns>
        public bool ShouldSerializeRef()
        {
            return false;
        }
        /// <summary>
        /// Gets or Sets Links
        /// </summary>
        [DataMember(Name = "links", EmitDefaultValue = true)]
        public List<Link> Links { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("class ConfigurationItemSummary {\n");
            sb.Append("  Key: ").Append(Key).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  ValueType: ").Append(ValueType).Append("\n");
            sb.Append("  IsSecret: ").Append(IsSecret).Append("\n");
            sb.Append("  Ref: ").Append(Ref).Append("\n");
            sb.Append("  Links: ").Append(Links).Append("\n");
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
            return this.Equals(input as ConfigurationItemSummary);
        }

        /// <summary>
        /// Returns true if ConfigurationItemSummary instances are equal
        /// </summary>
        /// <param name="input">Instance of ConfigurationItemSummary to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ConfigurationItemSummary input)
        {
            if (input == null)
            {
                return false;
            }
            return 
                (
                    this.Key == input.Key ||
                    (this.Key != null &&
                    this.Key.Equals(input.Key))
                ) && 
                (
                    this.Value == input.Value ||
                    (this.Value != null &&
                    this.Value.Equals(input.Value))
                ) && 
                (
                    this.ValueType == input.ValueType ||
                    (this.ValueType != null &&
                    this.ValueType.Equals(input.ValueType))
                ) && 
                (
                    this.IsSecret == input.IsSecret ||
                    this.IsSecret.Equals(input.IsSecret)
                ) && 
                (
                    this.Ref == input.Ref ||
                    (this.Ref != null &&
                    this.Ref.Equals(input.Ref))
                ) && 
                (
                    this.Links == input.Links ||
                    this.Links != null &&
                    input.Links != null &&
                    this.Links.SequenceEqual(input.Links)
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
                if (this.Key != null)
                {
                    hashCode = (hashCode * 59) + this.Key.GetHashCode();
                }
                if (this.Value != null)
                {
                    hashCode = (hashCode * 59) + this.Value.GetHashCode();
                }
                if (this.ValueType != null)
                {
                    hashCode = (hashCode * 59) + this.ValueType.GetHashCode();
                }
                hashCode = (hashCode * 59) + this.IsSecret.GetHashCode();
                if (this.Ref != null)
                {
                    hashCode = (hashCode * 59) + this.Ref.GetHashCode();
                }
                if (this.Links != null)
                {
                    hashCode = (hashCode * 59) + this.Links.GetHashCode();
                }
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
            // Key (string) minLength
            if (this.Key != null && this.Key.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Key, length must be greater than 1.", new [] { "Key" });
            }

            // Value (string) minLength
            if (this.Value != null && this.Value.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Value, length must be greater than 1.", new [] { "Value" });
            }

            // ValueType (string) minLength
            if (this.ValueType != null && this.ValueType.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for ValueType, length must be greater than 1.", new [] { "ValueType" });
            }

            // Ref (string) minLength
            if (this.Ref != null && this.Ref.Length < 1)
            {
                yield return new System.ComponentModel.DataAnnotations.ValidationResult("Invalid value for Ref, length must be greater than 1.", new [] { "Ref" });
            }

            yield break;
        }
    }
}