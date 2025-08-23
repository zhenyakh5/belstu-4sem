-- �������� ���� ������
IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'HotelManagement')
BEGIN
    CREATE DATABASE HotelManagement;
END
GO

USE HotelManagement;
GO

-- ������� �������������
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(255) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    IsAdmin BIT NOT NULL DEFAULT 0,
    RegistrationDate DATETIME NOT NULL DEFAULT GETDATE()
);

-- ������� ��������� �������
CREATE TABLE RoomCategories (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500) NOT NULL,
    BasePrice FLOAT NOT NULL,
    Image VARBINARY(MAX) NULL
);

-- ������� ����������� �������
CREATE TABLE HotelRooms (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoomNumber NVARCHAR(10) NOT NULL UNIQUE,
    CategoryId INT NOT NULL,
    Floor INT NOT NULL,
    Capacity INT NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Amenities NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Available',
    Image VARBINARY(MAX) NULL,
    FOREIGN KEY (CategoryId) REFERENCES RoomCategories(Id)
);

-- ������� ������������
CREATE TABLE Bookings (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoomId INT NOT NULL,
    UserId INT NOT NULL,
    CheckInDate DATE NOT NULL,
    CheckOutDate DATE NOT NULL,
    BookingDate DATETIME NOT NULL DEFAULT GETDATE(),
    TotalPrice FLOAT NOT NULL,
    Status NVARCHAR(20) NOT NULL DEFAULT 'Confirmed',
    Notes NVARCHAR(MAX) NULL,
    FOREIGN KEY (RoomId) REFERENCES HotelRooms(Id),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- ������� �������
CREATE TABLE Reviews (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    BookingId INT NOT NULL,
    Rating INT NOT NULL CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(MAX) NULL,
    ReviewDate DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (BookingId) REFERENCES Bookings(Id)
);

-- ������� ��� ���������� ������� ������ ��� ������������
CREATE TRIGGER tr_UpdateRoomStatusOnBooking
ON Bookings
AFTER INSERT
AS
BEGIN
    UPDATE HotelRooms
    SET Status = 'Booked'
    WHERE Id IN (SELECT RoomId FROM inserted WHERE Status = 'Confirmed');
END;
GO

-- �������� ��������� ��� ������ ��������� �������
CREATE PROCEDURE sp_FindAvailableRooms
    @CheckInDate DATE,
    @CheckOutDate DATE,
    @CategoryId INT = NULL,
    @Capacity INT = NULL
AS
BEGIN
    SELECT r.*, c.Name AS CategoryName, c.BasePrice
    FROM HotelRooms r
    JOIN RoomCategories c ON r.CategoryId = c.Id
    WHERE r.Status = 'Available'
    AND r.Id NOT IN (
        SELECT RoomId FROM Bookings 
        WHERE NOT (CheckOutDate <= @CheckInDate OR CheckInDate >= @CheckOutDate)
        AND Status <> 'Cancelled'
    )
    AND (@CategoryId IS NULL OR r.CategoryId = @CategoryId)
    AND (@Capacity IS NULL OR r.Capacity >= @Capacity)
    ORDER BY c.BasePrice;
END;
GO

-- �������� ��������� ��� ������� ��������� ������������
CREATE PROCEDURE sp_CalculateBookingPrice
    @RoomId INT,
    @CheckInDate DATE,
    @CheckOutDate DATE
AS
BEGIN
    DECLARE @Days INT = DATEDIFF(DAY, @CheckInDate, @CheckOutDate);
    DECLARE @BasePrice FLOAT;
    
    SELECT @BasePrice = c.BasePrice
    FROM HotelRooms r
    JOIN RoomCategories c ON r.CategoryId = c.Id
    WHERE r.Id = @RoomId;
    
    SELECT @Days * @BasePrice AS TotalPrice;
END;
GO

-- ������� ��������� ������
INSERT INTO Users (Username, Password, FullName, Email, Phone, IsAdmin)
VALUES 
('admin', 'admin123', '�������������', 'admin@hotel.com', '+1234567890', 1),
('user1', 'user1123', '���� ������', 'user1@mail.com', '+7987654321', 0),
('user2', 'user2123', '���� ������', 'user2@mail.com', '+7912345678', 0);

INSERT INTO RoomCategories (Name, Description, BasePrice)
VALUES
('��������', '��������������� ����� � �������� ����������', 100.00),
('������', '���������� ����� � ���������� �������� � �������', 150.00),
('����', '��������� ����� � ��������������� ��������', 250.00),
('�������������', '����� ������� � ��������������� ����� � �����', 500.00);

-- ���������� ����������� � RoomCategories (����������� �������� � ����������)