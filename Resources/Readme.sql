-- BORRAR TODA LA BASE DE DATOS Y VOLVER A CREARLA

USE master;

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'DiazNaturals')
BEGIN
    ALTER DATABASE DiazNaturals SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DiazNaturals;
END;

CREATE DATABASE DiazNaturals;


-- ACCEDER A LA BASE DE DATOS

USE DiazNaturals;

-- CORRER EL SCRIPT


-- VER LAS TABLAS CREADAS EN LA BASE DE DATOS

SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';


-- VER TODOS LOS CONSTRAINT CREADOS

SELECT CONSTRAINT_NAME, CONSTRAINT_TYPE, TABLE_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS;


-- VER LAS LLAVES PRIMARIAS

SELECT CONSTRAINT_NAME, TABLE_NAME
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
WHERE CONSTRAINT_TYPE = 'PRIMARY KEY';


-- VER LAS LLAVES FORÁNEAS

SELECT 
    KCU.CONSTRAINT_NAME AS FKConstraintName,
    KCU.TABLE_NAME AS TableName,
    KCU.COLUMN_NAME AS ColumnName,
    RC.UNIQUE_CONSTRAINT_NAME AS ReferencedPK
FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU
INNER JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
    ON KCU.CONSTRAINT_NAME = RC.CONSTRAINT_NAME;


-- VER INFORMER GENERAL DE LLAVES PRIMARIAS Y FORÁNEAS

SELECT
    C.CONSTRAINT_NAME AS ConstraintName,
    C.TABLE_NAME AS TableName,
    K.COLUMN_NAME AS ColumnName,
    C2.CONSTRAINT_NAME AS ReferencedPK,
    C2.TABLE_NAME AS ReferencedTableName
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS C
JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE K
    ON C.CONSTRAINT_NAME = K.CONSTRAINT_NAME
LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC
    ON C.CONSTRAINT_NAME = RC.CONSTRAINT_NAME
LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS C2
    ON RC.UNIQUE_CONSTRAINT_NAME = C2.CONSTRAINT_NAME
WHERE C.CONSTRAINT_TYPE IN ('PRIMARY KEY', 'FOREIGN KEY');