CREATE DATABASE BoletosDB;
USE BoletosDB;

CREATE TABLE XXXX(
	xXId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    xName VARCHAR(30) NOT NULL,
    xMail VARCHAR(50) NOT NULL ,
    xXx VARCHAR(40) NOT NULL 
);

ALTER TABLE XXXX
ADD CONSTRAINT UC_xName UNIQUE (xName);

ALTER TABLE XXXX
ADD CONSTRAINT UC_xMail UNIQUE (xMail);



CREATE TABLE EventoEstado( 
	EstadoId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Estado VARCHAR(150) NOT NULL
);

ALTER TABLE eventos
ADD EstadoId INT NOT NULL references EventoEstado(estadoid) ;


CREATE TABLE Compras(
	CompraId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Total DOUBLE NOT NULL,
    FechaCompra DATETIME NOT NULL,
    xMail VARCHAR(50) NOT NULL 
);

CREATE TABLE dCompras(
	Id INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Precio DOUBLE NOT NULL ,
    Cantidad INT NOT NULL ,
    CompraId INT NOT NULL references Compras(CompraId),
    BoletoId INT NOT NULL references Boletos(boletoId)
);

CREATE TABLE dSecciones (
	SeccionId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    EventoId INT NOT NULL references Eventos(EventoId),
    Seccion Varchar(50) NOT NULL,
    Precio DOUBLE NOT NULL
);


CREATE TABLE CategoriaEventos (
	CategoriaId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Categoria VARCHAR(50) NOT NULL
);

ALTER TABLE CategoriaEventos
ADD CONSTRAINT UC_Categoria UNIQUE (Categoria);

CREATE TABLE Ubicacion (
	UbicacionId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Ubicacion VARCHAR(150) NOT NULL,
    Latitud DOUBLE,
    Longitud DOUBLE,
    Especificaciones VARCHAR(250)
);

ALTER TABLE Ubicacion
ADD CONSTRAINT UC_Ubicacion UNIQUE (Ubicacion, Latitud, Longitud);

CREATE TABLE Eventos (
	EventoId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    NombreEvento VARCHAR(40) NOT NULL,
    Descripcion VARCHAR(255) NOT NULL,
    xXId INT References XXXX(xXId),
    CategoriaId INT NOT NULL References CategoriaEventos(CategoriaId) ,
    UbicacionId INT NOT NULL References Ubicacion(UbicacionId),
    FechaEvento DATETIME NOT NULL,
    BoletosDisponibles INT NOT NULL
);

CREATE TABLE Boletos (
	BoletoId INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    SeccionId INT NOT NULL references dsecciones(seccionId) ,
    EventoId INT NOT NULL references eventos(eventoId)
);