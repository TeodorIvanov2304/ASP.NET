/*
Updated all packages to .NET 8, also changed  <TargetFramework>net6.0</TargetFramework> to 8.0

 After model creating, migration and DB update, we create GameController
 
Правим IActionResult методи за всички View , Например:

/Game/All (logged-in user, publisher of a specific game)
/Game/All (logged-in user, not publisher of any game)
/Game/MyZone (logged-in user)

За да видим, какво трябва да визуализира View модела, например за All IActionResult, отиваме на Views/Game/All и пишем ctrl + f e.

След като напишем IActionResult All, отиваме на Views/Game/All и разкоментираме , освен това и добавяме модела GameInfoViewModel най-отгоре

Отиваме на Areas/Identity/Pages/Account/Login.cshtml в метода 
public async Task<IActionResult> OnPostAsync(string returnUrl = null) на If-statement-a
if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }

И променяме return LocalRedirect(returnUrl); на return RedirectToAction("All","Game");

Така , когато се логнем ще отиваме на странциата All


Продължаваме с Add IActionResult, като отиваме на Views/Game/Add.cshtml и пишем @model GameViewModel най-отгоре. Съзаваме съответният модел, който ще използваме като DTO, за да вкарваме модели в базата. Хубаво е да се напишат и Message, като DataAnnotations 
*/