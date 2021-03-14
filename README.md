![Rider](https://bitbucket.org/dsidirop/gighub/raw/3053c14bf8cf41ce78cec197828b66353422c914/Collateral/Badges/rider.png)
![Resharper](https://bitbucket.org/dsidirop/gighub/raw/3053c14bf8cf41ce78cec197828b66353422c914/Collateral/Badges/resharper.png)

![ASPNET](https://bitbucket.org/dsidirop/gighub/raw/3a22265aecb7a0af8cd2c58f7ee445209cb15ec2/Collateral/Badges/AspNet.png)
![Autofac](https://user-images.githubusercontent.com/7284501/110253283-248b6500-7f92-11eb-993c-73eaadfc4830.png)
![Bootstrap](https://bitbucket.org/dsidirop/gighub/raw/3a22265aecb7a0af8cd2c58f7ee445209cb15ec2/Collateral/Badges/Bootstrap.png)
![jQuery](https://bitbucket.org/dsidirop/gighub/raw/3a22265aecb7a0af8cd2c58f7ee445209cb15ec2/Collateral/Badges/jQuery.png)

# ASPNET CORE (.Net5.x) Two-Factor Authentication Proof-of-Concept Scaffold

Proof of concept website for two factor authentication (2 challenges ala "guess the password").

The site supports login via a simple 2-stage password challenge:

1. The first screen is the landing page which has just a simple password field and a submit ('login') button.
    It also contains a *hidden* input field right before the visible password field, containing the
    password in a numeric format. When the user submits the correct password he is redirected to
    the second screen.

2. The second screen has (again) a password input field and a submit ('login') button. It also has a specially
    crafted image containing the correct password embedded into the bottom of the image. When the user types in
    the correct password he's navigated to next and final view.

3. The third screen should include a title congratulating the user on logging in. It should include a download
    button. The download button should download this README.md file.

This is essentially a custom two-factor authentication mechanism. Like with all sane two-factor authentication mechanisms,
provisions have been taken so that users who haven't succeeded in the first password challenge within the last X hours
(where X is configurable and set to 24-hours by default) cannot proceed to step 2 via deep-link. If they attempt to do so
they get redirected back to step 1.

# IDE Build & Run

0. Make sure:

   a. You have SQL Server 2012+ running on your local machine (including services).

   b. You have a DB called TwoFactorAuth and a db-user & owner of it called TwoFactorAuth with password '123456789'.

   c. Doublecheck that your SqlServer allows logging both with Windows Authentication and via [Sql Server Directly](https://serverfault.com/a/399871/502822).

   d. Last but not least edit appsettings.json and change 'Server=DROOD\\SQLEXPRESS' to whatever your local server is running as

1. cd Code

2. dotnet restore

3. Open your .Net5.x-aware IDE of choice and build/run the solution

To build a docker image and run it use (in a single line):

          docker
                build
                -f "C:\<path to>\aspnet-core-dummy-two-factor-authentication\Code\Web\TwoFactorAuth.Web\Dockerfile"
                --force-rm
                -t twofactorauthweb:dev
                --target base
                --label "com.microsoft.created-by=visual-studio"
                --label "com.microsoft.visual-studio.project-name=TwoFactorAuth.Web"
                "C:\<path to>\aspnet-core-dummy-two-factor-authentication\Code" 

## Project Overview

The project is based off the dotnet template [ASP.NET-Core-Template](https://github.com/NikolayIT/ASP.NET-Core-Template/tree/master/)
kind courtesy of [Nikolay Kostov](https://github.com/NikolayIT). It has been modified to employ Autofac for dependency injection.

![image](https://user-images.githubusercontent.com/7284501/109699777-a72eb180-7b99-11eb-9064-770c029336f9.png)
![Dependencies Graph](https://user-images.githubusercontent.com/7284501/109700429-64210e00-7b9a-11eb-9de0-d91756719a4c.png)

### Web

This solution folder contains three subfolders:

- TwoFactorAuth.Web
- TwoFactorAuth.Web.ViewModels
- TwoFactorAuth.Web.Infrastructure

#### TwoFactorAuth.Web

[TwoFactorAuth.Web](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Web/TwoFactorAuth.Web) self explanatory.

The 2-stage authentication mechanism is implemented via the 'DummyTwoFactorAuthController'.

Note: In development mode the Web project advertises its API via Swagger under /swagger.

#### TwoFactorAuth.Web.ViewModels

[TwoFactorAuth.Web.ViewModels](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Web/TwoFactorAuth.Web.ViewModels) contains objects, which will be
mapped from/to our entities and used in the front-end/back-end.

#### TwoFactorAuth.Web.Infrastructure

[TwoFactorAuth.Web.Infrastructure](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Web/TwoFactorAuth.Web.Infrastructure) contains functionality
like Middlewares and Filters.

### Common

**TwoFactorAuth.Common** contains common things for the project solution. For example:

- [GlobalConstants.cs](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/blob/master/Code/TwoFactorAuth.Common/GlobalConstants.cs).

### Data

This solution folder contains three subfolders:

- TwoFactorAuth.Data.Common
- TwoFactorAuth.Data.Models
- TwoFactorAuth.Data

#### TwoFactorAuth.Data.Common

[TwoFactorAuth.Data.Common.Models](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Data/TwoFactorAuth.Data.Common/Models) provides abstract
generics classes and interfaces, which holds information about our entities. For example when the object is Created, Modified, Deleted or IsDeleted. It contains a property
for the primary key as well.

[TwoFactorAuth.Data.Common.Repositories](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Data/TwoFactorAuth.Data.Common/Repositories) provides
two interfaces IDeletableEntityRepository and IRepository, which are part of the **repository pattern**.

#### TwoFactorAuth.Data.Models

[TwoFactorAuth.Data.Models](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Data/TwoFactorAuth.Data.Models) contains ApplicationUser and
ApplicationRole classes, which inherits IdentityRole and IdentityUsers.

#### TwoFactorAuth.Data

[TwoFactorAuth.Data](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Data/TwoFactorAuth.Data) contains DbContext, Migrations and Configuraitons
for the EF Core.There is Seeding and Repository functionality as well.

### Services

This solution folder contains four subfolders:

- TwoFactorAuth.Services
- TwoFactorAuth.Services.Data
- TwoFactorAuth.Services.Mapping
- TwoFactorAuth.Services.Messaging

#### TwoFactorAuth.Services

[TwoFactorAuth.Services](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Services/TwoFactorAuth.Services)

#### TwoFactorAuth.Services.Data

[TwoFactorAuth.Services.Data](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Services/TwoFactorAuth.Services.Data) wil contains service layer logic.

#### TwoFactorAuth.Services.Mapping

[TwoFactorAuth.Services.Mapping](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Services/TwoFactorAuth.Services.Mapping) provides simplified
functionlity for auto mapping. For example:

```csharp
using Blog.Data.Models;
using Blog.Services.Mapping;

public class TagViewModel : IMapFrom<Tag>
{
    public int Id { get; set; }

    public string Name { get; set; }
}
```

Or if you have something specific:

```csharp
using System;

using AutoMapper;
using Blog.Data.Models;
using Blog.Services.Mapping;

public class IndexPostViewModel : IMapFrom<Post>, IHaveCustomMappings
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Author { get; set; }

    public string ImageUrl { get; set; }

    public DateTime CreatedOn { get; set; }

    public void CreateMappings(IProfileExpression configuration)
    {
        configuration.CreateMap<Post, IndexPostViewModel>()
            .ForMember(
                source => source.Author,
                destination => destination.MapFrom(member => member.ApplicationUser.UserName));
    }
}

```

#### TwoFactorAuth.Services.Messaging

[TwoFactorAuth.Services.Messaging](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Services/TwoFactorAuth.Services.Messaging) a ready
to use integration with [SendGrid](https://sendgrid.com/).

### Tests

This solution folder contains three subfolders:

- TwoFactorAuth.Services.Data.Tests
- TwoFactorAuth.Web.Tests
- Sandbox

#### TwoFactorAuth.Services.Data.Tests

[TwoFactorAuth.Services.Data.Tests](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Tests/TwoFactorAuth.Services.Data.Tests) holds unit
tests for our service layer with ready setted up xUnit.

#### TwoFactorAuth.Web.Tests

[TwoFactorAuth.Web.Tests](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Tests/TwoFactorAuth.Web.Tests) setted up Selenuim tests.

#### Sandbox

[Sandbox](https://github.com/dsidirop/aspnet-core-dummy-two-factor-authentication/tree/master/Code/Tests/Sandbox) can be used to test your logic.

