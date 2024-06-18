# TektonApi

TektonApi fue desarrollado basándose en los principios SOLID, si lo consideramos como 1 de muchos apis de una aplicación, entonces la arquitectura utilizada es la de microservicios, utiliza el patrón repository para separar la lógica de acceso a datos de la lógica del negocio, además utiliza el principio de "separation of concerns", donde el api es separado en n capas y cada una está encargada de una tarea específica, las capas desarrolladas son:

Capas
=====
1.  Data: Contiene el contexto a la base de datos, para la generación se utiliza EntityFramework.
2.  Entites: Contiene las clases de las entidades de la base de datos que se vayan a utilizar en la solución.
3.  ViewModel: Contiene las clases DTO (Data Transfer Object) que se utilizan en el envío o recepción de datos entre las capas de la solución.
4.  Repository: Esta capa instancia al contexto de la base de datos, además se encarga de transformar los DTO en las Entidades de la base, y viceversa.
5.  IRepository: Interfase de la capa Repository.
6.  Logic: Capa que recibe los datos de la capa Repository y aplica la lógica de negocio necesaria.
7.  Services: Esta capa es quien contiene la programación de los métodos disponibles a utilizar en la API.
8.  IServices: Es la interfase de la capa Service.
9.  Validator: Esta capa contiene los patrones de validación aplicadas a los request, se utiliza FluentValidation.
10. Api: Es la capa que expone los servicios publicados.
11. Test: Esta capa contiene los tests realizados al api, se utiliza Moq.

Para levantar el proyecto localmente, considere lo siguiente:

Inicializar Base de Datos MS Sql Server
=======================================
Cargar el respaldo TektonBackup ubicado en la carpeta \\TektonApiBD, o ejecutar los scripts ubicados en la misma carpeta en el siguiente orden:
1. DATABASE.sql (Modifique la ruta donde desea grabar los archivos .mdf y .ldf)
2. TABLES.sql
3. INSERT.sql

Inicializar Tekton.Api
=====================
Modificar las siguientes keys del archivo appsettings.json:
1. Serilog:WriteTo:path
2. ConnectionStrings:Tekton

