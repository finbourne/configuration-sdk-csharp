# Finbourne.Configuration.Sdk.Model.ConfigurationSet
The full version of the configuration set

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**CreatedAt** | **DateTimeOffset** | The date referring to the creation date of the configuration set | 
**CreatedBy** | **string** | Who created the configuration set | 
**LastModifiedAt** | **DateTimeOffset** | The date referring to the date when the configuration set was last modified | 
**LastModifiedBy** | **string** | Who modified the configuration set most recently | 
**Description** | **string** | Describes the configuration set | [optional] 
**Items** | [**List&lt;ConfigurationItemSummary&gt;**](ConfigurationItemSummary.md) | The collection of the configuration items that this set contains. | [optional] 
**Id** | [**ResourceId**](ResourceId.md) |  | 
**Type** | **string** | The type (personal or shared) of the configuration set | 
**Links** | [**List&lt;Link&gt;**](Link.md) |  | [optional] 

```csharp
using Finbourne.Configuration.Sdk.Model;
using System;

string createdBy = "createdBy";
string lastModifiedBy = "lastModifiedBy";
string description = "example description";
List<ConfigurationItemSummary> items = new List<ConfigurationItemSummary>();
ResourceId id = new ResourceId();
string type = "type";
List<Link> links = new List<Link>();

ConfigurationSet configurationSetInstance = new ConfigurationSet(
    createdAt: createdAt,
    createdBy: createdBy,
    lastModifiedAt: lastModifiedAt,
    lastModifiedBy: lastModifiedBy,
    description: description,
    items: items,
    id: id,
    type: type,
    links: links);
```

[Back to Model list](../README.md#documentation-for-models) &#8226; [Back to API list](../README.md#documentation-for-api-endpoints) &#8226; [Back to README](../README.md)
