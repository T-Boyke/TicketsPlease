# TODO List

- src\TicketsPlease.Web\Views\Shared\_CookieBanner.cshtml div id="cookie-banner" ist auf jeder seite/view und blockiert den inhalt der seite,
  muss nach zustimmung verschwinden
- in src\TicketsPlease.Web\Views\Messages\Conversation.cshtml ist die klasse:
  class="h-14 w-14 rounded-full bg-gradient-to-br from-brand-primary
  to-brand-accent flex items-center justify-center text-white text-xl
  font-black shadow-lg shadow-brand-primary/20 flex-shrink-0"
  Hier soll der nutzer den Avatar der person sehen, mit der er gerade chattet.
- in src\TicketsPlease.Web\Views\Messages\Create.cshtml wenn eine andere nutzergrupper als
  Admin, jemanden eine Nachricht schreibt, kommt es zu einem fehler:

  ```text
  An unhandled exception occurred while processing the request.

NullReferenceException: Object reference not set to an instance of an object.
TicketsPlease.Application.Services.MessageService.MapToDto(Message m) in MessageService.cs, line 148

Stack Query Cookies Headers Routing
NullReferenceException: Object reference not set to an instance of an object.
TicketsPlease.Application.Services.MessageService.MapToDto(Message m) in MessageService.cs
+
    var attachments = m.Attachments?.Select(a => new FileAssetDto(
TicketsPlease.Application.Services.MessageService.SendMessageAsync(Guid senderId, CreateMessageDto dto, CancellationToken ct) in MessageService.cs
+
    var mappedResult = MapToDto(savedMessage!);
TicketsPlease.Web.Controllers.MessagesController.Create(CreateMessageDto dto)
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

Show raw exception details"

```
- src\TicketsPlease.Web\Views\Stakeholder\Index.cshtml hat kein i18n in de und en
- src\TicketsPlease.Web\Views\AdminWorkspaces\Create.cshtml legt nach submit keinen neuen workspace an
- src\TicketsPlease.Web\Views\AdminWorkspaces\Edit.cshtml speichert nach submit keine änderungen
- src\TicketsPlease.Web\Views\Shared\_Navbar.cshtml btn onclick="toggleChat()" ist überflüssig und kann weg
- folgende fehler treten auf:
im haupt dashboard nach der anmeldung
```text
signalr.min.js:1 [2026-04-16T09:01:09.142Z] Information: Normalizing '/notificationHub' to 'https://localhost:7209/notificationHub'.
signalr.min.js:1 [2026-04-16T09:01:09.147Z] Information: Normalizing '/notificationHub' to 'https://localhost:7209/notificationHub'.
(index):971  GET https://localhost:7209/lib/chartjs/dist/chart.umd.js net::ERR_ABORTED 404 (Not Found)
(index):1 Refused to execute script from 'https://localhost:7209/lib/chartjs/dist/chart.umd.js' because its MIME type ('') is not executable, and strict MIME type checking is enabled.
signalr.min.js:1 [2026-04-16T09:01:10.613Z] Information: WebSocket connected to wss://localhost:7209/notificationHub?id=VaIlAIYS84a22tDO1fnBSQ.
signalr.min.js:1 [2026-04-16T09:01:10.947Z] Information: WebSocket connected to wss://localhost:7209/notificationHub?id=eUUWveX0T3tlXT543Ff2dg.
collaboration-client.js?v=_ezY920X4WPmP59rtlvlFzDeFD0ObiiCErdalTZghiU:135 Collaboration connection started
notification-client.js?v=aJqtpDf7FVmJrC5trRzGbEv4sPpehYWAudH1OBzT4J4:117 SignalR connected.
dashboard-charts.js?v=rlODhfDN43yR-zrRo3bJN2_9nv58Ot-2P3re9cbiiUQ:57 Error fetching performance details: ReferenceError: Chart is not defined
    at renderStatusChart (dashboard-charts.js?v=rlODhfDN43yR-zrRo3bJN2_9nv58Ot-2P3re9cbiiUQ:82:5)
    at openPerformanceDetail (dashboard-charts.js?v=rlODhfDN43yR-zrRo3bJN2_9nv58Ot-2P3re9cbiiUQ:53:9)
openPerformanceDetail @ dashboard-charts.js?v=rlODhfDN43yR-zrRo3bJN2_9nv58Ot-2P3re9cbiiUQ:57
await in openPerformanceDetail
onclick @ (index):336
dashboard-charts.js?v=rlODhfDN43yR-zrRo3bJN2_9nv58Ot-2P3re9cbiiUQ:57 Error fetching performance details: ReferenceError: Chart is not defined
    at renderStatusChart (dashboard-charts.js?v=rlODhfDN43yR-zrRo3bJN2_9nv58Ot-2P3re9cbiiUQ:82:5)
    at openPerformanceDetail (dashboard-charts.js?v=rlODhfDN43yR-zrRo3bJN2_9nv58Ot-2P3re9cbiiUQ:53:9)
openPerformanceDetail @ dashboard-charts.js?v=rlODhfDN43yR-zrRo3bJN2_9nv58Ot-2P3re9cbiiUQ:57
await in openPerformanceDetail
onclick @ (index):572
```
Als PO im kommt es zu folgendem Fehler:
```text
signalr.min.js:1 [2026-04-16T09:05:08.855Z] Information: Normalizing '/notificationHub' to 'https://localhost:7209/notificationHub'.
signalr.min.js:1 [2026-04-16T09:05:08.861Z] Information: Normalizing '/notificationHub' to 'https://localhost:7209/notificationHub'.
signalr.min.js:1 [2026-04-16T09:05:09.791Z] Information: WebSocket connected to wss://localhost:7209/notificationHub?id=qMcrHAWbjUBB81D9YxNx3w.
signalr.min.js:1 [2026-04-16T09:05:12.478Z] Information: WebSocket connected to wss://localhost:7209/notificationHub?id=XQu4veAeKnETRggEJrbpRw.
collaboration-client.js?v=_ezY920X4WPmP59rtlvlFzDeFD0ObiiCErdalTZghiU:135 Collaboration connection started
notification-client.js?v=aJqtpDf7FVmJrC5trRzGbEv4sPpehYWAudH1OBzT4J4:117 SignalR connected.
Tickets:1831  POST https://localhost:7209/Tickets/Move/a58b5f2e-d0af-450a-a854-6638289866d9?status=In%20Progress 500 (Internal Server Error)
onEnd @ Tickets:1831
G @ Sortable.min.js:2
V @ Sortable.min.js:2
_onDrop @ Sortable.min.js:2
handleEvent @ Sortable.min.js:2
Tickets:1839 Move failed
(anonymous) @ Tickets:1839
Promise.then
onEnd @ Tickets:1837
G @ Sortable.min.js:2
V @ Sortable.min.js:2
_onDrop @ Sortable.min.js:2
handleEvent @ Sortable.min.js:2
```
