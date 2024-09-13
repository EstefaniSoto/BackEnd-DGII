# DGII Backend

Este es el proyecto backend para la prueba técnica de la DGII. Está desarrollado en .NET y proporciona la API para gestionar contribuyentes y comprobantes fiscales.
## Video demostrativo de la página web

Mire el video de demostración de la aplicación aquí: [Demostración de la Aplicación](https://youtu.be/587ASD0pMWE)

## Diseño
![image](https://github.com/user-attachments/assets/a650a0b4-70f3-4f00-bea8-f68258ed7403)

![image](https://github.com/user-attachments/assets/3c10dfee-e38c-44f9-a552-61bbf29bc5c3)




## Requisitos Previos

Antes de comenzar, asegúrese de tener instalados los siguientes programas en su máquina:

- [Git](https://git-scm.com/) para el control de versiones.
- [Node.js](https://nodejs.org/) para la ejecución de JavaScript en el servidor y la gestión de dependencias.
- [npm](https://www.npmjs.com/) (se instala junto con Node.js) para gestionar los paquetes de JavaScript.
- [SQL Server](https://www.microsoft.com/sql-server) para la base de datos.
- [Visual Studio](https://visualstudio.microsoft.com/) para el desarrollo en .NET.

### Instalación

1. **Clonar el Repositorio:**

   Clone este repositorio en su máquina local usando el siguiente comando:

   ```bash
   git clone https://github.com/EstefaniSoto/BackEnd-DGII.git
2. **Navegue al directorio del proyecto:**
 ```bash
 cd DGIIApi
```
2.1. **Crear base de datos (SQL Server)**
### Configuración de la Base de Datos

1. **Crear la Base de Datos y Tablas**

   Ejecute el siguiente script SQL en su servidor de base de datos para crear la base de datos, las tablas, agregar datos iniciales y el procedimiento almacenado:

   ```sql
   -- Crear la base de datos
   CREATE DATABASE DGII;
   GO

   -- Usar la base de datos recién creada
   USE DGII;
   GO

   -- Crear la tabla Listado_Contribuyentes
   CREATE TABLE Listado_Contribuyentes (
       rncCedula VARCHAR(11) PRIMARY KEY,
       nombre VARCHAR(50),
       tipo BIT,
       estatus BIT
   );
   GO

   -- Crear la tabla ComprobantesFiscales
   CREATE TABLE ComprobantesFiscales (
       RNC_Cedula VARCHAR(11),
       NCF VARCHAR(12) PRIMARY KEY,
       Monto DECIMAL(10, 2),
       ITBIS18 DECIMAL(10, 2),
       FOREIGN KEY (RNC_Cedula) REFERENCES Listado_Contribuyentes(rncCedula)
   );
   GO

   -- Insertar datos iniciales en Listado_Contribuyentes
   INSERT INTO Listado_Contribuyentes (rncCedula, nombre, tipo, estatus) VALUES
   ('00112345678', 'Juan Pérez', 1, 1),
   ('00223456789', 'María López', 0, 1);
   GO

   -- Insertar datos iniciales en ComprobantesFiscales
   INSERT INTO ComprobantesFiscales (RNC_Cedula, NCF, Monto, ITBIS18) VALUES
   ('00112345678', 'E310000001', 500.00, 90.00),
   ('00223456789', 'E310000002', 750.00, 135.00);
   GO

   -- Procedimiento Almacenado para insertar comprobantes
   CREATE PROCEDURE InsertComprobante
       @RNC_Cedula VARCHAR(11),
       @Monto DECIMAL(10, 2),
       @ITBIS18 DECIMAL(10, 2)
   AS
   BEGIN
       -- Asegurarse de que se maneje una única transacción para evitar inserciones múltiples
       BEGIN TRANSACTION;

       DECLARE @LastNCF VARCHAR(12);
       DECLARE @NewNCF VARCHAR(12);

       -- Obtener el último NCF de la tabla
       SELECT @LastNCF = MAX(NCF)
       FROM ComprobantesFiscales;

       -- Si no hay registros, establecer un valor inicial
       IF @LastNCF IS NULL
           SET @LastNCF = 'E310000000000'; -- Valor inicial

       -- Generar el nuevo NCF incrementando el último dígito
       SET @NewNCF = LEFT(@LastNCF, 9) + RIGHT(CONVERT(VARCHAR(12), CAST(RIGHT(@LastNCF, 3) AS INT) + 1), 3);

       -- Insertar el nuevo registro con el NCF generado
       INSERT INTO ComprobantesFiscales (RNC_Cedula, NCF, Monto, ITBIS18)
       VALUES (@RNC_Cedula, @NewNCF, @Monto, @ITBIS18);

       -- Confirmar la transacción
       COMMIT TRANSACTION;
   END;
   GO
 

