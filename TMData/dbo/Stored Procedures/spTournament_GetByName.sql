CREATE PROCEDURE [dbo].[spTournament_GetByName]
	@TournamentName nvarchar(50)
AS
begin
	set nocount on;

	select Id, TournamentName, StartDate, EndDate, Prizepool
	from [dbo].[Tournament]
	where @TournamentName = TournamentName;
end
