<p align="center">
  <img height=250 src="./readme/icon.png"/>
</p>

# Acai

<p align="center">
  <img width=800 src="./readme/carousel.png"/>
</p>

Acai is a lightweight Food Journaling and (soon to be) Workout Logging App for Android devices built using .NET and .NET MAUI. Driven by a SQLite backend, it currently supports:
- Day-to-day cataloging of food consumed and caloric content to local device storage.
- User-configurable toggles for display and logging of principal macro-nutrients.
- Optional storage of commonly-used food items for quick logging.

# Project Structure
The full .NET solution for Acai consists of 3 projects:
| Project Name | Purpose |
|---|---|
|`AcaiCore`| .NET 8 Class Library Project which houses all essential business logic and rules. This is done to separate concerns and keep business logic decoupled from UI. In theory, it should be possible to build for other platforms by building on top of `AcaiCore` as a foundation should one wish. |
|`AcaiTests`| .NET 8 Project which houses Unit Tests for `AcaiCore` written using NUnit and Moq. When making changes to AcaiCore, a [Test-Driven](https://martinfowler.com/bliki/TestDrivenDevelopment.html) approach to Development is encouraged. |
|`AcaiMobile`| .NET MAUI Project which is built on top of `AcaiCore`. Contains logic and files for building the Android application. |


# Credits

Acai relies on several open-source packages and technologies which are subject to the following licenses:

## AcaiCore
- [SQLite](https://www.sqlite.org); provided in the [Public Domain](https://www.sqlite.org/copyright.html).
- [Microsoft.Data.Sqlite](https://www.nuget.org/packages/Microsoft.Data.Sqlite/8.0.6); provided under the [MIT License](./credits/Microsoft.Data.Sqlite). 

## AcaiTests
- [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk/17.5.0); provided under [Microsoft Software License Terms](./credits/Microsoft.NET.Test.Sdk).
- [NUnit](https://www.nuget.org/packages/NUnit/3.13.3); provided under the [MIT License](./credits/NUnit).
- [NUnit3TestAdapter](https://www.nuget.org/packages/NUnit3TestAdapter/4.4.2); provided under the [MIT License](./credits/NUnit3TestAdapter).
- [NUnit.Analyzers](https://www.nuget.org/packages/NUnit.Analyzers/3.6.1); provided under the [MIT License](./credits/NUnit.Analyzers).
- [coverlet.collector](https://www.nuget.org/packages/coverlet.collector/3.2.0); provided under the [MIT License](./credits/coverlet.collector).
- [Moq](https://www.nuget.org/packages/Moq/4.20.70); provided under the [BSD-3-Clause](./credits/Moq).

## AcaiMobile
- [Microsoft.Maui.Controls](https://www.nuget.org/packages/Microsoft.Maui.Controls/8.0.71); provided under the [MIT License](./credits/Microsoft.Maui.Controls).
- [CommunityToolkit.Maui](https://www.nuget.org/packages/CommunityToolkit.Maui/9.0.3); provided under the [MIT License](./credits/CommunityToolkit.Maui).
- [CommunityToolkit.Maui.Core](https://www.nuget.org/packages/CommunityToolkit.Maui.Core/9.0.3); provided under the [MIT License](./credits/CommunityToolkit.Maui.Core).
- [CommunityToolkit.Mvvm](https://www.nuget.org/packages/CommunityToolkit.Mvvm/8.3.2); provided under the [MIT License](./credits/CommunityToolkit.Mvvm).
