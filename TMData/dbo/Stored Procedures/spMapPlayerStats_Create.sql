CREATE PROCEDURE [dbo].[spMapPlayerStats_Create]
	@MapScoreId int,
	@PlayerId int,
	@Kills int,
	@Assists int,
	@Deaths int
AS
begin
	set nocount on;

	insert into [dbo].[MapPlayerStats] (MapScoreId, PlayerId, Kills, Assists, Deaths)
	values (@MapScoreId, @PlayerId, @Kills, @Assists, @Deaths);	
end
