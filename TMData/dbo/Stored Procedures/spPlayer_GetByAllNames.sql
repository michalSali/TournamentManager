CREATE PROCEDURE [dbo].[spPlayer_GetByAllNames]
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@NickName nvarchar(50)
AS
begin
	set nocount on;

	select FirstName, LastName, NickName, Age, Role
	from [dbo].[Player]
	where @FirstName = FirstName and @LastName = LastName and @NickName = NickName;
end
