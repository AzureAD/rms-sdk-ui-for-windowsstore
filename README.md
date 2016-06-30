## UI Library for Microsoft RMS SDK v4.1 for Windows Store Applications ##
----------

The UI Library for Microsoft RMS SDK v4.1 for Windows Store Applications provides a windows store flyout that implement the UI required for the SDK functionality. This library is optional and a developer may choose to build their own UI when using Microsoft RMS SDK v4.1
## Features
----------
This library provides the following:
 - PermissionFlyout - A Windows Store Appilcation standard that allows viewing the appropriate UserPolicy information consumed on a document according.
 - Localisations - Localized strings for error message

## Community Help and Support
----------
We leverage [Stack Overflow](http://stackoverflow.com/) to work with the community on supporting Azure Active Directory and its SDKs, including this one! We highly recommend you ask your questions on Stack Overflow (we're all on there!) Also browser existing issues to see if someone has had your question before. 

We recommend you use the "adal" tag so we can see it! Here is the latest Q&A on Stack Overflow for ADAL: [http://stackoverflow.com/questions/tagged/adal](http://stackoverflow.com/questions/tagged/adal)

## Security Reporting
----------
If you find a security issue with our libraries or services please report it to [secure@microsoft.com](mailto:secure@microsoft.com) with as much detail as possible. Your submission may be eligible for a bounty through the [Microsoft Bounty](http://aka.ms/bugbounty) program. Please do not post security issues to GitHub Issues or any other public site. We will contact you shortly upon receiving the information. We encourage you to get notifications of when security incidents occur by visiting [this page](https://technet.microsoft.com/en-us/security/dd252948) and subscribing to Security Advisory Alerts.

##Contributing
----------
All code is licensed under MICROSOFT SOFTWARE LICENSE TERMS, MICROSOFT RIGHTS MANAGEMENT SERVICE SDK UI LIBRARIES. We enthusiastically welcome contributions and feedback. You can clone the repository and start contributing now.

##How to use this library
----------
Prerequisites 
 You must have installed the following software 
 - Git for Windows 
 - RMS SDK for Windows Store Applications([SDK download site](http://go.microsoft.com/fwlink/?LinkId=526163)) 
 - ADAL for Windows Store Applications ([ADAL download site](https://www.nuget.org/packages/Microsoft.IdentityModel.Clients.ActiveDirectory)) 
 - Visual studio 12.0 and above with Windows Store Developer SDK 
##Setting up development environment
----------
 - Create a new windows store application, with Microsoft Visual C++ Runtime package for Windows. 
 - Download directly, or via nugget package, the ADAL SDK from [here](https://www.nuget.org/packages/Microsoft.IdentityModel.Clients.ActiveDirectory).
 - Download the RMS SDK v4.1 for Windows Store Applications from [here](http://go.microsoft.com/fwlink/?LinkId=526163)
 - Get the latest UI Library drop: "git clone git@github.com:AzureAD/rms-sdk-ui-for-windowsstore.git"
 - Add references to libraries in your project, (if you used the nugget command to get the ADAL SDK you will need to add [BP2] 

##Sample Usage
----------

 1. Code Integration.
```csharp

				// Adding Reference to Page.
                PermissionsViewer.HostingPage = this;
				// Supplying the user policy.
                PermissionsViewer.Policy = result.Stream.Policy;
                // Setting state to open. (aka visible).
                PermissionsViewer.IsOpen = true;
 ```
 2. XAML integration.
 ```XML
 
	         <uiLibrary:PermissionsFlyout x:Name="PermissionsViewer" Margin="0,0,0,-10" Padding="0" HorizontalAlignment="Left" VerticalContentAlignment="Top" IsAutoDismissEnabled="False"/>
 ```
 
## We Value and Adhere to the Microsoft Open Source Code of Conduct

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
