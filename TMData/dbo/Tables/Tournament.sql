CREATE TABLE [dbo].[Tournament]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TournamentName] NVARCHAR(50) NOT NULL, 
    [StartDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [EndDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [Prizepool] MONEY NOT NULL
)
