# TwitterOhana

Developed on Windows 8 with Visual Studio 2017, .NetCoreApp 1.1

### Setting Up

Download Nuget packages:

- [Tweetinvi](https://github.com/linvi/tweetinvi)
`Install-Package TweetinviAPI`

- [Microsoft.AspNetCore.Session](https://www.nuget.org/packages/Microsoft.AspNetCore.Session/)
`Install-Package Microsoft.AspNetCore.Session`

- [StructureMap](http://structuremap.github.io/) 
`Install-Package Microsoft.AspNetCore.Session -Version 1.1.2`

- [StructureMap.Microsoft.DependencyInjection](https://www.nuget.org/packages/StructureMap.Microsoft.DependencyInjection/)
`Install-Package StructureMap.Microsoft.DependencyInjection`

- [HtmlSanitizer](https://github.com/mganss/HtmlSanitizer)

Get your Twitter App Credentials and change `appsettings.json`

### Development 
- [MVC](https://docs.microsoft.com/en-us/aspnet/core/mvc/overview) Model-View-Controller Pattern 
implemented as a way to achieve [separation of concerns](http://deviq.com/separation-of-concerns/). 

Using this pattern, user requests are routed to a Controller which is responsible for working with the Model to perform user actions and/or retrieve results of queries. The Controller chooses the View to display to the user, and provides it with any Model data it requires.
- [DIP](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection) Dependency Inversion Principle as part of the [SOLID design principles](https://en.wikipedia.org/wiki/SOLID_(object-oriented_design))

> - High-level modules should not depend on low-level modules. Both should depend on abstractions.
> - Abstractions should not depend on details. Details should depend on abstractions.

@ creds to Uncle Bob

- Cookie and [Claims-Based Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authorization/claims)
- [TDD](http://agiledata.org/essays/tdd.html) Test Driven Methodology
> Law 1: You are not allowed to write any production code until you have first written a test that fails

> Law 2: You are not allowed to write more of a test then it is sufficient to fail

> Law 3: You are not allowed to wirte any more production code then it is sufficient to pass but currently failing test

@ creds to Uncle Bob

- XSS Prevention

### Testing Methodologies

- [Unit Testing](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test) using [xUnit](https://xunit.github.io/docs/getting-started-dotnet-core)

- [Integration Testing](https://docs.microsoft.com/en-us/aspnet/core/testing/integration-testing)

- [Acceptance Testing](http://softwaretestingfundamentals.com/acceptance-testing/) carried out manually



Useful Resources:
- [Getting Started with Structuremap in ASP.NET CORE](https://andrewlock.net/getting-started-with-structuremap-in-asp-net-core/)
- [Uncle Bob's blog](http://blog.cleancoder.com/)
