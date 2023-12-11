CREATE DATABASE PortaAviones;

USE PortaAviones;

IF NOT EXISTS (
    SELECT
        name
    FROM
        master.sys.server_principals
    WHERE
        name = 'Dguzman')
BEGIN
    CREATE LOGIN Dguzman WITH PASSWORD = 'Admin@SQLServer03101',
    DEFAULT_DATABASE = PortaAviones;

END CREATE USER Dguzman FOR LOGIN Dguzman;

GRANT CONTROL ON DATABASE::PortaAviones TO Dguzman;

CREATE SCHEMA PortaAviones AUTHORIZATION Dguzman GRANT CONTROL ON SCHEMA::PortaAviones TO Dguzman;

CREATE TABLE PortaAviones.PortaAviones.marca(
    id int IDENTITY (1, 1) NOT NULL,
    nombre varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    CONSTRAINT marca_PK PRIMARY KEY (id),
    CONSTRAINT marca_nombre_UN UNIQUE (nombre)
);

CREATE TABLE PortaAviones.PortaAviones.despegue(
    codigo varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    id int IDENTITY (1, 1) NOT NULL,
    tecnico varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    mision varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    fecha_registro datetime2(0) DEFAULT getdate() NOT NULL,
    fecha_despegue datetime2(0) NOT NULL,
    CONSTRAINT despegue_PK PRIMARY KEY (id),
    CONSTRAINT despegue_UN UNIQUE (codigo)
);

CREATE TABLE PortaAviones.PortaAviones.modelo(
    id int IDENTITY (1, 1) NOT NULL,
    nombre varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    marca_fk int NOT NULL,
    CONSTRAINT modelo_PK PRIMARY KEY (id),
    CONSTRAINT modelo_nombre_UN UNIQUE (nombre),
    CONSTRAINT marca_FK FOREIGN KEY (marca_fk) REFERENCES PortaAviones.PortaAviones.marca(id)
);

CREATE TABLE PortaAviones.PortaAviones.aeronave(
    id int IDENTITY (1, 1) NOT NULL,
    serie varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    marca_fk int NOT NULL,
    modelo_fk int NOT NULL,
    nombre varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    alto decimal(38, 0) NOT NULL,
    ancho decimal(38, 0) NOT NULL,
    largo decimal(38, 0) NOT NULL,
    fecha_registro datetime2(0) DEFAULT getdate() NOT NULL,
    fecha_actualizacion datetime2(0) DEFAULT getdate() NOT NULL,
    retirado bit DEFAULT 0 NOT NULL,
    tecnico_ingreso varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    tecnico_retiro varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    perdida_material bit DEFAULT 0 NOT NULL,
    perdida_humana int NULL,
    razon_retiro varchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
    CONSTRAINT aeronave_PK PRIMARY KEY (id),
    CONSTRAINT aeronave_serie_UN UNIQUE (serie),
    CONSTRAINT aeronave_marca_FK FOREIGN KEY (marca_fk) REFERENCES PortaAviones.PortaAviones.marca(id),
    CONSTRAINT modelo_FK FOREIGN KEY (modelo_fk) REFERENCES PortaAviones.PortaAviones.modelo(id)
);

CREATE TABLE PortaAviones.PortaAviones.aeronaves_despegue(
    id int IDENTITY (1, 1) NOT NULL,
    despegue_fk int NOT NULL,
    aeronave_fk int NOT NULL,
    piloto varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
    CONSTRAINT aeronaves_despegue_PK PRIMARY KEY (id),
    CONSTRAINT despegue_FK FOREIGN KEY (despegue_fk) REFERENCES PortaAviones.PortaAviones.despegue(id),
    CONSTRAINT despegue_aeronave_FK FOREIGN KEY (id) REFERENCES PortaAviones.PortaAviones.aeronave(id)
);

CREATE SEQUENCE PortaAviones.secuencia_despegues
    AS int START WITH 1
    INCREMENT BY 1;

INSERT INTO PortaAviones.PortaAviones.marca(nombre)
    VALUES (N'Boeing'),
(N'Dassault'),
(N'Lockheed Martin'),
(N'Saab'),
(N'Sukhoi');

INSERT INTO PortaAviones.PortaAviones.modelo(nombre, marca_fk)
    VALUES (N'F-35 Lightning II', 3),
(N'F-22 Raptor', 3),
(N'SR-71 Blackbird', 3),
(N'F-16 Fighting Falcon', 3),
(N'Su-35', 5),
(N'Su-37', 5),
(N'Su-39', 5),
(N'Su-47', 5),
(N'Su-57', 5),
(N'F/A-18 Super Hornet', 1),
(N'F/A-18E/F Advanced Super Hornet II', 1),
(N'F-15SG Strike Eagle', 1),
(N'F-15K Slam Eagle', 1),
(N'F-15SE Silent Eagle', 1),
(N'Draken', 4),
(N'Gripen', 4),
(N'Viggen 37', 4),
(N'Lansen 32', 4),
(N'Tunnan 29', 4),
(N'Rafale F3', 2),
(N'Neuron', 2),
(N'Mirage 2000C', 2),
(N'Mirage 4000', 2),
(N'Falcon Guardian', 2);

