SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectCarById] @Id INT, @IncludeDeleted BIT = 0
AS
	IF (@IncludeDeleted = 1)
	BEGIN
		SELECT * FROM [Car]
		LEFT JOIN Brand
		ON [Car].BrandId = [Brand].Id
		LEFT JOIN [CarPicture]
		ON [Car].Id = [CarPicture].CarId
		WHERE Car.Id=@Id
	END
	ELSE
	BEGIN
		SELECT * FROM [Car]
		LEFT JOIN Brand
		ON [Car].BrandId = [Brand].Id
		LEFT JOIN [CarPicture]
		ON [Car].Id = [CarPicture].CarId
		WHERE [Car].Id=@Id AND [Car].IsDeleted = 0
	END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SelectUserByEmail] @Email NVARCHAR(50)
AS (
	SELECT * FROM [User]
    INNER JOIN UserRole 
    ON [User].Id = UserRole.UserId
    INNER JOIN [Role]
    ON UserRole.RoleId = [Role].Id
    WHERE [User].Email = @Email
);