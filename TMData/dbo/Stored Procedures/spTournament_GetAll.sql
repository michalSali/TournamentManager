CREATE PROCEDURE [dbo].[spTournament_GetAll]

AS
begin
	set nocount on;

	select Id, TournamentName, StartDate, EndDate, Prizepool
	from [dbo].[Tournament]
end