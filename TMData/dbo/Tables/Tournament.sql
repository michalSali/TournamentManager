CREATE TABLE [dbo].[Tournament]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [StartDate] DATETIME2(0) NOT NULL DEFAULT getutcdate(), 
    [EndDate] DATETIME2(0) NOT NULL DEFAULT getutcdate(), 
    [Prizepool] MONEY NOT NULL
)
