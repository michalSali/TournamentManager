CREATE PROCEDURE [dbo].[spTeam_GetById]
	@Id int

AS
begin
	set nocount on;

	select Id, TeamName, CoachName
	from [dbo].[Team]
	where @Id = Id;
end
