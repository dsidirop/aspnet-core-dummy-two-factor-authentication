#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Web/TwoFactorAuth.Web/TwoFactorAuth.Web.csproj", "Web/TwoFactorAuth.Web/"]
COPY ["Services/TwoFactorAuth.Services.Data/TwoFactorAuth.Services.Data.csproj", "Services/TwoFactorAuth.Services.Data/"]
COPY ["Services/TwoFactorAuth.Services.Data.Contracts/TwoFactorAuth.Services.Data.Contracts.csproj", "Services/TwoFactorAuth.Services.Data.Contracts/"]
COPY ["Services/TwoFactorAuth.Services.Mapping/TwoFactorAuth.Services.Mapping.csproj", "Services/TwoFactorAuth.Services.Mapping/"]
COPY ["Services/TwoFactorAuth.Services.Mapping.Contracts/TwoFactorAuth.Services.Mapping.Contracts.csproj", "Services/TwoFactorAuth.Services.Mapping.Contracts/"]
COPY ["Data/TwoFactorAuth.Data.Models/TwoFactorAuth.Data.Models.csproj", "Data/TwoFactorAuth.Data.Models/"]
COPY ["Data/TwoFactorAuth.Data.Common/TwoFactorAuth.Data.Common.csproj", "Data/TwoFactorAuth.Data.Common/"]
COPY ["Common/TwoFactorAuth.Common/TwoFactorAuth.Common.csproj", "Common/TwoFactorAuth.Common/"]
COPY ["Web/TwoFactorAuth.Web.ViewModels/TwoFactorAuth.Web.ViewModels.csproj", "Web/TwoFactorAuth.Web.ViewModels/"]
COPY ["Data/TwoFactorAuth.Data/TwoFactorAuth.Data.csproj", "Data/TwoFactorAuth.Data/"]
COPY ["Common/TwoFactorAuth.Common.Contracts/TwoFactorAuth.Common.Contracts.csproj", "Common/TwoFactorAuth.Common.Contracts/"]
COPY ["Services/TwoFactorAuth.Services.Auth/TwoFactorAuth.Services.Auth.csproj", "Services/TwoFactorAuth.Services.Auth/"]
COPY ["Services/TwoFactorAuth.Services.Auth.Contracts/TwoFactorAuth.Services.Auth.Contracts.csproj", "Services/TwoFactorAuth.Services.Auth.Contracts/"]
COPY ["Services/TwoFactorAuth.Services/TwoFactorAuth.Services.csproj", "Services/TwoFactorAuth.Services/"]
COPY ["Services/TwoFactorAuth.Services.Contracts/TwoFactorAuth.Services.Contracts.csproj", "Services/TwoFactorAuth.Services.Contracts/"]
COPY ["Services/TwoFactorAuth.Services.Messaging/TwoFactorAuth.Services.Messaging.csproj", "Services/TwoFactorAuth.Services.Messaging/"]
COPY ["Services/TwoFactorAuth.Services.Messaging.Contracts/TwoFactorAuth.Services.Messaging.Contracts.csproj", "Services/TwoFactorAuth.Services.Messaging.Contracts/"]
COPY ["Web/TwoFactorAuth.Web.Infrastructure/TwoFactorAuth.Web.Infrastructure.csproj", "Web/TwoFactorAuth.Web.Infrastructure/"]
RUN dotnet restore "Web/TwoFactorAuth.Web/TwoFactorAuth.Web.csproj"
COPY . .
WORKDIR "/src/Web/TwoFactorAuth.Web"
RUN dotnet build "TwoFactorAuth.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TwoFactorAuth.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TwoFactorAuth.Web.dll"]