CREATE PROCEDURE [dbo].[spTeamMember_Create]
	@TeamId int,
	@PlayerId int
AS
begin
	set nocount on;

	insert into [dbo].[TeamMember] (TeamId, PlayerId)	
	values (@TeamId, @PlayerId);	
	
end
