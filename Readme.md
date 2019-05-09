[![Build Status](https://dev.azure.com/dnutiu-pub/NucuPaste/_apis/build/status/dnutiu.NucuPaste?branchName=master)](https://dev.azure.com/dnutiu-pub/NucuPaste/_build/latest?definitionId=1&branchName=master)

[![Board Status](https://dev.azure.com/dnutiu-pub/8caeb9b3-d25e-4627-aada-1f683fdabb6b/201ad6d2-3e46-4e37-b93e-8ba531d2a415/_apis/work/boardbadge/32d9a22c-152b-44a0-ac2c-0d3711f9a5dc?columnOptions=1)](https://dev.azure.com/dnutiu-pub/8caeb9b3-d25e-4627-aada-1f683fdabb6b/_boards/board/t/201ad6d2-3e46-4e37-b93e-8ba531d2a415/Microsoft.RequirementCategory)

This is my attempt at creating a Pastebin like web service in ASP .Net Core 2.

I've took this challenge in order to familiarize myself with ASP .Net Core 2 and the .Net ecosystem,
this repo can also serve as an example on how to build web applications APIs using ASP.NET Core 2.

In order to track my progress and use continuous integration this project uses 
[Azure DevOps](https://azure.microsoft.com/en-us/services/devops/), a free service provided by Microsoft.

## Developing

__Prerequisites__

* ASP.Net Core 2
* Docker

Start the services with the `docker-compose up`, create the database tables using `dotnet ef database update`
and jump right in!
