# STAC API Broad HTTP Library Documentation

Main entry point of the DotNetStac.Api.Clients library is [Stac.Api.Clients.ApiClient](xref:Stac.Api.Clients.ApiClient) that provides a multi-purpose STAC API client that can be used to query a STAC API compliant service and organised as follow:

- `Core` client for the [Core API](https://github.com/radiantearth/stac-api-spec/tree/main/core)
- `Collections` client for the [Collections API](https://github.com/radiantearth/stac-api-spec/tree/main/ogcapi-features)
- `Features` client for the [Features API](https://github.com/radiantearth/stac-api-spec/tree/main/ogcapi-features)
- `ItemSearch` client for the [Item Search API](https://github.com/radiantearth/stac-api-spec/tree/main/item-search)
- `Extensions` helper class to access all the registered [extensions of the STAC API](https://stac-api-extensions.github.io/)
  
Each API has it's own client class that inherits from the [Stac.Api.Clients.StacApiClient](xref:Stac.Api.Clients.StacApiClient) class.
