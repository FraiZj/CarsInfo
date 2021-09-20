CREATE TABLE Brand (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	[Name] nvarchar(50) UNIQUE NOT NULL
)

CREATE TABLE [User] (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	FirstName nvarchar(50) NOT NULL,
	LastName nvarchar(50),
	Email nvarchar(50) NOT NULL UNIQUE,
	[Password] nvarchar(500),
	IsExternal bit DEFAULT 0
)

CREATE TABLE UserRefreshToken (
    Id int PRIMARY KEY IDENTITY(1,1),
    IsDeleted bit DEFAULT 0,
    UserId int NOT NULL,
    Token nvarchar(max),
    ExpiryTime DATETIMEOFFSET,
    CONSTRAINT FK_UserRefreshToken_User FOREIGN KEY (UserId) REFERENCES [User](Id),
)

CREATE TABLE Car (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	BrandId int NOT NULL,
	Model nvarchar(50) NOT NULL,
	[Description] nvarchar(150) NOT NULL,
	CONSTRAINT FK_Car_Brand FOREIGN KEY (BrandId) REFERENCES Brand(Id),
	CONSTRAINT UQ_Car_BrandId_Model UNIQUE (BrandId, Model)
)

CREATE TABLE CarPicture (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	CarId int NOT NULL,
	PictureLink nvarchar(500) UNIQUE NOT NULL,
	CONSTRAINT FK_CarPicture_Car FOREIGN KEY (CarId) REFERENCES Car(Id)
)

CREATE TABLE Comment (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	UserId int NOT NULL,
	CarId int NOT NULL,
	[Text] nvarchar(150) NOT NULL,
	PublishDate DATETIMEOFFSET NOT NULL DEFAULT GETDATE(),
	CONSTRAINT FK_Comment_User FOREIGN KEY (UserId) REFERENCES [User](Id),
	CONSTRAINT FK_Comment_Car FOREIGN KEY (CarId) REFERENCES Car(Id)
)

CREATE TABLE UserCar (
	Id int UNIQUE IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	UserId int,
	CarId int,
	CONSTRAINT PK_UserCar PRIMARY KEY (UserId, CarId),
	CONSTRAINT FK_UserCar_User FOREIGN KEY (UserId) REFERENCES [User](Id), 
	CONSTRAINT FK_UserCar_Car FOREIGN KEY (CarId) REFERENCES [Car](Id), 
)

CREATE TABLE [Role] (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	[Name] nvarchar(50) UNIQUE NOT NULL
)

CREATE TABLE UserRole (
	Id int UNIQUE IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	UserId int,
	RoleId int,
	CONSTRAINT PK_UserRole PRIMARY KEY (UserId, RoleId),
	CONSTRAINT FK_UserRole_User FOREIGN KEY (UserId) REFERENCES [User](Id), 
	CONSTRAINT FK_UserRole_Role FOREIGN KEY (RoleId) REFERENCES Role(Id), 
)