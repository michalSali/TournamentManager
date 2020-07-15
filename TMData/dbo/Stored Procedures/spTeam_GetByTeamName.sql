CREATE PROCEDURE [dbo].[spTeam_GetByTeamName]
	@TeamName nvarchar(50)
AS
begin
	set nocount on;

	select Id, TeamName, CoachName
	from [dbo].[Team]
	where @TeamName = TeamName;
end
