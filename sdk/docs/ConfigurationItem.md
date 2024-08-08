# Finbourne.Configuration.Sdk.Model.ConfigurationItem
The full version of the configuration item

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**CreatedAt** | **DateTimeOffset** | The date referring to the creation date of the configuration item | 
**CreatedBy** | **string** | Who created the configuration item | 
**LastModifiedAt** | **DateTimeOffset** | The date referring to the date when the configuration item was last modified | 
**LastModifiedBy** | **string** | Who modified the configuration item most recently | 
**Description** | **string** | Describes the configuration item | [optional] 
**Key** | **string** | The key which identifies the configuration item | 
**Value** | **string** | The value of the configuration item | 
**ValueType** | **string** | The type of the configuration item&#39;s value | 
**IsSecret** | **bool** | Defines whether or not the value is a secret. | 
**Ref** | **string** | The reference to the configuration item | [readonly] 
**Links** | [**List&lt;Link&gt;**](Link.md) |  | [optional] 

[Back to Model list](../README.md#documentation-for-models) &#8226; [Back to API list](../README.md#documentation-for-api-endpoints) &#8226; [Back to README](../README.md)

