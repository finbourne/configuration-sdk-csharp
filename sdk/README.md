<a id="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://fbn-prd.lusid.com/configuration*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*ApplicationMetadataApi* | [**ListAccessControlledResources**](docs/ApplicationMetadataApi.md#listaccesscontrolledresources) | **GET** /api/metadata/access/resources | [EARLY ACCESS] ListAccessControlledResources: Get resources available for access control
*ConfigurationSetsApi* | [**AddConfigurationToSet**](docs/ConfigurationSetsApi.md#addconfigurationtoset) | **POST** /api/sets/{type}/{scope}/{code}/items | [EARLY ACCESS] AddConfigurationToSet: Add a configuration item to an existing set
*ConfigurationSetsApi* | [**CheckAccessTokenExists**](docs/ConfigurationSetsApi.md#checkaccesstokenexists) | **HEAD** /api/sets/personal/me | [DEPRECATED] CheckAccessTokenExists: Check the Personal Access Token exists for the current user
*ConfigurationSetsApi* | [**CreateConfigurationSet**](docs/ConfigurationSetsApi.md#createconfigurationset) | **POST** /api/sets | [EARLY ACCESS] CreateConfigurationSet: Create a configuration set
*ConfigurationSetsApi* | [**DeleteAccessToken**](docs/ConfigurationSetsApi.md#deleteaccesstoken) | **DELETE** /api/sets/personal/me | [DEPRECATED] DeleteAccessToken: Delete any stored Personal Access Token for the current user
*ConfigurationSetsApi* | [**DeleteConfigurationItem**](docs/ConfigurationSetsApi.md#deleteconfigurationitem) | **DELETE** /api/sets/{type}/{scope}/{code}/items/{key} | [EARLY ACCESS] DeleteConfigurationItem: Remove the specified configuration item from the specified configuration set
*ConfigurationSetsApi* | [**DeleteConfigurationSet**](docs/ConfigurationSetsApi.md#deleteconfigurationset) | **DELETE** /api/sets/{type}/{scope}/{code} | [EARLY ACCESS] DeleteConfigurationSet: Deletes a configuration set along with all their configuration items
*ConfigurationSetsApi* | [**GenerateAccessToken**](docs/ConfigurationSetsApi.md#generateaccesstoken) | **PUT** /api/sets/personal/me | [DEPRECATED] GenerateAccessToken: Generate a Personal Access Token for the current user and stores it in the me token
*ConfigurationSetsApi* | [**GetConfigurationItem**](docs/ConfigurationSetsApi.md#getconfigurationitem) | **GET** /api/sets/{type}/{scope}/{code}/items/{key} | [EARLY ACCESS] GetConfigurationItem: Get the specific configuration item within an existing set
*ConfigurationSetsApi* | [**GetConfigurationSet**](docs/ConfigurationSetsApi.md#getconfigurationset) | **GET** /api/sets/{type}/{scope}/{code} | [EARLY ACCESS] GetConfigurationSet: Get a configuration set, including all the associated metadata. By default secrets will not be revealed
*ConfigurationSetsApi* | [**GetSystemConfigurationItems**](docs/ConfigurationSetsApi.md#getsystemconfigurationitems) | **GET** /api/sets/system/{code}/items/{key} | [EARLY ACCESS] GetSystemConfigurationItems: Get the specific system configuration items within a system set  All users have access to this endpoint
*ConfigurationSetsApi* | [**GetSystemConfigurationSets**](docs/ConfigurationSetsApi.md#getsystemconfigurationsets) | **GET** /api/sets/system/{code} | [EARLY ACCESS] GetSystemConfigurationSets: Get the specified system configuration sets, including all their associated metadata. By default secrets will not be revealed  All users have access to this endpoint
*ConfigurationSetsApi* | [**ListConfigurationSets**](docs/ConfigurationSetsApi.md#listconfigurationsets) | **GET** /api/sets | [EARLY ACCESS] ListConfigurationSets: List all configuration sets summaries (I.e. list of scope/code combinations available)
*ConfigurationSetsApi* | [**UpdateConfigurationItem**](docs/ConfigurationSetsApi.md#updateconfigurationitem) | **PUT** /api/sets/{type}/{scope}/{code}/items/{key} | [EARLY ACCESS] UpdateConfigurationItem: Update a configuration item's value and/or description
*ConfigurationSetsApi* | [**UpdateConfigurationSet**](docs/ConfigurationSetsApi.md#updateconfigurationset) | **PUT** /api/sets/{type}/{scope}/{code} | [EARLY ACCESS] UpdateConfigurationSet: Update the description of a configuration set


<a id="documentation-for-models"></a>
## Documentation for Models

 - [AccessControlledAction](docs/AccessControlledAction.md)
 - [AccessControlledResource](docs/AccessControlledResource.md)
 - [ActionId](docs/ActionId.md)
 - [ConfigurationItem](docs/ConfigurationItem.md)
 - [ConfigurationItemSummary](docs/ConfigurationItemSummary.md)
 - [ConfigurationSet](docs/ConfigurationSet.md)
 - [ConfigurationSetSummary](docs/ConfigurationSetSummary.md)
 - [CreateConfigurationItem](docs/CreateConfigurationItem.md)
 - [CreateConfigurationSet](docs/CreateConfigurationSet.md)
 - [IdSelectorDefinition](docs/IdSelectorDefinition.md)
 - [IdentifierPartSchema](docs/IdentifierPartSchema.md)
 - [Link](docs/Link.md)
 - [LusidProblemDetails](docs/LusidProblemDetails.md)
 - [LusidValidationProblemDetails](docs/LusidValidationProblemDetails.md)
 - [PersonalAccessToken](docs/PersonalAccessToken.md)
 - [ResourceId](docs/ResourceId.md)
 - [ResourceListOfAccessControlledResource](docs/ResourceListOfAccessControlledResource.md)
 - [ResourceListOfConfigurationItem](docs/ResourceListOfConfigurationItem.md)
 - [ResourceListOfConfigurationSet](docs/ResourceListOfConfigurationSet.md)
 - [ResourceListOfConfigurationSetSummary](docs/ResourceListOfConfigurationSetSummary.md)
 - [UpdateConfigurationItem](docs/UpdateConfigurationItem.md)
 - [UpdateConfigurationSet](docs/UpdateConfigurationSet.md)

