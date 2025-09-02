# Finbourne.Configuration.Sdk.Model.UpdateConfigurationItem
The information required to update a configuration item

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Value** | **string** | The new value of the configuration item | 
**Description** | **string** | The new description of the configuration item | [optional] 
**BlockReveal** | **bool** | The requested new state of BlockReveal | [optional] 

```csharp
using Finbourne.Configuration.Sdk.Model;
using System;

string value = "value";
string description = "example description";
bool blockReveal = //"True";

UpdateConfigurationItem updateConfigurationItemInstance = new UpdateConfigurationItem(
    value: value,
    description: description,
    blockReveal: blockReveal);
```

[Back to Model list](../README.md#documentation-for-models) &#8226; [Back to API list](../README.md#documentation-for-api-endpoints) &#8226; [Back to README](../README.md)
