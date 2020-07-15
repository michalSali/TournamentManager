CREATE PROCEDURE [dbo].[spTeamMember_GetByTeamId]
	@TeamId int

AS
begin
	set nocount on;

	select Id, TeamId, PlayerId
	from [dbo].[TeamMember]
	where @TeamId = TeamId;
end
