INSERT INTO Role (Name)
VALUES 
('User'),
('Admin');

INSERT INTO UselRole (UserId, RoleId)
VALUES 
('John', 'John', 'john@email.com', '$2a$11$EqFAIQMxzVZfbbjRBLuSk.wN5YQu1b3DsN9mvQkvlhmr6MIsJK/sq'),
('Admin', 'Admin', 'admin@email.com', '$2a$11$W8SUYi8ivY4J.OVrlBWBH.LqNQ2rnVzigfV9mgHhaXfpHas1f61zC');

INSERT INTO [User] (Id, FirstName, LastName, Email, Password)
VALUES 
(1, 'John', 'John', 'john@email.com', '$2a$11$EqFAIQMxzVZfbbjRBLuSk.wN5YQu1b3DsN9mvQkvlhmr6MIsJK/sq'),
('Admin', 'Admin', 'admin@email.com', '$2a$11$W8SUYi8ivY4J.OVrlBWBH.LqNQ2rnVzigfV9mgHhaXfpHas1f61zC');
