FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["NucuPaste/NucuPaste.csproj", "NucuPaste/"]
RUN dotnet restore "NucuPaste/NucuPaste.csproj"
COPY . .
WORKDIR "/src/NucuPaste"
RUN dotnet build "NucuPaste.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "NucuPaste.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NucuPaste.dll"]