CREATE PROCEDURE [dbo].[spMapScore_GetAll]

AS
begin
	set nocount on;

	select Id, MatchId, MapNumber, MapName, TeamOneScore, TeamTwoScore
	from [dbo].[MapScore]
end
