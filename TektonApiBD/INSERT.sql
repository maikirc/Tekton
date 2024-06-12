USE [Tekton]
GO

INSERT INTO [dbo].[Product]
	([Name]
    ,[Status]
    ,[Stock]
    ,[Description]
    ,[Price]
    ,[CreationDate]
    ,[CreationUser]
    ,[LastModificationDate]
    ,[LastModificationUser])
VALUES
	('MOUSE'
    ,1
    ,100
    ,'MOUSE INALAMBRICO'
    ,20.50
    ,GETDATE()
    ,'MREGALADO'
    ,GETDATE()
    ,'MREGALADO')
GO


