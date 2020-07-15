CREATE PROCEDURE [dbo].[spMapScore_GetByMatchId]
	@MatchId int
AS
begin
	set nocount on;

	select Id, MatchId, MapNumber, MapName, TeamOneScore, TeamTwoScore
	from [dbo].[MapScore]
	where @MatchId = MatchId;
end
