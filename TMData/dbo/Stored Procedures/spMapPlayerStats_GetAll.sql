CREATE PROCEDURE [dbo].[spMapPlayerStats_GetAll]
	
AS
begin
	set nocount on;

	select Id, MapScoreId, PlayerId, Kills, Assists, Deaths
	from [dbo].[MapPlayerStats]
end