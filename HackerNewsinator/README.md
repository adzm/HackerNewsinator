# HackerNewsinator

Displays new articles and search results of Hacker News.

## Platform

- .NET 5 (.NET Core)
- ASP.NET Core (C#)
- Angular 8 (TypeScript)
- Visual Studio 2019 (optional) for development and debugging

## History

This project was generated via the Visual Studio ASP.NET Core with Angular template. Unit test and integration test projects were added. Interfaces for dependency injection were created for both the C# backend and Angular front-end. 

## Features

- Search results provided by Algolia
- Infinite scrolling of new articles and search results.
- Pagination based on last item rather than offsets, which handles underlying cache / page changes without duplicating or skipping items
- Backend cache of new articles and search results using IMemoryCache
- Backend distributed cache of item details using IDistributedCache (currently in-memory but the implementation can be changed to use SQL or Redis in the future)
- Dependency injection on backend and frontend

## Testing

From within Visual Studio, choose Run All Tests to test the C# / ASP.NET code. To test angular code and e2e test, then from within Visual Studio developer terminal at the `HackerNewsInator/ClientApp` path, run `ng test` and `ng e2e` respectively.

C# tests run via [xUnit](https://xunit.net/).

Angular unit tests run via [Karma](https://karma-runner.github.io).

Angular e2e tests run via [Protractor](http://www.protractortest.org/).

## Shortcomings

The infrastructure exists to build further tests while ensuring the existing simple tests do not fail. This setup unfortunately consumed the better part of the time I invested in this project, but that is thankfully a one-time cost.

Angular and e2e testing is somewhat new to me with the ASP.NET Core backend. I need to figure out how to get the ASP.NET Core backend to run during e2e testing so better tests can be performed. Additionally, the Angular components and services definitely need to be split up further for better component testing and mocking. The ASP.NET Core backend also needs further tests and mocks.

Caching is always a hard problem, and the options used are for demonstration only. These should be configurable and probably smarter, but suffice in this scenario.

Error handling needs to be greatly improved for a better user experience and also extensively tested as much as the success paths.

No Denial-of-Service safeguards. Service can be overwhelmed by floods of requets, though that may be a better job for software on the edge. Cache can be bloated with lots of malicious search requests.
