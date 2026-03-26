# Summary

|||
|:---|:---|
| Generated on: | 26.03.2026 - 13:50:46 |
| Coverage date: | 26.03.2026 - 09:30:33 - 26.03.2026 - 13:48:43 |
| Parser: | MultiReport (9x Cobertura) |
| Assemblies: | 4 |
| Classes: | 92 |
| Files: | 91 |
| **Line coverage:** | 26.2% (769 of 2925) |
| Covered lines: | 769 |
| Uncovered lines: | 2156 |
| Coverable lines: | 2925 |
| Total lines: | 7749 |
| **Branch coverage:** | 10.4% (107 of 1024) |
| Covered branches: | 107 |
| Total branches: | 1024 |
| **Method coverage:** | [Feature is only available for sponsors](https://reportgenerator.io/pro) |

# Risk Hotspots

| **Assembly** | **Class** | **Method** | **Crap Score** | **Cyclomatic complexity** |
|:---|:---|:---|---:|---:|
| TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | TransformAsync(...) | 8930 | 94 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | TransformAsync(...) | 8930 | 94 || TicketsPlease.Web | AspNetCoreGeneratedDocument.Views_Tickets_Details | ExecuteAsync() | 4422 | 66 || TicketsPlease.Application | TicketsPlease.Application.Services.TicketService | MapToDto(...) | 2352 | 48 || TicketsPlease.Web | AspNetCoreGeneratedDocument.Views_Tickets_Index | <ExecuteAsync() | 1980 | 44 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | TransformAsync(...) | 1190 | 34 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | TransformAsync(...) | 1190 | 34 || TicketsPlease.Web | AspNetCoreGeneratedDocument.Views_Shared__KanbanCard | ExecuteAsync() | 930 | 30 || TicketsPlease.Infrastructure | TicketsPlease.Infrastructure.Persistence.DbInitialiser | SeedAsync() | 812 | 28 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | GetTypeDocId(...) | 812 | 28 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | GetTypeDocId(...) | 812 | 28 || TicketsPlease.Web | AspNetCoreGeneratedDocument.Views_Shared__Navbar | ExecuteAsync() | 563 | 46 || TicketsPlease.Web | AspNetCoreGeneratedDocument.Views_Shared__Notification | ExecuteAsync() | 542 | 58 || TicketsPlease.Web | AspNetCoreGeneratedDocument.Views_Project_Index | ExecuteAsync() | 420 | 20 || TicketsPlease.Web | AspNetCoreGeneratedDocument.Views_Project_Details | ExecuteAsync() | 342 | 18 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | CreateDocumentationId(...) | 342 | 18 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | CreateDocumentationId(...) | 342 | 18 || TicketsPlease.Web | AspNetCoreGeneratedDocument.Views_Shared__PriorityIcon | ExecuteAsync() | 272 | 16 || TicketsPlease.Web | AspNetCoreGeneratedDocument.Views_Shared__StatusBadge | ExecuteAsync() | 272 | 16 || TicketsPlease.Web | AspNetCoreGeneratedDocument.Views_Tickets_Index | ExecuteAsync() | 272 | 16 || TicketsPlease.Web | AspNetCoreGeneratedDocument.Views_Shared__TicketCard | ExecuteAsync() | 156 | 12 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | CreateDocumentationId(...) | 110 | 10 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | CreateDocumentationId(...) | 110 | 10 || TicketsPlease.Domain | TicketsPlease.Domain.Entities.Ticket | Close(...) | 72 | 8 || TicketsPlease.Web | TicketsPlease.Web.Controllers.TicketsController | PrepareViewBags() | 72 | 8 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | NormalizeDocId(...) | 42 | 6 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | UnwrapOpenApiParameter(...) | 42 | 6 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | NormalizeDocId(...) | 42 | 6 || TicketsPlease.Web | Microsoft.AspNetCore.OpenApi.Generated | UnwrapOpenApiParameter(...) | 42 | 6 || TicketsPlease.Web | TicketsPlease.Web.Controllers.AccountController | Register() | 42 | 6 |
# Coverage

| **Name** | **Covered** | **Uncovered** | **Coverable** | **Total** | **Line coverage** | **Covered** | **Total** | **Branch coverage** |
|:---|---:|---:|---:|---:|---:|---:|---:|---:|
| **TicketsPlease.Application** | **133** | **192** | **325** | **695** | **40.9%** | **22** | **106** | **20.7%** |
| TicketsPlease.Application.Common.Dtos.CommentDto | 0 | 6 | 6 | 22 | 0% | 0 | 0 |  |
| TicketsPlease.Application.Common.Dtos.CreateCommentDto | 3 | 0 | 3 | 16 | 100% | 0 | 0 |  |
| TicketsPlease.Application.Common.Dtos.CreateProjectDto | 0 | 5 | 5 | 20 | 0% | 0 | 0 |  |
| TicketsPlease.Application.Common.Dtos.CreateTicketDto | 7 | 0 | 7 | 24 | 100% | 0 | 0 |  |
| TicketsPlease.Application.Common.Dtos.ProjectDto | 3 | 5 | 8 | 26 | 37.5% | 0 | 0 |  |
| TicketsPlease.Application.Common.Dtos.TicketDto | 0 | 16 | 16 | 43 | 0% | 0 | 0 |  |
| TicketsPlease.Application.Common.Dtos.TicketLinkDto | 0 | 8 | 8 | 27 | 0% | 0 | 0 |  |
| TicketsPlease.Application.Common.Dtos.TicketPriorityDto | 0 | 4 | 4 | 18 | 0% | 0 | 0 |  |
| TicketsPlease.Application.Common.Dtos.UpdateProjectDto | 0 | 7 | 7 | 24 | 0% | 0 | 0 |  |
| TicketsPlease.Application.Common.Dtos.UpdateTicketDto | 3 | 5 | 8 | 26 | 37.5% | 0 | 0 |  |
| TicketsPlease.Application.Services.CommentService | 23 | 13 | 36 | 82 | 63.8% | 3 | 10 | 30% |
| TicketsPlease.Application.Services.ProjectService | 19 | 41 | 60 | 124 | 31.6% | 1 | 10 | 10% |
| TicketsPlease.Application.Services.TicketService | 75 | 82 | 157 | 243 | 47.7% | 18 | 86 | 20.9% |
| **TicketsPlease.Domain** | **159** | **255** | **414** | **2066** | **38.4%** | **8** | **50** | **16%** |
| TicketsPlease.Domain.Common.BaseAuditableEntity | 2 | 2 | 4 | 33 | 50% | 0 | 0 |  |
| TicketsPlease.Domain.Common.BaseEntity | 4 | 6 | 10 | 72 | 40% | 0 | 0 |  |
| TicketsPlease.Domain.Common.ValueObject | 0 | 12 | 12 | 41 | 0% | 0 | 6 | 0% |
| TicketsPlease.Domain.Entities.Comment | 17 | 10 | 27 | 89 | 62.9% | 1 | 4 | 25% |
| TicketsPlease.Domain.Entities.CustomFieldDefinition | 0 | 3 | 3 | 28 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.FileAsset | 0 | 7 | 7 | 49 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.Message | 0 | 11 | 11 | 69 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.MessageReadReceipt | 0 | 5 | 5 | 39 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.Notification | 0 | 7 | 7 | 51 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.Organization | 3 | 0 | 3 | 28 | 100% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.Project | 28 | 16 | 44 | 144 | 63.6% | 2 | 4 | 50% |
| TicketsPlease.Domain.Entities.Role | 3 | 7 | 10 | 70 | 30% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.SlaPolicy | 0 | 4 | 4 | 34 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.SubTicket | 0 | 7 | 7 | 49 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.Tag | 0 | 2 | 2 | 23 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.Team | 0 | 7 | 7 | 50 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.TeamMember | 0 | 6 | 6 | 44 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.Ticket | 70 | 67 | 137 | 419 | 51% | 5 | 34 | 14.7% |
| TicketsPlease.Domain.Entities.TicketAssignment | 0 | 7 | 7 | 49 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.TicketCustomValue | 0 | 5 | 5 | 39 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.TicketHistory | 0 | 8 | 8 | 54 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.TicketLink | 10 | 4 | 14 | 61 | 71.4% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.TicketPriority | 3 | 0 | 3 | 28 | 100% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.TicketTag | 0 | 4 | 4 | 34 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.TicketTemplate | 0 | 6 | 6 | 44 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.TicketUpvote | 0 | 5 | 5 | 39 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.TimeLog | 0 | 8 | 8 | 54 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.User | 7 | 10 | 17 | 107 | 41.1% | 0 | 2 | 0% |
| TicketsPlease.Domain.Entities.UserAddress | 0 | 6 | 6 | 44 | 0% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.UserProfile | 4 | 6 | 10 | 64 | 40% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.Workflow | 3 | 0 | 3 | 30 | 100% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.WorkflowState | 5 | 1 | 6 | 43 | 83.3% | 0 | 0 |  |
| TicketsPlease.Domain.Entities.WorkflowTransition | 0 | 6 | 6 | 44 | 0% | 0 | 0 |  |
| **TicketsPlease.Infrastructure** | **336** | **215** | **551** | **942** | **60.9%** | **20** | **50** | **40%** |
| TicketsPlease.Infrastructure.Persistence.AppDbContext | 218 | 25 | 243 | 392 | 89.7% | 10 | 12 | 83.3% |
| TicketsPlease.Infrastructure.Persistence.DbInitialiser | 0 | 190 | 190 | 269 | 0% | 0 | 28 | 0% |
| TicketsPlease.Infrastructure.Repositories.CommentRepository | 18 | 0 | 18 | 55 | 100% | 0 | 0 |  |
| TicketsPlease.Infrastructure.Repositories.ProjectRepository | 28 | 0 | 28 | 70 | 100% | 0 | 0 |  |
| TicketsPlease.Infrastructure.Repositories.TicketRepository | 69 | 0 | 69 | 129 | 100% | 10 | 10 | 100% |
| TicketsPlease.Infrastructure.Services.DefaultCorporateSkinProvider | 3 | 0 | 3 | 27 | 100% | 0 | 0 |  |
| **TicketsPlease.Web** | **141** | **1494** | **1635** | **4069** | **8.6%** | **57** | **818** | **6.9%** |
| AspNetCoreGeneratedDocument.Views__ViewStart | 1 | 0 | 1 | 3 | 100% | 0 | 0 |  |
| AspNetCoreGeneratedDocument.Views_Account_Login | 0 | 1 | 1 | 130 | 0% | 0 | 4 | 0% |
| AspNetCoreGeneratedDocument.Views_Account_Profile | 0 | 1 | 1 | 46 | 0% | 0 | 0 |  |
| AspNetCoreGeneratedDocument.Views_Account_Register | 0 | 1 | 1 | 128 | 0% | 0 | 4 | 0% |
| AspNetCoreGeneratedDocument.Views_Admin_Settings | 0 | 1 | 1 | 23 | 0% | 0 | 0 |  |
| AspNetCoreGeneratedDocument.Views_Admin_Users | 0 | 1 | 1 | 25 | 0% | 0 | 2 | 0% |
| AspNetCoreGeneratedDocument.Views_Home_Impressum | 0 | 1 | 1 | 66 | 0% | 0 | 0 |  |
| AspNetCoreGeneratedDocument.Views_Home_Index | 1 | 0 | 1 | 150 | 100% | 2 | 2 | 100% |
| AspNetCoreGeneratedDocument.Views_Home_Privacy | 0 | 1 | 1 | 73 | 0% | 0 | 0 |  |
| AspNetCoreGeneratedDocument.Views_Project_Create | 0 | 1 | 1 | 64 | 0% | 0 | 4 | 0% |
| AspNetCoreGeneratedDocument.Views_Project_Details | 0 | 1 | 1 | 67 | 0% | 0 | 18 | 0% |
| AspNetCoreGeneratedDocument.Views_Project_Edit | 0 | 1 | 1 | 70 | 0% | 0 | 4 | 0% |
| AspNetCoreGeneratedDocument.Views_Project_Index | 0 | 9 | 9 | 81 | 0% | 0 | 20 | 0% |
| AspNetCoreGeneratedDocument.Views_Shared__Avatar | 0 | 2 | 2 | 11 | 0% | 0 | 4 | 0% |
| AspNetCoreGeneratedDocument.Views_Shared__KanbanCard | 0 | 4 | 4 | 32 | 0% | 0 | 30 | 0% |
| AspNetCoreGeneratedDocument.Views_Shared__Navbar | 6 | 10 | 16 | 134 | 37.5% | 11 | 46 | 23.9% |
| AspNetCoreGeneratedDocument.Views_Shared__Notification | 10 | 11 | 21 | 49 | 47.6% | 26 | 58 | 44.8% |
| AspNetCoreGeneratedDocument.Views_Shared__PriorityIcon | 0 | 25 | 25 | 37 | 0% | 0 | 16 | 0% |
| AspNetCoreGeneratedDocument.Views_Shared__StatusBadge | 0 | 13 | 13 | 22 | 0% | 0 | 16 | 0% |
| AspNetCoreGeneratedDocument.Views_Shared__TicketCard | 0 | 3 | 3 | 35 | 0% | 0 | 12 | 0% |
| AspNetCoreGeneratedDocument.Views_Shared_Error | 0 | 4 | 4 | 25 | 0% | 0 | 2 | 0% |
| AspNetCoreGeneratedDocument.Views_Tickets_Create | 0 | 1 | 1 | 67 | 0% | 0 | 4 | 0% |
| AspNetCoreGeneratedDocument.Views_Tickets_Details | 0 | 36 | 36 | 212 | 0% | 0 | 66 | 0% |
| AspNetCoreGeneratedDocument.Views_Tickets_Edit | 0 | 1 | 1 | 71 | 0% | 0 | 4 | 0% |
| AspNetCoreGeneratedDocument.Views_Tickets_Index | 0 | 25 | 25 | 156 | 0% | 0 | 60 | 0% |
| Microsoft.AspNetCore.OpenApi.Generated | 7 | 1078 | 1085 | 1214 | 0.6% | 0 | 376 | 0% |
| Program | 102 | 14 | 116 | 175 | 87.9% | 7 | 10 | 70% |
| System.Runtime.CompilerServices | 0 | 3 | 3 | 23 | 0% | 0 | 0 |  |
| TicketsPlease.Web.Controllers.AccountController | 0 | 54 | 54 | 147 | 0% | 0 | 12 | 0% |
| TicketsPlease.Web.Controllers.AdminController | 0 | 9 | 9 | 46 | 0% | 0 | 0 |  |
| TicketsPlease.Web.Controllers.Api.TicketsApiController | 0 | 13 | 13 | 54 | 0% | 0 | 2 | 0% |
| TicketsPlease.Web.Controllers.CommentController | 0 | 18 | 18 | 57 | 0% | 0 | 2 | 0% |
| TicketsPlease.Web.Controllers.HomeController | 3 | 9 | 12 | 56 | 25% | 0 | 4 | 0% |
| TicketsPlease.Web.Controllers.ProjectController | 0 | 44 | 44 | 135 | 0% | 0 | 8 | 0% |
| TicketsPlease.Web.Controllers.StyleguideController | 0 | 3 | 3 | 24 | 0% | 0 | 0 |  |
| TicketsPlease.Web.Controllers.TicketsController | 0 | 84 | 84 | 223 | 0% | 0 | 16 | 0% |
| TicketsPlease.Web.InternalControllerFeatureProvider | 11 | 2 | 13 | 39 | 84.6% | 11 | 12 | 91.6% |
| TicketsPlease.Web.Models.Account.LoginViewModel | 0 | 3 | 3 | 33 | 0% | 0 | 0 |  |
| TicketsPlease.Web.Models.Account.RegisterViewModel | 0 | 4 | 4 | 45 | 0% | 0 | 0 |  |
| TicketsPlease.Web.Models.ErrorViewModel | 0 | 2 | 2 | 21 | 0% | 0 | 0 |  |

