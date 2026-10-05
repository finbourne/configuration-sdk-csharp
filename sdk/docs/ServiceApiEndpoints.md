# Finbourne.Configuration.Sdk.Model.ServiceApiEndpoints

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Application** | **string** |  | 
**Endpoints** | [**List&lt;ApiEndpoint&gt;**](ApiEndpoint.md) |  | 
**Href** | **string** |  | [optional] 
**Links** | [**List&lt;Link&gt;**](Link.md) |  | [optional] 

```csharp
using Finbourne.Configuration.Sdk.Model;
using System;

string application = "application";
List<ApiEndpoint> endpoints = new List<ApiEndpoint>();
string href = "example href";
List<Link> links = new List<Link>();

ServiceApiEndpoints serviceApiEndpointsInstance = new ServiceApiEndpoints(
    application: application,
    endpoints: endpoints,
    href: href,
    links: links);
```

[Back to Model list](../README.md#documentation-for-models) &#8226; [Back to API list](../README.md#documentation-for-api-endpoints) &#8226; [Back to README](../README.md)
