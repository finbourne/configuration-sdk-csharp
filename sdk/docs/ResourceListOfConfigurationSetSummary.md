# Finbourne.Configuration.Sdk.Model.ResourceListOfConfigurationSetSummary

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Values** | [**List&lt;ConfigurationSetSummary&gt;**](ConfigurationSetSummary.md) |  | 
**Href** | **string** |  | [optional] 
**Links** | [**List&lt;Link&gt;**](Link.md) |  | [optional] 
**NextPage** | **string** |  | [optional] 
**PreviousPage** | **string** |  | [optional] 

```csharp
using Finbourne.Configuration.Sdk.Model;
using System;

List<ConfigurationSetSummary> values = new List<ConfigurationSetSummary>();
string href = "example href";
List<Link> links = new List<Link>();
string nextPage = "example nextPage";
string previousPage = "example previousPage";

ResourceListOfConfigurationSetSummary resourceListOfConfigurationSetSummaryInstance = new ResourceListOfConfigurationSetSummary(
    values: values,
    href: href,
    links: links,
    nextPage: nextPage,
    previousPage: previousPage);
```

[Back to Model list](../README.md#documentation-for-models) &#8226; [Back to API list](../README.md#documentation-for-api-endpoints) &#8226; [Back to README](../README.md)
