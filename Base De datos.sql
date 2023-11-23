
-- Obtener la fecha y hora actual en tu zona horaria

Create database GameRealDTB


ALTER DATABASE [dbo.SekyhRestaurant] SET TIMEZONE = 'Central Standard Time';

SELECT getDate()

--------------------------
create table Categoria(
	Codigo int not null primary key identity(1,1),
	Nombre varchar(150) not null
);


create table Titulo(
	Codigo int not null primary key identity(100101,1),
	Nombre varchar(150) not null,
	imagen varchar(150),
	precio numeric(12,2) not null check(precio>0),
	descripcion varchar(2000),
	Categoria int references Categoria(codigo) on delete no action on update no action 
);
alter table Titulo add plataforma text
alter table Titulo add Region varchar(6)
alter table Titulo add Idioma text
alter table Titulo add Link Text

select * from Titulo
select * from Region
create table Usuario(
	id int not null primary key identity(1,1),
	nombreCompleto varchar(150) not null,
	NombreUsuario varchar(150) unique not null,
	email varchar(150) unique not null,
	pwd varchar(64)not null,
	telefono varchar(20) not null,
	activo char(1) check(activo = 's' or activo = 'n'),
	token varchar(64) unique 
);	
alter table Usuario add [Admin] char(1) check([Admin] = 's' or [Admin] = 'n')
alter table Usuario add Tarjetas text
alter table Usuario add fecha date
alter table Usuario add pais char(6) references region(clave)


select * from Region
select * from Usuario

select * from Titulo

create table Empleado(
	id int not null primary key,
	nombreCompleto varchar(150) not null,
	email varchar(150) unique not null,
	pwd varchar(64)not null,
	telefono varchar(20) not null,
	[admin] char(1) check([admin] = 's' or [admin] = 'n'),
	token char(60) not null unique
);



select * from Usuario

CREATE TABLE Bitacora(
	id int not null primary key identity(1,1),
	TipoDeAccion int references TipoDeAccion(id),
	Usuario int not null references usuario(id),
	fechaYhora datetime,
);
CREATE TABLE TipoDeAccion(
	id int not null primary key,
	nombre varchar(20) not null,
);

CREATE TABLE Biblioteca(
	id int not null primary key identity(1,1),
	idUsuario int references Usuario(id),
	CodigoTitulo int references Titulo(codigo),
	TituloKey varchar(64) not null

);
CREATE TABLE Region(
    Clave CHAR(6) PRIMARY KEY,
    RegionNombre VARCHAR(255) NOT NULL
)

SELECT * FROM Titulo
SELECT * FROM Usuario
select * from Biblioteca
--Creando Procedimientos de almacenado

CREATE PROCEDURE spAddTitulo(
	@adminID int,
	@adminToken varchar(64),
	@nombre varchar(150),
	@imagen varchar(150),
	@precio numeric(12,2),
	@descripcion varchar(64),
	@Categoria int

)
as
begin	

	if exists(SELECT * FROM usuario WHERE id= @adminId and token = @adminToken and [Admin] = 's')begin

			INSERT INTO Titulo(Nombre,imagen,precio,descripcion,Categoria)
			VALUES(@nombre,@imagen,@precio,@descripcion,@Categoria)

			--ASIGNAR ZONA HORARIA CORRECTAMENTE
			
			declare @fecha datetime
			set @fecha = (Select dbo.ConvertirFecha())

			INSERT INTO Bitacora(TipoDeAccion,Usuario,fechayhora)
			VALUES(1,@adminId,@fecha)

			print 'Se guardo todo con madre' 
	end	
end

select * from usuario 
select * from Bitacora
select * from Titulo where Categoria = 5
select * from tipodeaccion
select * from Region
Create PROCEDURE spUpdateTitulo(
	@adminID int,
	@adminToken varchar(64),
	@codigo int,
	@Nombre varchar(150),
	@imagen varchar(150),
	@precio varchar(150),
	@descripcion varchar(64),
	@Categoria int

)
as
begin	
	if exists(SELECT * FROM usuario WHERE id=@adminId and token = @adminToken)begin
			UPDATE Titulo 
			SET Nombre = @Nombre,
				imagen = @imagen,
				precio = @precio,
				descripcion = @descripcion,
				Categoria = @Categoria
			WHERE codigo = @codigo

			--ASIGNAR ZONA HORARIA CORRECTAMENTE
			
			declare @fecha datetime
			set @fecha = (Select dbo.ConvertirFecha())

			INSERT INTO Bitacora(TipoDeAccion,Usuario,fechayhora)
			VALUES(3,@adminId,@fecha)

			print 'Se guardo todo con madre' 
	end	
end


spAddTitulo 2,'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3','nuevoproducto','imagen',20,'adss',1


alter PROCEDURE spAddUser(
	@nombreCompleto varchar(150),
	@NombreUsuario varchar(150),
	@email varchar(150),
	@pwd varchar(64),
	@telefono varchar(20),
	@codigo char(4),
	@token char(64)
)
as
begin
	
	INSERT INTO Usuario(nombreCompleto,nombreUsuario,email,pwd,telefono,activo,[Admin],codigoVerificacion,token)
	VALUES(@nombreCompleto,@NombreUsuario,@email,@pwd,@telefono,'n','n',@codigo,@token)
	
end

spAddUser 'Antonio','si','Antonioa@gmail.com','123','66237712123','1234','0123'

SELECT * FROM Usuario

ALTER TABLE usuario
ADD codigoVerificacion CHAR(4);
SELECT * FROM usuario WHERE codigoVerificacion = 1234

SELECT * FROM usuario WHERE codigoVerificacion = '12'
SELECT * FROM Titulo
SELECT * FROM Categoria

--CREANDO TRIGGERS


--Creando Funciones

Create FUNCTION ConvertirFecha()
returns datetime
as
begin
			--Azure SQL Database no admite la configuración de zonas horarias; 
			--siempre sigue la hora UTC. Si necesita interpretar la información
			--de fecha y hora en una zona horaria distinta de UTC, use el valor AT TIME ZONE en SQL Database

			DECLARE @fechaHoraActual DATETIMEOFFSET;
			SET @fechaHoraActual = SYSDATETIMEOFFSET() AT TIME ZONE 'Pacific Standard Time';
			RETURN @fechaHoraActual

end



select dbo.ConvertirFecha()

			declare @fecha varchar(150)
			set @fecha = (Select dbo.ConvertirFecha())
			print @fecha


			se


			SELECT * FROM usuario
-- Nuevas consultas a agregar para la db

ALTER PROCEDURE ActivarUsuarioVerificado(
	@usuario varchar(200),
	@codigo varchar(200)
)
as
begin
	if exists((SELECT * FROM usuario WHERE nombreUsuario = @usuario and codigoVerificacion = @codigo))begin
		UPDATE Usuario SET activo = 's' WHERE NombreUsuario = @usuario
		SELECT * FROM Usuario WHERE NombreUsuario = @usuario
	end

end

ActivarUsuarioVerificado si , 1234


select * from Usuario

UPDATE Usuario Set activo = 'n' WHERE id  = 4

DELETE FROM Usuario WHERE id =30



--INSERTS
-- Insertar países en la tabla de regiones
INSERT INTO Region (Clave, RegionNombre) VALUES ('USA', 'Estados Unidos');
INSERT INTO Region (Clave, RegionNombre) VALUES ('CAN', 'Canadá');
INSERT INTO Region (Clave, RegionNombre) VALUES ('MEX', 'México');
INSERT INTO Region (Clave, RegionNombre) VALUES ('GBR', 'Reino Unido');
INSERT INTO Region (Clave, RegionNombre) VALUES ('FRA', 'Francia');
INSERT INTO Region (Clave, RegionNombre) VALUES ('GER', 'Alemania');
INSERT INTO Region (Clave, RegionNombre) VALUES ('JPN', 'Japón');
-- Puedes agregar más países según tus necesidades

INSERT INTO Region (Clave, RegionNombre) VALUES
('Global', 'Global');

SELECT * FROM Region



