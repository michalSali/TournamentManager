CREATE PROCEDURE [dbo].[spTournamentEntry_Create]
	@TournamentId int,
	@TeamId int
AS
begin
	set nocount on;

	insert into [dbo].[TournamentEntry] (TournamentId, TeamId)	
	values (@TournamentId, @TeamId);	
	
end
