CREATE TABLE [dbo].[Match]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TournamentId] INT NOT NULL, 
    [MatchNumber] INT NOT NULL, 
    [Date] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [Format] INT NOT NULL, 
    [TeamOneId] INT NOT NULL, 
    [TeamTwoId] INT NOT NULL, 
    [TeamOneScore] INT NOT NULL, 
    [TeamTwoScore] INT NOT NULL
)
