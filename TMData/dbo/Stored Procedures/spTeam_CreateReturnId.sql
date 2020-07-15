CREATE PROCEDURE [dbo].[spTeam_CreateReturnId]
	@TeamName nvarchar(50),
	@CoachName nvarchar(50)
AS
begin
	set nocount on;

	insert into [dbo].[Team] (TeamName, CoachName)
	output INSERTED.[Id]
	values (@TeamName, @CoachName);	
	
end
