CREATE PROCEDURE [dbo].[spPlayerLookup]
	@Id int
AS
begin
	set nocount on;

	select FirstName, LastName, NickName, Age, Role
	from [dbo].[Player]
	where Id = @Id
end