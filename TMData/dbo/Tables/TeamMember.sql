CREATE TABLE [dbo].[TeamMember]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TeamId] INT NOT NULL, 
    [PlayerId] INT NOT NULL
)
