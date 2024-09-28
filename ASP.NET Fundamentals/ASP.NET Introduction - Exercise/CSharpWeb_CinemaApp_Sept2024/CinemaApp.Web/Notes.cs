/*
Първо, създаваме нов проект към solution-a oт тип class Library : CinemaApp.Web.ViewModels. Изтриваме първоначалният клас class1 и местим ErrorViewModel на негово място.Трием папката Models. Цъкаме на крушката горе до namespace и променяме namespace да съответства на новият път.Даваме дясно копче на CinemaApp.Web Dependencies и му даваме add project reference на CinemaApp.Web.ViewModels. Поправяме навсякъе namespace на CinemaApp.Web.ViewModels.
Махаме private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

от HomeControler. Оставяме само public HomeController()
		                                {
			
		                                }
 
Оправяме namespace в .cshtml файловете. Добавяме в HomeControlet Index Method ViewBag.Title и ViewBag.Message
После отиваме в Index.cshtml, изтриваме всичко и пишем:
<h2>@ViewData["Title"]</h2>
<h3>@ViewData["Message"]</h3>
Добавяме още 2 параграфа с текст <p>Tekst</p>
Трием методите Privacy и Error в HomeControler
Отиваме в _Layout и трием Privacy

                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
                        </li
Променяме <title>@ViewData["Title"] - CinemaApp.Web</title> на <title>@ViewData["Title"]<title>
Проемняме <a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">CinemaApp.Web</a> на 
<a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">CinemaWebApp</a>
Променяме <footer class= като премахваме Privacy и го правим на 
<footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2024 - CinemaWebApp
        </div>

Добавяме нов проект class Library CinemaApp.Common и преименуваме класа на ApplicationConstants, който е static
Добавяме променлива public const int ReleaseYear = 2024;
Правим dependency към новият проект CinemaApp.Common. Отиваме най-отгоре в Layout и пишем код, така че годината да се сменя:
@using static CinemaApp.Common.ApplicationConstants
@{
	int currentYear = DateTime.Now.Year;
	string footerNote = (ReleaseYear == currentYear) ? 
	                                $"{currentYear}" : 
									$"{ReleaseYear} - {currentYear}";
}

После долу във footer променяме 2024 с @footerNote

h2 -> Header размер 2
h3 -> Header размер 3
p -> Параграф

************************
Започваме DB

Създаваме нов проект class Library CinemaApp.Data.Models
Цъкаме дясно копче на Solution и правим нов Solution folder, в който местим CinemaApp.Web и CinemaApp.Web.ViewModels
Правим още 1 Solution Folder - Data и местим там моделите

Започваме класa Movie
Правим Id-то Guid вместо int!
Инциализираме го в конструктора Id = Guid.NewGuid();
Довършваме класа, засега не слагаме атрибути

Създаваме нов проект Library CinemaApp.Data в папката Data
Трием Class1 и инсталираме пакетите 
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore.Design

На CinemaApp.Data - Dependencies свързваме с CinemaApp.Data.Models
Правим CinemaDbContext class в CinemaApp.Data 
Създаваме му два конструктора, един празен:

public CinemaDbContext()
        {
            
        }

И един, който приема options

public CinemaDbContext(DbContextOptions options) : base(options)
		{

		}

Добавяме public virtual DbSet<Movie> Movies { get; set; } = null!;
Правим папка Configuration в CinemaApp.Data
Добавяме в нея клас MoveConfiguration, която наследява IEntityTypeConfiguration<Movie>
Имплементираме public void Configure метода с Fluent API

Правим нов клас в CinemaApp.Common с име EntityValidationConstants. Правим вътре статичен клас
public static class Movie и там слагаме константите като property-та.
Отиваме на CinemaApp.Data и правим dependency към Common

Отиваме на CinemaDbContext и override-ваме OnModelCreating

protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
		}

Връщаме се в MoveiConfiguration и правим метод за seed на базата (запълване)
private List<Movie> SeedMovies()
Създаваме n на бр. филма и връщаме List<Movies>
В Configure извикваме метода builder.HasData(this.SeedMovies()); за да попълним базата

Инсталираме в CinemaApp.Web пакетите Microsoft.Extensions.DependencyInjection и
Microsoft.EntityFrameworkCore.Tools
В CinemaApp.Web добавяме dependency към CinemaApp.Data
В Program.cs в container-a добавяме 

builder.Services.AddDbContext<CinemaDbContext>(options => 
			{
				options.UseSqlServer(connectionString);
			});

Отиваме в appsettings.Development.json и пишем

"ConnectionStrings": {
    "SQLServer": "Server=(localdb)\\MSSQLLocalDB;Database=CinemaApp2024;Trusted_Connection=True;TrustServerCertificate = true;"
  }

Отиваме в Program.cs и над containter-a пишем 
string connectionString = builder.Configuration.GetConnectionString("SQLServer"); и го подаваме на options

Съсдаваме първата миграция в Package Manager Console:
Сменяме Default project на CinemaApp.Data
Add-Migration InitialMigration
Update-Database

Отиваме на Controler и добавяме нов контролер MVC-Controler Empty, и го кръщаваме MovieControler
Създаваме field private readonly CinemaDbContext _dbContext; и го инициализираме с Depenency Injection
Инициализираме колеккция, която ще пази филмите, отново без ключова дума new()
IEnumerable<Movie> movies = this._dbContext.Movies.ToList(); 
Слагаме на метода атрибут [HttpGet] отгоре. [HttpGet] казва на метода да се използва тогава, когато има GET заявка.

Във Views правим нова папка, която се казва Movie, т.е като контролера, без контролер! В нея правим празно View, което кръщаваме с името на метода, т.е Index
Отваряме файла с кода за View-то, който е в документа към лекцията от СофтУни.Копираме кода и го пействаме в View-то и оправяме using-a
Редът @model IEnumerable<Movie> оказва какъв тип данни получава View-то
Като напишем в конзолата https://localhost:7191/Movie би трябвало да излиза страница с филми

Създаваме ново View - Create в MovieControler
Създаваме нова форма в папкa Views - Create
Копираме кода от формата в документа и оправяме using на @using CinemaApp.Data.Models;
Пишем @model Movie защото това е типа, който ще връща формата

Правим нов IActionResult Метод в MovieControler - Create(Movie movie) - ОВЪРЛОАД на метода
Само че този метод ще има атрибут [HttpPost]

[HttpPost]
		public IActionResult Create(Movie movie)
		{
			
		}

Допълваме филма в базата и сейваме промените
_dbContext.Movies.Add(movie);
_dbContext.SaveChanges();
ТУК ПО ПРИНЦИП ТРЯБВА ДА ИМА ВАЛИДАЦИЯ, ЗАЩОТО МОЖЕ ДА ВКАРАМЕ ГРЕШНИ ДАННИ В БАЗАТА!

Накрая връщаме вместо View, пренасочване към метода IActionResult Index
return RedirectToAction(nameof(Index));
Добавяме нов филм през сайта за проба

Правим нов метод в MovieControler с атрибут [HttpGet] Details, който приема id на филма
string id
Проверяваме, дали id е валидно
bool isValidId = Guid.TryParse(id, out Guid guidId);
Правим проверки, дали Id-то е валидно и дали съществува в базата данни
Ако всичко е наред, връщаме View(movie)
Добавяме ново View в Views/Movie
Копираме кода на View от документа
Оправяме @using и правим да връща @model Movie

Отиваме на Views/Shared/_Layout
Това е нав-бара:

<div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul class="navbar-nav flex-grow-1">
                        <li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
                        </li>
                    </ul>
Копираме целия <li>(лист итем)
<li class="nav-item">
                            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
                        </li>
Добавяме го отдолу и проемняме Home на Movie и следващото Home на Movies

<li class="nav-item">
							<a class="nav-link text-dark" asp-area="" asp-controller="Movie" asp-action="Index">Movies</a>
						</li>
Така добавяме бутона Movies в Navigation bar

Отиваме на CinemaApp.Web.ViewModels и правим папка Movie. Вътре правим клас AddMovieInputModel(DTO?) с който ще валидираме Movie, които потребителя ще добавя.
Добравяме на CinemaApp.Web.ViewModels dependency към Common. От там ще изпозлваме EntityValidationConstants
Правим пропъртита същите като на Movie и им слагаме атрибути.
За ReleaseDate използваме string, вместо DateTime. Допълваме EntityValidationConstants, когато се налага
Отиваме на Views/Movie/Create и променяме using на CinemaApp.Web.ViewModels.Movie
Променяме @model Movie на @model AddMovieInputModel
Добавяме за всяко пропърти
<div>
		<label for="Title">Movie Name</label>
		<input asp-for="Title" class="form-control" />
	</div>

Този код: <span asp-validation-for=Title></span>

<div>
		<label for="Title">Movie Name</label>
		<input asp-for="Title" class="form-control" />
        <span asp-validation-for=Title></span>
	</div>

Отиваме на MovieControler и променяме типа на метода public IActionResult Create(Movie movie)
на public IActionResult Create(AddMovieInputModel inputModel)

Правим проверка, дали пропъртитата са валидни:
Ако са невалидни, връщаме същото View, със съответните грешки

if (this.ModelState.IsValid)
			{
				//Render the same form with user entered values + model errors
				return View(movie);
			}

Проверяваме дали датата е валидна с bool и TryParse
bool isReleaseDateValid = DateTime.TryParseExact(inputModel.ReleaseDate,"dd/MM/yyyy",CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);

if (!isReleaseDateValid)
			{
				this.ModelState.AddModelError(nameof(inputModel.ReleaseDate), "The Release Date must be in the following format: dd/MM/yyyy");
				return this.View(inputModel);
			}

Ако всички валидации минат, създаваме нов филм, като за стойности добавяме тези на inputModel + releaseDate от TryParse

Поправяме всички пропъртита в Views/Movie/Create на 
<span asp-validation-for=Title class="text-danger"></span>

"text-danger" е bootstrap, който ще промени текста на червен


 */