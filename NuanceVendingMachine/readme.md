Assumptions:

- Assuming that machine is already having enough change as requirements don't ask to maintain machine's coins
- Authentication and Authorization are not in the scope
- Sales coins are not being maintained 
- Assuming, if item is not in stock then it should not be visible to users
- Didn't work much on look and feel of the application but it should be working functional properly
- Added both bonus points
- Didn't write few unit test cases (already written 18) but used Moq and xunit framework to showcase the procedure to write unit test case
- Assuming, exception handling is not in the scope hence you might see very few exception handling 



How to run the application:
- Open solution in visual studio 
- Open Server Explorer from View visual studio menu option 
- Make sure Database is showing connected
- Right click on Database  --> Properties --> copy connection string and paste value in ConnectionString of appsettings.Development.json and appsettings.json

- Click to run the project

- For admin ui, please follow the below link:
http://localhost:64313/admin

Please note * First time application load will take little time as it has to start connection with SignalR


In case, if you are facing issues with Database connectivity or any unexpected error then please run below script to create table and data 
and change to connection string to point new database.

CREATE TABLE [dbo].[Producttype] (
    [ProductTypeId] INT          NOT NULL,
    [TypeName]      VARCHAR (50) NOT NULL,
    [Price]         INT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductTypeId] ASC)
);
CREATE TABLE [dbo].[Product] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [Title]         VARCHAR (50) NULL,
    [Stock]         INT          DEFAULT ((0)) NOT NULL,
    [ProductTypeId] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Product_ToTable] FOREIGN KEY ([ProductTypeId]) REFERENCES [dbo].[Producttype] ([ProductTypeId])
);





INSERT INTO [dbo].[Producttype] ([ProductTypeId], [TypeName], [Price]) VALUES (1, N'Drink', 100)
INSERT INTO [dbo].[Producttype] ([ProductTypeId], [TypeName], [Price]) VALUES (2, N'Chips', 220)
INSERT INTO [dbo].[Producttype] ([ProductTypeId], [TypeName], [Price]) VALUES (3, N'Chocolate', 150)
INSERT INTO [dbo].[Producttype] ([ProductTypeId], [TypeName], [Price]) VALUES (4, N'Biscuit', 110)


SET IDENTITY_INSERT [dbo].[Product] ON
INSERT INTO [dbo].[Product] ([Id], [Title], [Stock], [ProductTypeId]) VALUES (3, N'Lays', 100, 2)
INSERT INTO [dbo].[Product] ([Id], [Title], [Stock], [ProductTypeId]) VALUES (4, N'Coca-Cola', 11, 1)
INSERT INTO [dbo].[Product] ([Id], [Title], [Stock], [ProductTypeId]) VALUES (5, N'Pepsi', 12, 1)
INSERT INTO [dbo].[Product] ([Id], [Title], [Stock], [ProductTypeId]) VALUES (7, N'Water', 11, 1)
INSERT INTO [dbo].[Product] ([Id], [Title], [Stock], [ProductTypeId]) VALUES (8, N'Kit-Kat', 1, 3)
SET IDENTITY_INSERT [dbo].[Product] OFF
