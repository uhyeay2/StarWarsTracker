﻿CREATE TABLE [dbo].[Event]
(
	[Id] INT NOT NULL CONSTRAINT [PK_Event] PRIMARY KEY IDENTITY(1, 1),
	[Name] NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[CanonTypeId] INT NOT NULL CONSTRAINT [FK_CanonType_Event] REFERENCES CanonType(Id)
)