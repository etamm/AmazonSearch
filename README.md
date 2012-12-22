## Amazon Product Search

Small web app that allows to search products on amazon using the Amazon Product Advertising API.

# Instructions
Programmed in Visual Studio 2012 Express for the WEB - asp.net MVC 3.

1. Enable "NuGet Package Restore" to get missing packages
2. Insert your amazon acces_key, secret_key, associate_tag in Globals.cs. Currently empty strings.
3. Currently it is setup to use amazon.co.uk. If you want to use another site add a new locale description in LocaleData and
change CURRENT_LOCALE to your locale.