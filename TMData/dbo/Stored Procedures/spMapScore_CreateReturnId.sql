CREATE PROCEDURE [dbo].[spMapScore_CreateReturnId]
	@MatchId int,
	@MapNumber int,
	@MapName nvarchar(50),
	@TeamOneScore int,
	@TeamTwoScore int
AS
begin
	set nocount on;

	insert into [dbo].[MapScore] (MatchId, MapNumber, MapName, TeamOneScore, TeamTwoScore)
	output INSERTED.[Id]
	values (@MatchId, @MapNumber, @MapName, @TeamOneScore, @TeamTwoScore);	
	
end
