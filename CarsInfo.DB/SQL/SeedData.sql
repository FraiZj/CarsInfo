INSERT INTO Role (Name)
VALUES 
('User'),
('Admin');

INSERT INTO [User] (FirstName, LastName, Email, Password)
VALUES 
('John', 'John', 'john@email.com', '$2a$11$EqFAIQMxzVZfbbjRBLuSk.wN5YQu1b3DsN9mvQkvlhmr6MIsJK/sq'),
('Admin', 'Admin', 'admin@email.com', '$2a$11$W8SUYi8ivY4J.OVrlBWBH.LqNQ2rnVzigfV9mgHhaXfpHas1f61zC');

INSERT INTO UserRole (UserId, RoleId)
VALUES 
(1, 1),
(2, 1),
(2, 2);

INSERT INTO Brand (Name)
VALUES 
('BMW'),
('Audi'),
('Toyota');

INSERT INTO FuelType (Name)
VALUES 
('Petrol'),
('Diesel'),
('Gas'),
('Electro');

INSERT INTO BodyType (Name)
VALUES 
('Coupe'),
('Sedan'),
('SUV'),
('Hatchback'),
('Sports car');

INSERT INTO Country (Name)
VALUES 
('Germany'),
('Japan');

INSERT INTO Gearbox (Name)
VALUES 
('Automatic Transmission (AT)'),
('Manual Transmission (MT)'),
('Automated Manual Transmission (AM)'),
('Continuously Variable Transmission (CVT)');

INSERT INTO Car (BrandId, Model, Description, FuelTypeId, CountryId, BodyTypeId, GearboxId)
VALUES 
(1, 'X5M', 'BMW X5M', 1, 1, 3, 1),
(2, 'S7', 'Audi S7', 2, 1, 2, 1),
(3, 'Camry', 'Toyota Camry', 1, 2, 2, 3);

INSERT INTO CarPicture (CarId, PictureLink)
VALUES 
(1, 'https://www.google.com/url?sa=i&url=http%3A%2F%2Fwww.motorpage.ru%2FBMW%2FX5M%2Flast%2F&psig=AOvVaw15zeIibK1xkoZywqqh07_4&ust=1627999121658000&source=images&cd=vfe&ved=0CAsQjRxqFwoTCMCCw7y_kvICFQAAAAAdAAAAABAD');

INSERT INTO Comment (UserId, CarId, Text, PublishDate)
VALUES 
(1, 1, 'Good car', '02-08-2021 12:32:10');

INSERT INTO UserCar (UserId, CarId)
VALUES 
(1, 1);