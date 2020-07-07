CREATE TABLE [dbo].[MapPlayerStats]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [MapScoreId] INT NOT NULL, 
    [PlayerId] INT NOT NULL, 
    [Kills] INT NULL DEFAULT 0, 
    [Assists] INT NULL DEFAULT 0, 
    [Deaths] INT NULL DEFAULT 0
)
