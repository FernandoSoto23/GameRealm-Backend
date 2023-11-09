
-- Obtener la fecha y hora actual en tu zona horaria


ALTER DATABASE [dbo.SekyhRestaurant] SET TIMEZONE = 'Central Standard Time';

SELECT getDate()


create table tipoMenu(
	Codigo int not null primary key identity(1,1),
	Nombre varchar(150) not null
);

create table Platillo(
	Codigo int not null primary key identity(100101,1),
	titulo varchar(150) not null,
	imagen varchar(150),
	precio numeric(12,2) not null check(precio>0),
	descripcion varchar(2000),
	tipoMenu int references tipoMenu(codigo) on delete no action on update no action 
);
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


create table Empleado(
	id int not null primary key,
	nombreCompleto varchar(150) not null,
	email varchar(150) unique not null,
	pwd varchar(64)not null,
	telefono varchar(20) not null,
	[admin] char(1) check([admin] = 's' or [admin] = 'n'),
	token char(60) not null unique
);
alter table Usuario add [Admin] char(1) check([Admin] = 's' or [Admin] = 'n')

CREATE TABLE Vitacora(
	id int not null primary key identity(1,1),
	TipoDeAccion int references TipoDeAccion(id),
	Usuario int not null references usuario(id),
	fechaYhora datetime,
);
CREATE TABLE TipoDeAccion(
	id int not null primary key,
	nombre varchar(20) not null,
);

select * from Platillo


--Creando Procedimientos de almacenado
ALTER PROCEDURE spAddPlatillo(
	@adminID int,
	@adminToken varchar(64),
	@titulo varchar(150),
	@imagen varchar(150),
	@precio numeric(12,2),
	@descripcion varchar(64),
	@tipoMenu int

)
as
begin	

	if exists(SELECT * FROM usuario WHERE id= @adminId and token = @adminToken and [Admin] = 's')begin

			INSERT INTO platillo(titulo,imagen,precio,descripcion,tipomenu)
			VALUES(@titulo,@imagen,@precio,@descripcion,@tipoMenu)

			--ASIGNAR ZONA HORARIA CORRECTAMENTE
			
			declare @fecha datetime
			set @fecha = (Select dbo.ConvertirFecha())

			INSERT INTO vitacora(TipoDeAccion,Usuario,fechayhora)
			VALUES(1,@adminId,@fecha)

			print 'Se guardo todo con madre' 
	end	
end

select * from usuario 
select * from vitacora
select * from platillo where tipomenu = 5
select * from tipodeaccion

ALTER PROCEDURE spUpdatePlatillo(
	@adminID int,
	@adminToken varchar(64),
	@codigo int,
	@titulo varchar(150),
	@imagen varchar(150),
	@precio varchar(150),
	@descripcion varchar(64),
	@tipoMenu int

)
as
begin	
	if exists(SELECT * FROM usuario WHERE id=@adminId and token = @adminToken)begin
			UPDATE platillo 
			SET titulo = @titulo,
				imagen = @imagen,
				precio = @precio,
				descripcion = @descripcion,
				tipomenu = @tipoMenu
			WHERE codigo = @codigo

			--ASIGNAR ZONA HORARIA CORRECTAMENTE
			
			declare @fecha datetime
			set @fecha = (Select dbo.ConvertirFecha())

			INSERT INTO vitacora(TipoDeAccion,Usuario,fechayhora)
			VALUES(3,@adminId,@fecha)

			print 'Se guardo todo con madre' 
	end	
end


spAddPlatillo 3,'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3','nuevoproducto','imagen',20,'adss',1






CREATE PROCEDURE spAddUser(
	@nombreCompleto varchar(150),
	@NombreUsuario varchar(150),
	@email varchar(150),
	@pwd varchar(64),
	@telefono varchar(20)
)
as
begin
	
	INSERT INTO Usuario(nombreCompleto,nombreUsuario,email,pwd,telefono,activo,[Admin])
	VALUES(@nombreCompleto,@NombreUsuario,@email,@pwd,@telefono,'s','n')
	
end

spAddUser 'Antonio','Concha','Antonio@gmail.com','1234','66237712123'



SELECT * FROM usuario
SELECT * FROM Platillo
SELECT * FROM tipoMenu

--CREANDO TRIGGERS


--Creando Funciones

alter FUNCTION ConvertirFecha()
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