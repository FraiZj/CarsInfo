INSERT INTO [Role] ([Name])
VALUES 
('User'),
('Admin');

INSERT INTO [User] (FirstName, LastName, Email, [Password], EmailVerified)
VALUES 
('John', 'John', 'john@email.com', '$2a$11$EqFAIQMxzVZfbbjRBLuSk.wN5YQu1b3DsN9mvQkvlhmr6MIsJK/sq', 1),
('Admin', 'Admin', 'admin@email.com', '$2a$11$W8SUYi8ivY4J.OVrlBWBH.LqNQ2rnVzigfV9mgHhaXfpHas1f61zC', 1);

INSERT INTO UserRole (UserId, RoleId)
VALUES 
(1, 1),
(2, 1),
(2, 2);

INSERT INTO Brand ([Name])
VALUES 
(N'BMW'),
(N'Audi'),
(N'Toyota'),
(N'Honda'),
(N'Nissan'),
(N'Lexus');

SET IDENTITY_INSERT [dbo].[Car] ON

INSERT INTO Car (Id, BrandId, Model, [Description])
VALUES
(1, 1, N'X5M', N'BMW X5M'),
(2, 2, N'S7', N'Audi S7'),
(3, 3, N'Camry', N'Toyota Camry'),
(4, 1, N'M3', N'BMW M3'),
(5, 2, N'Q8', N'Audi Q8'),
(6, 4, N'Accord', N'Honda Accord'),
(7, 3, N'Land Cruiser 200', N'Toyota Land Cruiser 200 Khann'),
(8, 1, N'X6M', N'BMW X6M'),
(9, 5, N'GT-R', N'Nissan GT-R'),
(10, 5, N'Skyline', N'Nissan Skyline'),
(11, 1, N'M3 E36', N'BMW M3 E36'),
(12, 6, N'LX', N''),
(13, 6, N'LC 500', N'Lexus LC 500');

SET IDENTITY_INSERT [dbo].[Car] OFF

INSERT [dbo].[CarPicture] ([CarId], [PictureLink]) VALUES 
(3, N'https://upload.wikimedia.org/wikipedia/commons/a/ac/2018_Toyota_Camry_%28ASV70R%29_Ascent_sedan_%282018-08-27%29_01.jpg'),
(4, N'https://upload.wikimedia.org/wikipedia/commons/f/f6/2018_BMW_M3_3.0.jpg'),
(4, N'https://upload.wikimedia.org/wikipedia/commons/7/73/DT121563_%284724925097%29.jpg'),
(5, N'https://upload.wikimedia.org/wikipedia/commons/f/f0/2018_Audi_Q8.jpg'),
(6, N'https://upload.wikimedia.org/wikipedia/commons/2/22/2018_Honda_Accord_front_4.1.18.jpg'),
(7, N'https://www.khann.ru/upload/iblock/4dd/4ddcca2e63a8094a2460f1eb5983c8c0.jpg'),
(8, N'https://upload.wikimedia.org/wikipedia/commons/2/2a/2018_BMW_X6_xDrive30d_M_Sport_Automatic_3.0_Front.jpg'),
(8, N'https://upload.wikimedia.org/wikipedia/commons/7/77/2018_BMW_X6_%28F16%29_xDrive35i_wagon_%282018-10-30%29_01.jpg'),
(8, N'https://upload.wikimedia.org/wikipedia/commons/b/b1/BMW_G06_IMG_3715.jpg'),
(2, N'https://upload.wikimedia.org/wikipedia/commons/a/ae/Audi_s7_4k_2020-1.jpg'),
(1, N'https://hips.hearstapps.com/hmg-prod.s3.amazonaws.com/images/2020-bmw-x5-m-113-1582911123.jpg?crop=0.795xw:0.649xh;0.173xw,0.335xh&resize=640:*'),
(1, N'https://upload.wikimedia.org/wikipedia/commons/f/f1/2019_BMW_X5_M50d_Automatic_3.0.jpg'),
(9, N'https://upload.wikimedia.org/wikipedia/commons/7/7e/Nissan_GT-R_MY2017_%281%29.jpg'),
(10, N'https://upload.wikimedia.org/wikipedia/commons/8/87/Nissan_Skyline_ENR34_1999.jpg'),
(11, N'https://s.auto.drom.ru/i24239/pubs/4483/72436/3110888.jpg'),
(12, N'https://motor.ru/thumb/908x0/filters:quality(75):no_upscale()/imgs/2020/08/13/08/4048871/07badcf6c3270161f60d99756a66ab4b703832fe.jpg'),
(13, N'https://content.presspage.com/uploads/1850/1920_2016-naias-lexus-lc-500-012.jpg?10000');

INSERT INTO Comment (UserId, CarId, [Text], PublishDate)
VALUES 
(1, 1, 'Nice car', CAST(N'2021-02-08T12:32:10.0000000+00:00' AS DateTimeOffset));

INSERT INTO UserCar (UserId, CarId)
VALUES 
(1, 1);