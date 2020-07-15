CREATE PROCEDURE [dbo].[spMatch_CreateReturnId]
	@TournamentId int,
	@MatchNumber int,
	@Date datetime2,
	@Format int,
	@TeamOneId int,
	@TeamTwoId int,
	@TeamOneScore int,
	@TeamTwoScore int
AS
begin
	set nocount on;

	insert into [dbo].[Match] (TournamentId, MatchNumber, [Date], [Format],
           TeamOneId, TeamTwoId, TeamOneScore, TeamTwoScore)
	output INSERTED.[Id]
	values (@TournamentId, @MatchNumber, @Date, @Format,
           @TeamOneId, @TeamTwoId, @TeamOneScore, @TeamTwoScore);	
	
end

