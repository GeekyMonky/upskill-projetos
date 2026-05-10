USE master;
GO

IF EXISTS (SELECT Name FROM sys.databases WHERE Name = 'BibliotecaXPTO')
BEGIN
    ALTER DATABASE BibliotecaXPTO SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE BibliotecaXPTO;
END;
GO

CREATE DATABASE BibliotecaXPTO;
GO

USE BibliotecaXPTO;
GO


-- =========================================================
-- TABELAS
-- =========================================================

CREATE TABLE Tema (
      TemaID INT IDENTITY(1,1) NOT NULL
    , Designacao NVARCHAR(50) NOT NULL UNIQUE
    , CONSTRAINT PK_Tema PRIMARY KEY (TemaID)
);
GO

CREATE TABLE Nucleo (
      NucleoID INT IDENTITY(1,1) NOT NULL
    , Distrito NVARCHAR(50) NOT NULL UNIQUE
    , EhCentral BIT NOT NULL DEFAULT 0
    , CONSTRAINT PK_Nucleo PRIMARY KEY (NucleoID)
);
GO

CREATE TABLE Leitor (
      LeitorID INT IDENTITY(1,1) NOT NULL
    , Nome NVARCHAR(100) NOT NULL
    , Email VARCHAR(100) NOT NULL UNIQUE
    , EstaAtivo BIT NOT NULL DEFAULT 1
    , NumeroAtrasos INT NOT NULL DEFAULT 0
    , DataUltimaRequisicao DATE NULL
    , DataRegisto DATE NOT NULL DEFAULT GETDATE()
    , CONSTRAINT PK_Leitor PRIMARY KEY (LeitorID)
);
GO

CREATE TABLE Obra (
      ObraID INT IDENTITY(1,1) NOT NULL
    , Titulo NVARCHAR(100) NOT NULL
    , Autor NVARCHAR(100) NOT NULL
    , ISBN VARCHAR(20) NOT NULL UNIQUE
    , Capa VARCHAR(255) NULL
    , TemaID INT NOT NULL
    , CONSTRAINT PK_Obra PRIMARY KEY (ObraID)
    , CONSTRAINT FK_Obra_Tema FOREIGN KEY (TemaID)
      REFERENCES Tema (TemaID)
);
GO

CREATE TABLE Exemplar (
      ExemplarID INT IDENTITY(1,1) NOT NULL
    , ObraID INT NOT NULL
    , NucleoID INT NOT NULL
    , CONSTRAINT PK_Exemplar PRIMARY KEY (ExemplarID)
    , CONSTRAINT FK_Exemplar_Obra FOREIGN KEY (ObraID)
      REFERENCES Obra (ObraID)
    , CONSTRAINT FK_Exemplar_Nucleo FOREIGN KEY (NucleoID)
      REFERENCES Nucleo (NucleoID)
);
GO

CREATE TABLE Requisicao (
      RequisicaoID INT IDENTITY(1,1) NOT NULL
    , DataEmprestimo DATE NOT NULL DEFAULT GETDATE()
    , LeitorID INT NOT NULL
    , CONSTRAINT PK_Requisicao PRIMARY KEY (RequisicaoID)
    , CONSTRAINT FK_Requisicao_Leitor FOREIGN KEY (LeitorID)
      REFERENCES Leitor (LeitorID)
);
GO

CREATE TABLE ExemplarRequisicao (
      ExemplarID INT NOT NULL
    , RequisicaoID INT NOT NULL
    , DataDevolucao DATE NULL
    , CONSTRAINT PK_ExemplarRequisicao PRIMARY KEY (ExemplarID, RequisicaoID)
    , CONSTRAINT FK_ExemplarRequisicao_Exemplar FOREIGN KEY (ExemplarID)
      REFERENCES Exemplar (ExemplarID)
    , CONSTRAINT FK_ExemplarRequisicao_Requisicao FOREIGN KEY (RequisicaoID)
      REFERENCES Requisicao (RequisicaoID)
);
GO


-- =========================================================
-- DADOS DE TESTE
-- =========================================================

-- Temas
INSERT INTO Tema (Designacao) VALUES
    ('Ficcao'),
    ('Romance'),
    ('Historia'),
    ('Ciencia'),
    ('Biografia');

-- Nucleos
INSERT INTO Nucleo (Distrito, EhCentral) VALUES
    ('Lisboa', 1),
    ('Porto', 0),
    ('Coimbra', 0),
    ('Faro', 0);

-- Leitores
INSERT INTO Leitor (Nome, Email, EstaAtivo, NumeroAtrasos, DataUltimaRequisicao, DataRegisto) VALUES
    ('Ana Silva',     'ana.silva@email.com',    1, 0, '2025-09-15', '2024-01-10'),
    ('Bruno Costa',   'bruno.costa@email.com',  1, 1, '2025-10-02', '2024-03-22'),
    ('Carla Mendes',  'carla.mendes@email.com', 1, 0, '2025-08-20', '2024-05-05'),
    ('Diogo Pereira', 'diogo.p@email.com',      0, 3, '2025-06-12', '2023-11-15'),
    ('Eva Tavares',   'eva.tavares@email.com',  1, 0, NULL,          '2025-09-01');

-- Obras
INSERT INTO Obra (Titulo, Autor, ISBN, Capa, TemaID) VALUES
    ('1984',                         'George Orwell',     '9780451524935', NULL, 1),
    ('O Alquimista',                 'Paulo Coelho',      '9780062315007', NULL, 2),
    ('Sapiens',                      'Yuval Noah Harari', '9780062316097', NULL, 3),
    ('A Breve Historia do Tempo',    'Stephen Hawking',   '9780553380163', NULL, 4),
    ('Steve Jobs',                   'Walter Isaacson',   '9781451648539', NULL, 5),
    ('Os Maias',                     'Eca de Queiros',    '9789722325806', NULL, 2),
    ('Fahrenheit 451',               'Ray Bradbury',      '9781451673319', NULL, 1),
    ('Cosmos',                       'Carl Sagan',        '9780345539434', NULL, 4),
    ('Mensagem',                     'Fernando Pessoa',   '9789722006286', NULL, 2),
    ('A Origem das Especies',        'Charles Darwin',    '9780451529060', NULL, 4);

-- Exemplares
INSERT INTO Exemplar (ObraID, NucleoID) VALUES
    (1, 1), (1, 1), (1, 2),
    (2, 1), (2, 2),
    (3, 1), (3, 3),
    (4, 1), (4, 2),
    (5, 1),
    (6, 2), (6, 3),
    (7, 1), (7, 4),
    (8, 1), (8, 2),
    (9, 1),
    (10, 3), (10, 4),
    (10, 1);

-- Requisicoes
INSERT INTO Requisicao (DataEmprestimo, LeitorID) VALUES
    ('2025-09-10', 1),
    ('2025-09-25', 2),
    ('2025-10-05', 3),
    ('2025-10-15', 1),
    ('2025-10-20', 2);

-- ExemplarRequisicao
INSERT INTO ExemplarRequisicao (ExemplarID, RequisicaoID, DataDevolucao) VALUES
    (1,  1, '2025-09-24'),
    (4,  1, '2025-09-24'),
    (6,  2, '2025-10-08'),
    (10, 3, NULL),
    (15, 3, NULL),
    (8,  4, '2025-10-29'),
    (13, 5, NULL);

GO

PRINT 'Base de dados BibliotecaXPTO criada com sucesso!';