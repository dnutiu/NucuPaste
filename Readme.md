[![Build Status](https://dev.azure.com/dnutiu-pub/NucuPaste/_apis/build/status/dnutiu.NucuPaste?branchName=master)](https://dev.azure.com/dnutiu-pub/NucuPaste/_build/latest?definitionId=1&branchName=master)

[![Board Status](https://dev.azure.com/dnutiu-pub/8caeb9b3-d25e-4627-aada-1f683fdabb6b/201ad6d2-3e46-4e37-b93e-8ba531d2a415/_apis/work/boardbadge/32d9a22c-152b-44a0-ac2c-0d3711f9a5dc?columnOptions=1)](https://dev.azure.com/dnutiu-pub/8caeb9b3-d25e-4627-aada-1f683fdabb6b/_boards/board/t/201ad6d2-3e46-4e37-b93e-8ba531d2a415/Microsoft.RequirementCategory)

This is my attempt at creating a PasteBin like web service in ASP.NET Core 2.

I've took this challenge in order to familiarize myself with ASP.NET Core 2,
this repo can also serve as an example on how to build web applications APIs
using ASP.NET Core 2.


The following features have been implemented:

- Implement basic CRUD for the PasteController.
- Implement logging.
- Implement Swagger UI.
- Implement API versioning.
- xUnit tests for the PasteController. 

These are the features that I haven't implemented yet, feel free to make a pull request if you'd like:

- Implement authentication.
- Improve Model.
- Implement MongoDB/PostreSQL support.
- More tests.