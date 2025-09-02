# Finbourne.Configuration.Sdk.Model.CreateConfigurationSet
The information required to create a new configuration set

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | [**ResourceId**](ResourceId.md) |  | 
**Type** | **string** | The type (personal or shared) of the new configuration set | 
**Description** | **string** | The description of the new configuration set | [optional] 

```csharp
using Finbourne.Configuration.Sdk.Model;
using System;

ResourceId id = new ResourceId();
string type = "type";
string description = "example description";

CreateConfigurationSet createConfigurationSetInstance = new CreateConfigurationSet(
    id: id,
    type: type,
    description: description);
```

[Back to Model list](../README.md#documentation-for-models) &#8226; [Back to API list](../README.md#documentation-for-api-endpoints) &#8226; [Back to README](../README.md)
