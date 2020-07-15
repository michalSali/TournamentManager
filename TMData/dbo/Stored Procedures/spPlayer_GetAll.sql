CREATE PROCEDURE [dbo].[spPlayer_GetAll]

AS
begin
	set nocount on;

	select Id, FirstName, LastName, NickName, Age, [Role]
	from [dbo].[Player]
end
