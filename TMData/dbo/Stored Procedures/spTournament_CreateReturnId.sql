CREATE PROCEDURE [dbo].[spTournament_CreateReturnId]
	@TournamentName nvarchar(50),
	@StartDate datetime2,
	@EndDate datetime2,
	@Prizepool money
AS
begin
	set nocount on;

	insert into [dbo].[Tournament] (TournamentName, StartDate, EndDate, Prizepool)
	output INSERTED.[Id]
	values (@TournamentName, @StartDate, @EndDate, @Prizepool);
end