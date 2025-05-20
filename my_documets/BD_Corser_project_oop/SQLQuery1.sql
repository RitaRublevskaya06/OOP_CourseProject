USE Corser_project;

SELECT * FROM [Procedures];

SELECT * FROM Doctor;
SELECT * FROM Prescriptions;
SELECT * FROM [Procedures];
--SELECT * FROM Doctor;
SELECT * FROM Clients;
SELECT * FROM [Order];


------ Проверка наличия врача с ID=3
----SELECT * FROM Doctor WHERE ID_Doctor = 3;

------ Проверка рецептов для клиента с ID=2
----SELECT * FROM Prescriptions WHERE ClientID = 2;

--CREATE TABLE Clients (
--    ID_Client INT PRIMARY KEY IDENTITY(1,1),
--    Name_Client NVARCHAR(MAX) NOT NULL,
--    Surname_Client NVARCHAR(MAX) NOT NULL,
--    Email NVARCHAR(MAX) NOT NULL,
--    Password NVARCHAR(MAX) NOT NULL,
--    Login NVARCHAR(MAX) NOT NULL,
--    Image VARBINARY(MAX) NULL
--);

--CREATE TABLE Reviews (
--    ID_Review INT PRIMARY KEY IDENTITY(1,1),
--    ID_Client INT,
--    Heading NVARCHAR(MAX),
--    Review NVARCHAR(MAX),
--    FOREIGN KEY (ID_Client) REFERENCES Clients(ID_Client)
--);

--CREATE TABLE Spezialization (
--    Spezialization NVARCHAR(200) PRIMARY KEY
--);

--CREATE TABLE Doctor (
--    ID_Doctor INT PRIMARY KEY IDENTITY(1,1),
--    Name_Doctor NVARCHAR(MAX),
--    Surname_Doctor NVARCHAR(MAX),
--    Middle_name_Doctor NVARCHAR(MAX),
--    Spezialization NVARCHAR(200),
--    Study NVARCHAR(MAX),
--    Work_experience SMALLINT,
--    Photo_Doctor VARBINARY(MAX),
--    FOREIGN KEY (Spezialization) REFERENCES Spezialization(Spezialization)
--);


--CREATE TABLE schedule (
--    ID_doctor INT NULL,
--    Mondfrom TIME NULL,
--    Mondto TIME NULL,
--    Tuefrom TIME NULL,
--    Tueto TIME NULL,
--    Wenfrom TIME NULL,
--    Wento TIME NULL,
--    Thufrom TIME NULL,
--    Thuto TIME NULL,
--    Frifrom TIME NULL,
--    Frito TIME NULL,
--    FOREIGN KEY (ID_doctor) REFERENCES Doctor(ID_Doctor)
--);

----DROP TABLE Coupon;

--CREATE TABLE [Procedures] (
--    ID_Procedures INT PRIMARY KEY IDENTITY(1,1),
--    Name_Procedures NVARCHAR(MAX) NOT NULL,
--    Decription NVARCHAR(MAX) NOT NULL,
--    Price FLOAT NOT NULL,
--    Photo VARBINARY(MAX) NULL,
--    Spezialization NVARCHAR(200) NULL,
--    FOREIGN KEY (Spezialization) REFERENCES Spezialization(Spezialization)
--);

--CREATE TABLE Coupon (
--    ID_Coupon INT PRIMARY KEY IDENTITY(1,1),
--    Date DATE NOT NULL,
--    time NVARCHAR(8) NOT NULL,
--    ID_Doctor INT NOT NULL,
--    Ordered NCHAR(10) NOT NULL,
--    FOREIGN KEY (ID_Doctor) REFERENCES Doctor(ID_Doctor)
--);

--CREATE TABLE [Order] (
--    Order_number SMALLINT PRIMARY KEY IDENTITY(1,1),
--    Date DATE,
--    ID_Procedures INT,
--    ID_Client INT,
--    ID_Doctor INT,
--    ID_Coupon INT,
--    Time TIME(7),
--    FOREIGN KEY (ID_Procedures) REFERENCES [Procedures](ID_Procedures),
--    FOREIGN KEY (ID_Client) REFERENCES Clients(ID_Client),
--    FOREIGN KEY (ID_Doctor) REFERENCES Doctor(ID_Doctor),
--    FOREIGN KEY (ID_Coupon) REFERENCES Coupon(ID_Coupon)
--);

--CREATE TABLE Prescriptions (
--    PrescriptionID INT PRIMARY KEY IDENTITY(1,1),
--    DoctorID INT,
--    ClientID INT,
--    Text NVARCHAR(MAX),
--    Date DATE DEFAULT GETDATE(),
--    FOREIGN KEY (DoctorID) REFERENCES Doctor(ID_Doctor),
--    FOREIGN KEY (ClientID) REFERENCES Clients(ID_Client)
--);

--ALTER TABLE Doctor 
--ADD 
--    Login NVARCHAR(50) NOT NULL DEFAULT 'temp_login',
--    Password NVARCHAR(50) NOT NULL DEFAULT 'temp_password';

--	ALTER TABLE Doctor
--DROP COLUMN Login, Password;

--EXEC sp_rename 'Doctor.Login', 'DoctorLogin', 'COLUMN';
--EXEC sp_rename 'Doctor.Password', 'DoctorPassword', 'COLUMN';



--	UPDATE Doctor
--SET 
--    Login = 'doctor_' + CAST(ID_Doctor AS NVARCHAR(50)), 
--    Password = 'password_' + CAST(ID_Doctor AS NVARCHAR(50))
--WHERE Login = 'temp_login'; -- Обновляем только временные значения

--ALTER TABLE Doctor 
--ALTER COLUMN Login NVARCHAR(50) NOT NULL;

--ALTER TABLE Doctor 
--ALTER COLUMN Password NVARCHAR(50) NOT NULL;

--ALTER TABLE Doctor
--ADD CONSTRAINT UQ_Doctor_Login UNIQUE (Login);

ALTER TABLE [Order] 
ADD Status NVARCHAR(50) DEFAULT 'Pending' CHECK (Status IN ('Pending', 'Completed', 'Canceled'));


--ALTER TABLE Coupon 
--ALTER COLUMN [time] TIME NOT NULL;





------ Отключаем проверки ограничений
----ALTER TABLE [Order] NOCHECK CONSTRAINT ALL;

------ Удаляем все строки из таблицы Coupon
----DELETE FROM Coupon;

------ Включаем проверки ограничений обратно
----ALTER TABLE [Order] CHECK CONSTRAINT ALL;


----EXEC sp_rename 'Review', 'Reviews';


--------SELECT * FROM Coupon WHERE ID_Coupon = 1271;
--------SELECT * FROM [Order] WHERE ID_Coupon = 1271;



--------SELECT * FROM Coupon;

--SELECT OBJECT_DEFINITION(OBJECT_ID('CK__Order__Status__52593CB8')) AS CheckConstraint;


--SELECT 
--  [Order].Order_number, 
--  [Order].Date, 
--  [Order].[Time], 
--  [Order].ID_Coupon, 
--  [Procedures].Name_Procedures, 
--  Doctor.Name_Doctor, 
--  Doctor.Middle_name_Doctor,
--  Doctor.ID_Doctor,
--  [Procedures].Price, 
--  Clients.Name_Client, 
--  Clients.Surname_Client,
--  Clients.ID_Client
--FROM [Order] 
--INNER JOIN [Procedures] ON [Order].ID_Procedures = [Procedures].ID_Procedures 
--INNER JOIN Doctor ON [Order].ID_Doctor = Doctor.ID_Doctor 
--INNER JOIN Clients ON Clients.ID_Client = [Order].ID_Client 
--WHERE [Order].Date > CAST(GETDATE() AS DATE)



--------UPDATE Clients
--------SET Password = HASHBYTES('SHA2_256', CAST(Password AS NVARCHAR(100)))
--------WHERE ISNUMERIC(Password) = 0 -- фильтр можно настроить по-своему

