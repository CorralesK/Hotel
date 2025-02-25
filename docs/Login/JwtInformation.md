# Configuracion para JWT

Para configurar JWT en tu proyecto .NET 8, sigue los siguientes pasos:

## Instalacion de Paquetes

1. Agrega el paquete `System.IdentityModel.Tokens.Jwt`:
2. Agrega el paquete `Microsoft.AspNetCore.Authentication.JwtBearer` (version 8.0.13):Estos paquetes son necesarios para manejar la autenticacion y generacion de tokens JWT.

## Instalacion de Paquetes para Pruebas

Para realizar pruebas unitarias, instala los siguientes paquetes:

1. `NUnit`:2. `Moq`:3. `NUnit3TestAdapter`:Estos paquetes te permitiran escribir y ejecutar pruebas unitarias de manera eficiente.

## Resumen

- **Paquetes para JWT**:
  - `System.IdentityModel.Tokens.Jwt`
  - `Microsoft.AspNetCore.Authentication.JwtBearer` (8.0.13)

- **Paquetes para Pruebas**:
  - dotnet add package `NUnit`
  - dotnet add package `Moq`
  - dotnet add package `NUnit3TestAdapter`

Sigue estos pasos para configurar y probar JWT en tu proyecto .NET 8.

