# DevAssignment 

Explain the usage of unit test and how to use the mock objects for unit test while demostrating the architecure capabilty.
I try to ran an extra mile with this architecure keeping main objective in mind.:)

## Requirement

PROBLEM #1

Given the following interface:
```
public interface IAccountService
{
   double GetAccountAmount(int accountId);
}
```
...and the following class that depends on this interface:
```
public class AccountInfo
{
   private readonly int _accountId;
   private readonly IAccountService _accountService;
   public AccountInfo(int accountId, IAccountService accountService)
{
   _accountId = accountId;
   _accountService = accountService;
}
public double Amount { get; private set; }
public void RefreshAmount()
{
   Amount = _accountService.GetAccountAmount(_accountId);
}
}
```
REQUIRED: Write a unit test that asserts the behaviour of RefreshAmount() method.

PROBLEM #2

It has been determined that IAccountService.GetAccountAmount() is a potentially slow and
unreliable remote network call and that it should be made asynchronous. Note that
AccountInfo.RefreshAmount() may be invoked multiple times concurrently. Adjust
IAccountService and / or AccountInfo and your tests accordingly.

PROBLEM #3
Write a build script. Consider compilation, test execution and producing a nuget package.

## Architecture

This project implements NTier architecure which contains,

1.UI - User interface layer (This can contain any js framework or MVC web app)

2.API - This can contain Any API (Ex: web API, WCF) 

3.Services - Service layer of the appplication where all the service functionality goes here

4.Domain - Domain related  entities,Dtos goes here

5.Infrastructure - Depends on the ORM or the DB communication this can contain releated main functionality(Ex: Entity framework context)

6.Common - all the other commen functionalities releated to whole project (Ex: Enum, Constants, Error loggin, Notification, Etc,etc...)

Architecture of above project implements some of the best practices aligning with coding standers and the dependancy injection.
The way it has been arranges is pretty easy to do the Moking of the entities for Unit test.

Each layer has seperate Test library to make it robust on testing. but this is purly depend on the requirement.
This is flexible to deside whether to have seperate test library or one sigle library to keep all the test classes.

## Test

When writing a unit test to test AccountInfo.RefreshAmount, you'll have to be aware of the requirements of this function; you'll have to know exactly what this function is supposed to do.

Looking at the code it seems that the requirements of RefreshAmount is:

Whatever object that implements IAccountService and whatever Id are used to construct an object of class AccountInfo, the post condition of calling RefreshAmount is that property Amount returns the same value as IAccountService.GetAccountAmount(Id) would have returned.

Because the requirement says it should work for all Ids and all AccountServices, you can't test your class with all of them. In such cases when writing unit tests you'll have to think of errors that are likely to happen.

Most of the coverd tests are reside in EvisionApp.Service.Test.
There are 8 unit tests covering refresh amount method. 7 out of 8 is used mock Mock<IAccountService> and one unit test is coverd using original class.

### Prerequisites

Visual Studio 2017
NUnit
Moq

##Build
I have used New Nuget pakage to Automatically creating NuGet package as part of build process insted of manual build process. please see the reference to find the package.
Pakage Name: CreateNewNuGetPackageFromProjectAfterEachBuild
This will create Nupkg file in BuildScript Folder in each and every build automatically.

## Authors

* **Gayan Pathirana** - *Initial work* - [Tech-Gayan](https://github.com/tech-gayan)

## references

*Creating Nuget Package* -[Creating Nuget Package](https://docs.microsoft.com/en-us/nuget/create-packages/creating-a-package)
*Publish Package* -[Publish Package](https://docs.microsoft.com/en-us/nuget/quickstart/create-and-publish-a-package-using-visual-studio)
*MsBuild Overview* -[MsBuild Overview](https://msdn.microsoft.com/en-us/library/ms171452.aspx)
*Nuget Package* -[Nuget Package](https://www.nuget.org/packages/CreateNewNuGetPackageFromProjectAfterEachBuild/)
