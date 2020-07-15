CREATE PROCEDURE [dbo].[spMapPlayerStats_GetByMapScoreId]
	@MapScoreId int

AS
begin
	set nocount on;

	select Id, MapScoreId, PlayerId, Kills, Assists, Deaths
	from [dbo].[MapPlayerStats]
	where @MapScoreId = MapScoreId;
end
