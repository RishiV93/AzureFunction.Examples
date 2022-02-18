# AzureFunction.Examples
Portfolio - AzureFunction.Examples based on theme parks context.

This is a small collection of samples I have built to demonstrate my understanding of Azure Function. It is written in C# with .Net 6 on Visual Studio 2022.

If you get to sift through the code you will find examples of the following:
* Generics - in the request extensions, this can deserialize the JSON payload to either a `ThemePark` or `Ride` class.
* Action Filters - Used in the Function classes themselves, logging is handled here thanks to the `OnExecutingAsync` method.
