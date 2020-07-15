CREATE PROCEDURE [dbo].[spTeamMember_GetAll]

AS
begin
	set nocount on;

	select Id, TeamId, PlayerId
	from [dbo].[TeamMember]
end
