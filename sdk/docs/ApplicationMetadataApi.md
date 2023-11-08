# Finbourne.Configuration.Sdk.Api.ApplicationMetadataApi

All URIs are relative to *https://fbn-prd.lusid.com/configuration*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ListAccessControlledResources**](ApplicationMetadataApi.md#listaccesscontrolledresources) | **GET** /api/metadata/access/resources | [EARLY ACCESS] ListAccessControlledResources: Get resources available for access control |

<a id="listaccesscontrolledresources"></a>
# **ListAccessControlledResources**
> ResourceListOfAccessControlledResource ListAccessControlledResources ()

[EARLY ACCESS] ListAccessControlledResources: Get resources available for access control

Get the comprehensive set of resources that are available for access control

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Finbourne.Configuration.Sdk.Api;
using Finbourne.Configuration.Sdk.Client;
using Finbourne.Configuration.Sdk.Model;

namespace Example
{
    public class ListAccessControlledResourcesExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "https://fbn-prd.lusid.com/configuration";
            // Configure OAuth2 access token for authorization: oauth2
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new ApplicationMetadataApi(config);

            try
            {
                // [EARLY ACCESS] ListAccessControlledResources: Get resources available for access control
                ResourceListOfAccessControlledResource result = apiInstance.ListAccessControlledResources();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApplicationMetadataApi.ListAccessControlledResources: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ListAccessControlledResourcesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // [EARLY ACCESS] ListAccessControlledResources: Get resources available for access control
    ApiResponse<ResourceListOfAccessControlledResource> response = apiInstance.ListAccessControlledResourcesWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApplicationMetadataApi.ListAccessControlledResourcesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**ResourceListOfAccessControlledResource**](ResourceListOfAccessControlledResource.md)

### Authorization

[oauth2](../README.md#oauth2)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: text/plain, application/json, text/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success |  -  |
| **0** | Error response |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

