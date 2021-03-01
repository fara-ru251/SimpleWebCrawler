USE [NewsDB]
GO
/****** Object:  StoredProcedure [dbo].[SetWords]    Script Date: 01.03.2021 9:41:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Farabi Kurmanshady
-- Create date: 26.02.2021
-- Description:	Save Frequent Words
-- =============================================
ALTER PROCEDURE [dbo].[SetWords] 
	@Name nvarchar(256),
	@Frequency int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT 1 FROM dbo.FrequentWords WHERE [Name] = @Name)
	BEGIN
		INSERT INTO dbo.FrequentWords([Name], Frequency)
		VALUES(@Name, @Frequency)
	END
	ELSE
	BEGIN
		UPDATE dbo.FrequentWords
		SET 
			Frequency = Frequency + @Frequency
		WHERE [Name] = @Name
	END
END
