CREATE PROCEDURE [dbo].[spMatch_GetByTournamentId]
	@TournamentId int
AS
begin
	set nocount on;

	select Id, TournamentId, MatchNumber, [Date], [Format],
           TeamOneId, TeamTwoId, TeamOneScore, TeamTwoScore
	from [dbo].[Match]
	where TournamentId = @TournamentId
end
