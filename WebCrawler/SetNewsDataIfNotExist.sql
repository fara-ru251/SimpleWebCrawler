USE [NewsDB]
GO
/****** Object:  StoredProcedure [dbo].[SetNewsDataIfNotExist]    Script Date: 01.03.2021 9:40:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Farabi Kurmanshady
-- Create date: 26.02.2021
-- Description:	Save Article
-- =============================================
ALTER PROCEDURE [dbo].[SetNewsDataIfNotExist] 
	@CreatedOn datetime2,
	@Title nvarchar(1024),
	@Text nvarchar(MAX),
	@Url nvarchar(256)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF NOT EXISTS(SELECT 1 FROM dbo.News WHERE [Url] = @Url)
		BEGIN
			INSERT INTO dbo.News(Title, CreatedOn, [Text], [Url])
			VALUES(@Title, @CreatedOn, @Text, @Url)
			SELECT CAST(0 AS BIT)
		END
	ELSE
		BEGIN
			SELECT CAST(1 AS BIT)
		END
END
