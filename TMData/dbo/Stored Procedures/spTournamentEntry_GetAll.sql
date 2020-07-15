CREATE PROCEDURE [dbo].[spTournamentEntry_GetAll]

AS
begin
	set nocount on;

	select Id, TournamentId, TeamId
	from [dbo].[TournamentEntry]
end