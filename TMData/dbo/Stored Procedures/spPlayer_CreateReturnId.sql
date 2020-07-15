CREATE PROCEDURE [dbo].[spPlayer_CreateReturnId]
	@FirstName nvarchar(50),
	@LastName nvarchar(50),
	@NickName nvarchar(50),
	@Age int,
	@Role nvarchar(50)
AS
begin
	set nocount on;

	insert into [dbo].[Player] (FirstName, LastName, NickName, Age, [Role])
	output INSERTED.[Id]
	values (@FirstName, @LastName, @NickName, @Age, @Role);	
	
end
