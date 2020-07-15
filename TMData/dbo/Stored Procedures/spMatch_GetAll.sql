CREATE PROCEDURE [dbo].[spMatch_GetAll]

AS
begin
	set nocount on;

	select Id, TournamentId, MatchNumber, [Date], [Format],
           TeamOneId, TeamTwoId, TeamOneScore, TeamTwoScore
	from [dbo].[Match]
end
