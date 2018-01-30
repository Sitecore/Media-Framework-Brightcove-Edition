SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MediaFrameworkMedia]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[MediaFrameworkMedia](
		[MediaId] [uniqueidentifier] NOT NULL,
		[Name] [nvarchar](500) NOT NULL,
		[Length] [int] NOT NULL,
		CONSTRAINT [PK_MediaFrameworkMedia] PRIMARY KEY CLUSTERED 
		(
			[MediaId] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Fact_MediaFrameworkEvents]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Fact_MediaFrameworkEvents](
		[Date] [smalldatetime] NOT NULL,
		[MediaId] [uniqueidentifier] NOT NULL,
		[PageEventDefinitionId] [uniqueidentifier] NOT NULL,
		[SiteNameId] [int] NOT NULL,
		[EventParameter] [varchar](10) NOT NULL,
		[Count] [bigint] NOT NULL,
		CONSTRAINT [PK_MediaFrameworkEvents] PRIMARY KEY CLUSTERED 
		(
			[Date] ASC,
			[MediaId] ASC,
			[PageEventDefinitionId] ASC,
			[SiteNameId] ASC,
			[EventParameter] ASC
		)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Fact_MediaFrameworkEvents]') AND name = N'SiteNameId')
BEGIN
	ALTER TABLE [dbo].[Fact_MediaFrameworkEvents]
		ADD [SiteNameId] [int] NOT NULL DEFAULT '-746361445' /* -746361445 is standart Sitecore site ID which will be setted as default for previous events. You can change it reguarding you logic. */
		
	ALTER TABLE [dbo].[Fact_MediaFrameworkEvents]
		DROP CONSTRAINT [PK_MediaFrameworkEvents]
		
	ALTER TABLE [dbo].[Fact_MediaFrameworkEvents]
		ADD CONSTRAINT PK_MediaFrameworkEvents PRIMARY KEY (Date,MediaId,PageEventDefinitionId,SiteNameId,EventParameter)
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fact_MediaFrameworkEvents_MediaFrameworkMedia]') AND parent_object_id = OBJECT_ID(N'[dbo].[Fact_MediaFrameworkEvents]'))
	ALTER TABLE [dbo].[Fact_MediaFrameworkEvents]  WITH NOCHECK ADD  CONSTRAINT [FK_Fact_MediaFrameworkEvents_MediaFrameworkMedia] FOREIGN KEY([MediaId])
	REFERENCES [dbo].[MediaFrameworkMedia] ([MediaId])
GO

IF EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Fact_MediaFrameworkEvents_MediaFrameworkMedia]') AND parent_object_id = OBJECT_ID(N'[dbo].[Fact_MediaFrameworkEvents]'))
	ALTER TABLE [dbo].[Fact_MediaFrameworkEvents] NOCHECK CONSTRAINT [FK_Fact_MediaFrameworkEvents_MediaFrameworkMedia]
GO