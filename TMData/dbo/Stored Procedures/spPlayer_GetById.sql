CREATE PROCEDURE [dbo].[spPlayer_GetById]
	@Id int
AS
begin
	set nocount on;

	select Id, FirstName, LastName, NickName, Age, [Role]
	from [dbo].[Player]
	where Id = @Id
end