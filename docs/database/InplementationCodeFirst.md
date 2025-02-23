# Implementación Code First

## Descripción General
Code First en Entity Framework permite a los desarrolladores definir el modelo de datos utilizando clases en C#. 
Entity Framework genera la base de datos y las tablas correspondientes a partir de estas clases. 
Esta guía cubre los comandos esenciales para la migración de bases de datos.

## Paquetes Requeridos
![Texto alternativo](/docs/database/paquetes.png)

> dotenv.Net
> Microsoft Entity Framework Core
> Microsoft Entity Framework tools
> Npgsql Entity Framework Core PostgreSQL

## Comandos de Migración

### Agregar Nueva Migración
```powershell
Add-Migration "Initial"
```
Crea un archivo de migración que contiene instrucciones para actualizar la base de datos según los cambios realizados en el modelo de datos.

### Actualizar Base de Datos
```powershell
Update-Database
```
Aplica las migraciones pendientes para actualizar la base de datos a la última versión del modelo de datos.

### Revertir a una Migración Anterior
```powershell
Update-Database MigrationName
```
Revierte la base de datos a un estado específico de una migración anterior.

### Eliminar Última Migración
```powershell
Remove-Migration
```
Elimina el archivo de la migración más reciente (solo si no se ha aplicado).

### Eliminar Base de Datos
```powershell
Drop-Database
```
Elimina completamente la base de datos especificada.

## Pasos de Implementación
1. Crear las clases del modelo en C#
2. Agregar una migración inicial
   ```powershell
   Add-Migration "Initial"
   ```
3. Crear/actualizar la base de datos
   ```powershell
   Update-Database
   ```

