CREATE PROCEDURE [dbo].[spTournamentEntry_GetByTournamentId]
	@TournamentId int	
AS
begin
	set nocount on;

	select Id, TournamentId, TeamId
	from [dbo].[TournamentEntry]
	where @TournamentId = TournamentId;
end
