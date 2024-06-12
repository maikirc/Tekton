USE [Tekton]
GO

/****** Object:  Table [dbo].[Product]    Script Date: 12/06/2024 11:01:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Product](
	[ProductId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Status] [bit] NOT NULL,
	[Stock] [int] NOT NULL,
	[Description] [varchar](500) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[CreationDate] [datetime] NOT NULL,
	[CreationUser] [varchar](50) NOT NULL,
	[LastModificationDate] [datetime] NOT NULL,
	[LastModificationUser] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


