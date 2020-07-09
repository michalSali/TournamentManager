CREATE TABLE [dbo].[Player]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,  
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL,
    [NickName] NVARCHAR(50) NOT NULL,
    [Age] INT NULL DEFAULT 0, 
    [Role] NVARCHAR(50) NULL DEFAULT 'Not specified'
)
