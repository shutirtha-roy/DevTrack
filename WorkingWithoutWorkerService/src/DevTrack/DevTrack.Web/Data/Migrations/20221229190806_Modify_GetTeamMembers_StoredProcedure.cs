using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevTrack.Web.Data.Migrations
{
    public partial class Modify_GetTeamMembers_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"CREATE OR ALTER PROCEDURE [dbo].[sp_GetTeamMembers]
							@PageIndex int,
							@PageSize int,
							@SearchText nvarchar(250) = '%',
							@OrderBy nvarchar(50),
							@ApplicationUserId uniqueidentifier,
							@ProjectId uniqueidentifier,
							@CurrentDate DateTime,
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
							Declare @role int;
							Declare @firstDateOfWeek DateTime = DATEADD(DAY, -7, @CurrentDate);
							Declare @month int = Month(@CurrentDate);

							SET NOCOUNT ON;

							SELECT  @role = [Role] FROM ProjectUsers WHERE ProjectId = @ProjectId AND ApplicationUserId = @ApplicationUserId;

							SET @totalSql = 'SELECT @Total = Count(*) FROM ProjectUsers WHERE ProjectId = @xProjectId';
							SET @countSql = 'SELECT @TotalDisplay = Count(*) FROM ProjectUsers PU 
											 LEFT JOIN AspNetUsers AU ON PU.ApplicationUserId = AU.Id 
											 WHERE ProjectId = @xProjectId';

							IF @role <> 1
								BEGIN
									SET @totalSql = @totalSql + ' AND ApplicationUserId = @xApplicationUserId';
									SET @countSql = @countSql + ' AND ApplicationUserId = @xApplicationUserId';
								END
							END

							IF @SearchText IS NOT NULL
								SET @countsql = @countSql + ' AND Name LIKE ''%'' + @xSearchText + ''%''';

							SET @sql = 'SELECT 
											 PU.ApplicationUserId
											,PU.ProjectId
											,U.Name
											,U.Image
											,(SELECT 
												CONVERT(FLOAT, CAST(ISNULL(SUM(DATEDIFF(SECOND, sA.StartTime, sA.EndTime) / 3600.0), 0) AS decimal(18,2))) 
											 FROM Activities sA 
											 WHERE sA.ProjectId = PU.ProjectId AND ApplicationUserId = PU.ApplicationUserId AND sA.StartTime >= @xFirstDateOfWeek) LoggedPastWeek
											,(SELECT 
												CONVERT(FLOAT, CAST(ISNULL(SUM(DATEDIFF(SECOND, sA.StartTime, sA.EndTime) / 3600.0), 0) AS decimal(18,2))) 
											 FROM Activities sA 
											 WHERE sA.ProjectId = PU.ProjectId AND ApplicationUserId = PU.ApplicationUserId AND Month(sA.StartTime) = @xMonth) LoggedThisMonth
											,(SELECT 
												CONVERT(FLOAT, CAST(ISNULL(SUM(DATEDIFF(SECOND, sA.StartTime, sA.EndTime) / 3600.0), 0) AS decimal(18,2))) 
											 FROM Activities sA 
											 WHERE sA.ProjectId = PU.ProjectId AND ApplicationUserId = PU.ApplicationUserId) TotalLogged
										FROM ProjectUsers PU
										LEFT JOIN AspNetUsers U ON PU.ApplicationUserId = U.Id
										LEFT JOIN Projects P ON PU.ProjectId = P.Id
										LEFT JOIN Invitations INV ON PU.ProjectId = INV.ProjectId AND U.Email = INV.Email
										WHERE PU.ProjectId = @xProjectId ';

							IF @SearchText IS NOT NULL
								SET @sql = @sql + ' AND U.Name LIKE ''%'' + @xSearchText + ''%''';

							IF @role <> 1
								SET @sql = @sql + ' AND PU.ApplicationUserId = @xApplicationUserId';
							ELSE
								SET @sql = @sql + ' AND PU.Role <> 1'

							SET @sql = @sql + ' ORDER BY '+ @OrderBy +' OFFSET @PageSize * (@PageIndex - 1) 
								ROWS FETCH NEXT @PageSize ROWS ONLY';

							SELECT @totalparamlist = '@xApplicationUserId uniqueidentifier,
								@xProjectId uniqueidentifier,
								@Total int output';

							exec sp_executesql @totalsql, @totalparamlist,
								@ApplicationUserId,
								@ProjectId,
								@Total = @Total output;

							SELECT @countparamlist = '@xApplicationUserId uniqueidentifier,
								@xProjectId uniqueidentifier,
								@xSearchText nvarchar(250),
								@TotalDisplay int output';

							exec sp_executesql @countsql, @countparamlist,
								@ApplicationUserId,
								@ProjectId,
								@SearchText,
								@TotalDisplay = @TotalDisplay output;

							SELECT @paramlist = '@xApplicationUserId uniqueidentifier,
								@xProjectId uniqueidentifier,
								@xFirstDateOfWeek DateTime, 
								@xMonth int, 
								@xSearchText nvarchar(250),
								@PageIndex int, 
								@PageSize int';

							exec sp_executesql @sql, @paramlist,
								@ApplicationUserId,
								@ProjectId,
								@firstDateOfWeek,
								@month,
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
            var sql = @"DROP PROCEDURE [dbo].[sp_GetTeamMembers]";
            migrationBuilder.Sql(sql);
        }
    }
}
