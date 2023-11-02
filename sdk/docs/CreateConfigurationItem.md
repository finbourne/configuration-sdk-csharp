# Finbourne.Configuration.Sdk.Model.CreateConfigurationItem
The information required to create a configuration item

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Key** | **string** | The key of the new configuration item | 
**Value** | **string** | The value of the new configuration item                The maximum size for secrets is 4KB and for text values is 2MB | 
**ValueType** | **string** | The type (text, number, boolean, textCollection, numberCollection) of the new configuration item&#39;s value.  The validation for each type is as follows:  - text: any value  - number: double (e.g. \&quot;5.5\&quot;)  - boolean: true/false  - textCollection: comma separated list (e.g. \&quot;a,b,c\&quot;)  - numberCollection: comma separated list of doubles (e.g. \&quot;1,2,3\&quot;) | [optional] 
**IsSecret** | **bool** | Defines whether or not the value is a secret | 
**Description** | **string** | The description of the new configuration item | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

