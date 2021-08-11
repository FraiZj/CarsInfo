USE CarsInfo

INSERT INTO [Role] ([Name])
VALUES 
('User'),
('Admin');

INSERT INTO [User] (FirstName, LastName, Email, [Password])
VALUES 
('John', 'John', 'john@email.com', '$2a$11$EqFAIQMxzVZfbbjRBLuSk.wN5YQu1b3DsN9mvQkvlhmr6MIsJK/sq'),
('Admin', 'Admin', 'admin@email.com', '$2a$11$W8SUYi8ivY4J.OVrlBWBH.LqNQ2rnVzigfV9mgHhaXfpHas1f61zC');

INSERT INTO UserRole (UserId, RoleId)
VALUES 
(1, 1),
(2, 1),
(2, 2);

INSERT INTO Brand ([Name])
VALUES 
('BMW'),
('Audi'),
('Toyota');

INSERT INTO Car (BrandId, Model, [Description])
VALUES
(1, 'X5M', 'BMW X5M'),
(2, 'S7', 'Audi S7'),
(3, 'Camry', 'Toyota Camry');

INSERT INTO CarPicture (CarId, PictureLink)
VALUES
(1, 'https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/2020-bmw-x5-m-113-1582911123.jpg?crop=0.795xw:0.649xh;0.173xw,0.335xh&resize=640:*'),
(2, 'https://upload.wikimedia.org/wikipedia/commons/4/43/Audi_S7_Sportback_4.0_TFSI_quattro_S_tronic.JPG'),
(3, 'https://upload.wikimedia.org/wikipedia/commons/a/ac/2018_Toyota_Camry_%28ASV70R%29_Ascent_sedan_%282018-08-27%29_01.jpg');

INSERT INTO Comment (UserId, CarId, [Text], PublishDate)
VALUES 
(1, 1, 'Nice car', '02-08-2021 12:32:10');

INSERT INTO UserCar (UserId, CarId)
VALUES 
(1, 1);