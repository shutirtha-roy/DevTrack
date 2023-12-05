CREATE OR ALTER PROCEDURE [dbo].[sp_GetProjects]
@PageIndex int,
@PageSize int,
@OrderBy nvarchar(50),
@ApplicationUserId uniqueidentifier,
@Title nvarchar(250) = '%', 
@IsArchived bit = null,
@Role int = null,
@Total int output,
@TotalDisplay int output

AS
BEGIN
	Declare @sql nvarchar(4000);
	Declare @totalSql nvarchar(4000)
	Declare @countSql nvarchar(4000);
	Declare @paramList nvarchar(MAX);
	Declare @totalparamList nvarchar(MAX);
	Declare @countparamList nvarchar(MAX);

	SET NOCOUNT ON;
	
	SET @totalSql = 'SELECT @Total = COUNT(*) FROM ProjectUsers LEFT JOIN Projects ON ProjectUsers.ProjectId = Projects.Id WHERE 1 = 1 '
	SET @countsql = 'SELECT @TotalDisplay = COUNT(*) FROM ProjectUsers LEFT JOIN Projects ON ProjectUsers.ProjectId = Projects.Id WHERE 1 =1 ';

	IF @ApplicationUserId IS NOT NULL
		BEGIN
			SET @totalSql = @totalSql + ' AND ProjectUsers.ApplicationUserId = @xApplicationUserId '; 
			SET @countSql = @countSql + ' AND ProjectUsers.ApplicationUserId = @xApplicationUserId '; 
		END
	
	IF @Role IS NOT NULL
		BEGIN
			SET @totalSql = @totalSql + ' AND ProjectUsers.Role = @xRole '; 
			SET @countSql = @countSql + ' AND ProjectUsers.Role = @xRole '; 
		END
	END

	IF @Title IS NOT NULL
		SET @countsql = @countSql + ' AND Title LIKE ''%'' + @xTitle + ''%''';

	IF @IsArchived IS NOT NULL
		SET @countSql = @countSql + ' AND Projects.IsArchived = @xIsArchived ';


	SET @sql = 'SELECT 
					 PU.ApplicationUserId
					,PU.ProjectId
					,PU.Role
					,U.Name
					,P.Title
					,INV.Status
					,CASE 
						WHEN PU.Role = 1 THEN
							(SELECT (COUNT(*)-1) FROM ProjectUsers sPU WHERE sPU.ProjectId = PU.ProjectId)
						WHEN PU.Role = 2 AND INV.Status = 2 THEN 1
						ELSE NULL
					  END TotalMembers 
					 ,CASE
						WHEN PU.Role = 1 THEN
							(SELECT CONVERT(FLOAT, CAST(ISNULL(SUM(DATEDIFF(SECOND, sA.StartTime, sA.EndTime) / 3600.0), 0) AS decimal(18,2))) FROM Activities sA WHERE sA.ProjectId = PU.ProjectId)
						WHEN PU.Role = 2 AND INV.Status = 2 THEN
							(SELECT CONVERT(FLOAT, CAST(ISNULL(SUM(DATEDIFF(SECOND, sA.StartTime, sA.EndTime) / 3600.0), 0) AS decimal(18,2))) FROM Activities sA WHERE sA.ApplicationUserId = PU.ApplicationUserId AND sA.ProjectId = PU.ProjectId)
						ELSE NULL
					  END TotalLogHours
	 
				FROM ProjectUsers PU
				left join AspNetUsers U ON PU.ApplicationUserId = U.Id
				left join Projects P ON PU.ProjectId = P.Id
				left join Invitations INV ON PU.ProjectId = INV.ProjectId AND U.Email = INV.Email
				WHERE PU.ApplicationUserId = @xApplicationUserId ';

	IF @Role IS NOT NULL
		SET @sql = @sql + ' AND PU.Role = @xRole'; 

	IF @Title IS NOT NULL
		SET @sql = @sql + ' AND P.Title LIKE ''%'' + @xTitle + ''%''';

	IF @IsArchived IS NOT NULL
		SET @sql = @sql + ' AND P.IsArchived = @xIsArchived '

	SET @sql = @sql + ' ORDER BY '+ @OrderBy +' OFFSET @PageSize * (@PageIndex - 1) 
		ROWS FETCH NEXT @PageSize ROWS ONLY';

	SELECT @totalparamlist = '@xApplicationUserId uniqueidentifier,
		@xRole int,
		@xIsArchived bit, 
		@Total int output';

	exec sp_executesql @totalsql, @totalparamlist,
		@ApplicationUserId,
		@Role,
		@IsArchived, 
		@Total = @Total output;

	SELECT @countparamlist = '@xApplicationUserId uniqueidentifier,
		@xRole int,
		@xTitle nvarchar(250),
		@xIsArchived bit, 
		@TotalDisplay int output';

	exec sp_executesql @countsql, @countparamlist,
		@ApplicationUserId,
		@Role,
		@Title, 
		@IsArchived, 
		@TotalDisplay = @TotalDisplay output;

	SELECT @paramlist = '@xApplicationUserId uniqueidentifier,
		@xRole int,
		@xTitle nvarchar(250), 
		@xIsArchived bit, 
		@PageIndex int, 
		@PageSize int';

	exec sp_executesql @sql, @paramlist,
		@ApplicationUserId,
		@Role,
		@Title,
		@IsArchived, 
		@PageIndex,
		@PageSize;

	print @sql;
	print @totalSql;
	print @countSql;

GO