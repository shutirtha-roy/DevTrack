﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTrack.Web.Data.Migrations
{
    public partial class Modify_StoredProcedure_GetInvitationDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"CREATE OR ALTER PROCEDURE [dbo].[sp_GetProjects]
						@PageIndex int,
						@PageSize int,
						@SearchText nvarchar(250) = '%',
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

							
							IF @SearchText IS NOT NULL
								SET @countsql = @countSql + ' AND Title LIKE ''%'' + @xSearchText + ''%''';

							SET @sql = 'SELECT 
											 U.Id ApplicationUserId
											,INV.ProjectId
											,CASE 
												WHEN PU.Role IS NULL THEN 2
												ELSE PU.Role
											 END Role 
											,U.Name
											,P.Title
											,ISNULL(INV.Status, 0) Status
											,ISNULL(
												CASE 
													WHEN PU.Role = 1 THEN
														(SELECT (COUNT(*)-1) FROM ProjectUsers sPU WHERE sPU.ProjectId = PU.ProjectId)
													WHEN PU.Role = 2 AND INV.Status = 2 THEN 1
													ELSE NULL
												  END, 0) TotalMembers 
											 ,ISNULL(
												 CASE
													WHEN PU.Role = 1 THEN
														(SELECT CONVERT(FLOAT, CAST(ISNULL(SUM(DATEDIFF(SECOND, sA.StartTime, sA.EndTime) / 3600.0), 0) AS decimal(18,2))) FROM Activities sA WHERE sA.ProjectId = PU.ProjectId)
													WHEN PU.Role = 2 AND INV.Status = 2 THEN
														(SELECT CONVERT(FLOAT, CAST(ISNULL(SUM(DATEDIFF(SECOND, sA.StartTime, sA.EndTime) / 3600.0), 0) AS decimal(18,2))) FROM Activities sA WHERE sA.ApplicationUserId = PU.ApplicationUserId AND sA.ProjectId = PU.ProjectId)
													ELSE NULL
												  END, 0) TotalLogHours
										FROM AspNetUsers U  
										LEFT JOIN Invitations INV ON U.Email = INV.Email AND INV.Status IN (1, 2)
										LEFT JOIN Projects P ON INV.ProjectId = P.Id
										LEFT JOIN ProjectUsers PU ON INV.ProjectId = PU.ProjectId AND U.Id = PU.ApplicationUserId
										WHERE U.Id = @xApplicationUserId ';

							IF @Role IS NOT NULL
								SET @sql = @sql + ' AND PU.Role = @xRole'; 

							IF @Title IS NOT NULL
								SET @sql = @sql + ' AND P.Title LIKE ''%'' + @xTitle + ''%''';

							IF @IsArchived IS NOT NULL
								SET @sql = @sql + ' AND P.IsArchived = @xIsArchived '

							IF @SearchText IS NOT NULL
								SET @sql = @sql + ' AND P.Title LIKE ''%'' + @xSearchText + ''%''';

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
								@xSearchText nvarchar(250),
								@TotalDisplay int output';

							exec sp_executesql @countsql, @countparamlist,
								@ApplicationUserId,
								@Role,
								@Title, 
								@IsArchived, 
								@SearchText,
								@TotalDisplay = @TotalDisplay output;

							SELECT @paramlist = '@xApplicationUserId uniqueidentifier,
								@xRole int,
								@xTitle nvarchar(250), 
								@xIsArchived bit, 
								@xSearchText nvarchar(250),
								@PageIndex int, 
								@PageSize int';

							exec sp_executesql @sql, @paramlist,
								@ApplicationUserId,
								@Role,
								@Title,
								@IsArchived, 
								@SearchText,
								@PageIndex,
								@PageSize;

							print @sql;
							print @totalSql;
							print @countSql;

						GO";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = @"DROP PROCEDURE [dbo].[sp_GetProjects]";
            migrationBuilder.Sql(sql);
        }
    }
}
