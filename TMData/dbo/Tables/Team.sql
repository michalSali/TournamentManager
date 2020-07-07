CREATE TABLE [dbo].[Team]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TeamName] NVARCHAR(50) NOT NULL, 
    [CoachName] NVARCHAR(50) NULL DEFAULT 'Not specified', 
)
