# deertier

This repository contains the code and DB schema/data for deertier.com.

## Website

The website is written in **C# (.NET 4.6)** with **ASP.NET MVC 5** and using Visual Studio 2015.

## Database

The database used on deertier.com is **SQL Server 2008** but other flavours of SQL would probably work as well. The scripts in this repo 
assume a schema name of [deertier] but this is not required.

If you'd like to work with the database please use the scripts in the db folder to set up your own instance and then enter the connection
info in **src\DeerTier.Web\Config\ConnectionStrings.config**.
