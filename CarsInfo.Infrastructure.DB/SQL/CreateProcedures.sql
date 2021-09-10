CREATE PROCEDURE [SelectCarById] @Id INT, @IncludeDeleted BIT = 0
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