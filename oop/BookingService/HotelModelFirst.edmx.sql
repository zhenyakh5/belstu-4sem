
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/15/2025 03:02:38
-- Generated from EDMX file: D:\4sem\oop\BookingService\HotelModelFirst.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [master];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[HotelRooms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HotelRooms];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Username] nvarchar(50)  NOT NULL,
    [Password] nvarchar(100)  NOT NULL,
    [IsAdmin] bit  NOT NULL
);
GO

-- Creating table 'HotelRooms'
CREATE TABLE [dbo].[HotelRooms] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [ShortDescription] nvarchar(200)  NULL,
    [FullDescription] nvarchar(4000)  NULL,
    [ImagePath] nvarchar(255)  NULL,
    [Category] nvarchar(50)  NULL,
    [Rating] float  NULL,
    [Price] float  NULL,
    [NumberOfBeds] int  NULL,
    [IsAvailable] bit  NOT NULL,
    [Amenities] nvarchar(4000)  NULL,
    [Stars] int  NULL,
    [HasBalcony] bit  NOT NULL,
    [IsNonSmoking] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HotelRooms'
ALTER TABLE [dbo].[HotelRooms]
ADD CONSTRAINT [PK_HotelRooms]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------