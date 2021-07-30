USE CarsInfo

CREATE TABLE Brand (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	Name nvarchar(50) UNIQUE NOT NULL
)

CREATE TABLE FuelType (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	Name nvarchar(50) UNIQUE NOT NULL
)

CREATE TABLE BodyType (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	Name nvarchar(50) UNIQUE NOT NULL
)

CREATE TABLE GearBox (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	Name nvarchar(50) UNIQUE NOT NULL
)

CREATE TABLE Country (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	Name nvarchar(50) UNIQUE NOT NULL
)

CREATE TABLE [User] (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	FirstName nvarchar(50) NOT NULL,
	LastName nvarchar(50) NOT NULL,
	Email nvarchar(50) NOT NULL UNIQUE,
	Password nvarchar(500) NOT NULL
)

CREATE TABLE Comment (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	UserId int NOT NULL,
	Text nvarchar(150) NOT NULL,
	PublishDate DATETIMEOFFSET NOT NULL DEFAULT GETDATE(),
	CONSTRAINT FK_Comment_User FOREIGN KEY (UserId) REFERENCES [User](Id)
)

CREATE TABLE Car (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	BrandId int NOT NULL,
	Model nvarchar(50) NOT NULL,
	Description nvarchar(150) NOT NULL,
	FuelTypeId int NOT NULL,
	CountryId int NOT NULL,
	GearboxId int NOT NULL,
	BodyTypeId int NOT NULL,
	CONSTRAINT FK_Car_Brand FOREIGN KEY (BrandId) REFERENCES Brand(Id), 
	CONSTRAINT FK_Car_FuelType FOREIGN KEY (FuelTypeId) REFERENCES FuelType(Id), 
	CONSTRAINT FK_Car_BodyType FOREIGN KEY (BodyTypeId) REFERENCES BodyType(Id), 
	CONSTRAINT FK_Car_Gearbox FOREIGN KEY (GearboxId) REFERENCES Gearbox(Id), 
	CONSTRAINT FK_Car_Country FOREIGN KEY (CountryId) REFERENCES Country(Id), 
)

CREATE TABLE UserCar (
	UserId int,
	CarId int,
	CONSTRAINT PK_UserCar PRIMARY KEY (UserId, CarId),
	CONSTRAINT FK_UserCar_User FOREIGN KEY (UserId) REFERENCES [User](Id), 
	CONSTRAINT FK_UserCar_Car FOREIGN KEY (CarId) REFERENCES [Car](Id), 
)

CREATE TABLE Role (
	Id int PRIMARY KEY IDENTITY(1,1),
	IsDeleted bit DEFAULT 0,
	Name nvarchar(50) UNIQUE NOT NULL
)

CREATE TABLE UserRole (
	UserId int,
	RoleId int,
	CONSTRAINT PK_UserRole PRIMARY KEY (UserId, RoleId),
	CONSTRAINT FK_UserRole_User FOREIGN KEY (UserId) REFERENCES [User](Id), 
	CONSTRAINT FK_UserRole_Role FOREIGN KEY (RoleId) REFERENCES Role(Id), 
)