1. 	ohne anmeldung https://localhost:7209/
-	die sektion py-24 bg-slate-900 text-white overflow-hidden ist etwas zu dunkel
- 	die h3 class ist zu dunkel und hat in der sektion keinen kontrast
2. 	https://localhost:7209/Account/Register
-	die nutzer registrierung, muss alle relevanten felder des userprofils beinhalten (Firma (workplaces per dropdown), position, techstack, Ort, Straße, Straßennummer, land )
3.	nach der anmeldung https://localhost:7209/
-	firmen(workspaces) sind von einander getrennt nur user mit einer entsprechenden beziehung zu einer firma können entsprechende projekte, tickets, teams und user und statistiken sehen, Admin kann alles wie gewohnt sehen.
-	class="bg-slate-900 rounded-[2.5rem] p-12 text-white overflow-hidden relative group" ist zu dunkel, die class h2 ist auch zu dunkel und hat keinen kontrast.
- die navbar brauch einen neuen eintrag, Übersicht, Projekte, Tickets, Teams und Chat, Projekte soll die Projekte der Firma zeigen.
4.	https://localhost:7209/Teams
-	type="submit" Join request, muss anfragen an admins. teamlead stellen, die von diesen nutzern dann bestätigt oder abgelehnt werden
-	Admins und Teamleads bnrauchen hier einen BTN CreateTeam
5.	https://localhost:7209/Teams/Details/
-	Muss mehr details, performance metriken, schöne charts, pie charts, tickets und projekte sowie für admin/teamlead  (accept request, edit team(https://localhost:7209/Teams/Management) und kick teammember) für das team enthalten
6. 	https://localhost:7209/Tickets/Details/
-	blocker hinzufügen führt zu: An unhandled exception occurred while processing the request.
KeyNotFoundException: Ticket nicht gefunden.
TicketsPlease.Application.Services.TicketService.AddDependencyAsync(Guid ticketId, Guid blockerId) in TicketService.cs, line 259

Stack Query Cookies Headers Routing
KeyNotFoundException: Ticket nicht gefunden.
TicketsPlease.Application.Services.TicketService.AddDependencyAsync(Guid ticketId, Guid blockerId) in TicketService.cs
+
    var blocker = await _ticketRepository.GetByIdAsync(blockerId).ConfigureAwait(false) ?? throw new KeyNotFoundException(TicketNotFoundMessage);
TicketsPlease.Web.Controllers.TicketsController.AddDependency(Guid id, Guid blockerId) in TicketsController.cs
+
      await this.ticketService.AddDependencyAsync(id, blockerId).ConfigureAwait(false);
Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor+TaskOfIActionResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, object controller, object[] arguments)
Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask<IActionResult> actionResultValueTask)
Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, object state, bool isCompleted)
Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)
Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(ref State next, ref Scope scope, ref object state, ref bool isCompleted)
Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, object state, bool isCompleted)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeNextResourceFilter>g__Awaited|25_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, object state, bool isCompleted)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Rethrow(ResourceExecutedContextSealed context)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.Next(ref State next, ref Scope scope, ref object state, ref bool isCompleted)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, object state, bool isCompleted)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)
Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)
Microsoft.AspNetCore.Authentication.AuthenticationMiddleware.Invoke(HttpContext context)
Microsoft.AspNetCore.Localization.RequestLocalizationMiddleware.Invoke(HttpContext context)
Microsoft.AspNetCore.Diagnostics.DeveloperExceptionPageMiddlewareImpl.Invoke(HttpContext context)

Show raw exception details
-	ich möchte blocker bestimmen können, achte auf circular dependencies
-	tickets brauchen ein start und ein optionales end datum, denke an SLA-Agreements und eine Blinkende Warnung für Eskalierte Tickets.
7.	https://localhost:7209/Admin/Settings
-	jeder User soll settings haben, gestallte universelle einstellungen, performance einstellungen ( wie zum beispiel update zeiten im kanbanaboard wenn man tickets verschiebt auf eine ander list verschiebt),notification settings (verschiedene töne, usw) spezielle admin settings wie das löschen der database, jeder einzelnen table der database usw
8.	https://localhost:7209/Tickets/Edit/
-	tickets brauchen ein start und ein optionales end datum, denke an SLA-Agreements und eine Blinkende Warnung für Eskalierte Tickets, die auch im Kanban sichtbar ist. SLA-Eskalierte tickets müssen den Teamlead benachrichten.
9.	https://localhost:7209/Tickets/Create
-	tickets brauchen ein start und ein optionales end datum, denke an SLA-Agreements und eine Blinkende Warnung für Eskalierte Tickets, die auch im Kanban sichtbar ist. SLA-Eskalierte tickets müssen den Teamlead benachrichten.
10.	https://localhost:7209/AdminWorkspaces/Edit/
-	wir brauchen mehr settings, wie service level Agreements...
11. generelles
-	Migrate entirely to HTTPS to have cookies sent to same-site subresources
-	i18n für alle seiten und unterseiten
-	Unload event listeners are deprecated and will be removed.

1 source
browserLink:4
-	