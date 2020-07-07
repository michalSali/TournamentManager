CREATE TABLE [dbo].[MapScore]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MatchId] INT NOT NULL, 
    [MapNumber] INT NOT NULL, 
    [MapName] NVARCHAR(50) NOT NULL, 
    [TeamOneScore] INT NOT NULL, 
    [TeamTwoScore] INT NOT NULL
)
