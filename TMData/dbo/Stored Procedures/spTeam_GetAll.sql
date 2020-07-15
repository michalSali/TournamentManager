CREATE PROCEDURE [dbo].[spTeam_GetAll]

AS
begin
	set nocount on;

	select Id, TeamName, CoachName
	from [dbo].[Team]
end