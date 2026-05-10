-- ============================================================
-- Script de criacao da base de dados WebAPIVehicles
-- ============================================================
-- Como usar:
-- 1. Abrir este ficheiro no SQL Server Management Studio (SSMS)
-- 2. Ligar ao servidor SQL Server local
-- 3. Executar (F5)
-- ============================================================

USE master;
GO

-- Apagar a BD se ja existir (cuidado: apaga tudo)
IF DB_ID('VehiclesDB') IS NOT NULL
BEGIN
    ALTER DATABASE VehiclesDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE VehiclesDB;
END
GO

-- Criar a base de dados
CREATE DATABASE VehiclesDB;
GO

USE VehiclesDB;
GO

-- Criar a tabela
CREATE TABLE Vehicles (
    VehicleID       INT IDENTITY(1,1) PRIMARY KEY,
    Brand           NVARCHAR(50)  NOT NULL,
    Model           NVARCHAR(50)  NOT NULL,
    Year            INT           NOT NULL,
    LastInspection  DATETIME      NULL,
    Sold            BIT           NOT NULL DEFAULT 0
);
GO

-- Inserir dados de teste
INSERT INTO Vehicles (Brand, Model, Year, LastInspection, Sold) VALUES
    ('Toyota',     'Corolla',  2020, '2025-03-10', 0),
    ('Honda',      'Civic',    2019, '2024-11-22', 0),
    ('Ford',       'Focus',    2018, NULL,         1),
    ('BMW',        'M3',       2024, '2025-01-15', 0),
    ('Volkswagen', 'Golf',     2021, '2024-09-01', 0);
GO

-- Confirmar
SELECT * FROM Vehicles;
GO