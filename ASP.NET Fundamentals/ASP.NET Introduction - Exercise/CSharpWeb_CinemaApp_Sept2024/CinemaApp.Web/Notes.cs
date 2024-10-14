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

Отиваме на AddMovieInputModel и създаваме конструктор, който инициализира default дата, DateTime.Now
Изнасяме формата на датата в отделнат константа. DRY!
Отиваме на EntityValidationConstants и правим константа за формата.
public const string ReleaseDateFormat = "MMMM yyyy";
Оправяме и isReleaseDateValid Метода с новата променлива и String.Format

if (!isReleaseDateValid)
			{	
				//ModelState becomes invalid => isValid == false
				this.ModelState.AddModelError(nameof(inputModel.ReleaseDate), String.Format($"The Release Date must be in the following format: {0}", ReleaseDateFormat));
				return this.View(inputModel);
			}

Отиваме на Views/Movie/Create и променяме всичко редове, на които пише
label for= на <label asp-for=
Така формата ще валидира стойностите (model binder)


Отиваме на CinemaApp.Common и създваме нов клас EntityValidationMessages, в който правим константи на грешките, който ще връща EntityValidation-a
public static class EntityValidationMessages
	{
		public static class Movie 
		{

		}
	}

Отиваме на AddMoveiInputModel и на всички [Required] атрибути добавяме съответния Error message
[Required(ErrorMessage = GenreRequiredMessage)]

Отиваме на CinemaApp.Data.Models за да направим нов модел Cinema и съответните му пропъртита, като инициализираме Guid 
public Guid Id { get; set; } = Guid.NewGuid();
и public virtual ICollection<CinemaMovie> CinemaMovies { get; set; } = new HashSet<CinemaMovie>();


После правим мапинг таблица CinemaMovie. Тя може да се направи по следният начин с DataAnotations но в случая го правим с FluentAPI: 
    [Key, Column(Order = 0)]
    public int CinemaId { get; set; }

    [Key, Column(Order = 1)]
    public int MovieId { get; set; }

Правим релацията и от другата страна в клас Movie
public virtual ICollection<CinemaMovie> MoviesCinema { get; set; } = new HashSet<CinemaMovie>();

Отиваме на клас CinemaApp.Data и праивм нов клас CinemaMovieConfiguration, в който с FluentAPI ще опишем връзките между таблицате, вместо с Data Anotations(Attributes)

Правим CinemaConfiguration клас и имплементираме интерфейса, отново с FluentAPI
public class Cinema : IEntityTypeConfiguration<Cinema> 

Отиваме на CinemaApp.Common -> EntityValidationConstants и правим нов клас 
public static class Cinema
{}
в който ще въведем константите за Cinema. Въвеждаме ги и ги използваме в CinemaConifuguration
Пак в CinemaConfiguration правим нов клас, с който ще сийднем кина в базата
private IEnummerable<Cinema> SeedCinemas

Довършваме в Configure метода с FluentAPI сийдването на базата
builder.HasData(this.SeedCinemas());

Отиваме на CinemaDbContext и добавяаме новите DbSets за Movie и CinemaMovie

Отиваме на папка Web и добавяме нов проект class library CinemaApp.Web.Infrastructure
В него правим папка Extensions и добавяме нов клас ApplicationBuilderExtensions
public static class ApplicationBuilderExtensions
Правим от CinemaApp.Web към CinemaApp.Web.Infrastructure
Ще го използваме за да напишем extension method на IApplicationBuilder app , с който базате ще се сийдва автоматично
Така трябва да изглежда Extension methoda, използва се this!
public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
		{
			using IServiceScope serviceScope = app.ApplicationServices.CreateScope(); 
			CinemaDbContext dbContext = serviceScope.ServiceProvider.GetRequiredService<CinemaDbContext>()!;
			dbContext.Database.Migrate();

			return app;
		}

Отиваме на Program и добавяме extension methoda на app преди app.Run();
app.ApplyMigrations(); като оправяме using-a

Отиваме на PMC и пишем Add-Migration AddCinemasToMovies? като сменяме Default project на CinemaApp.Data
Правим Delete-Migration И се връщаме в CinemaMovieConfiguration и добавяме към FluentAPI 
.OnDelete(DeleteBehavior.Restrict);
Правим наново миграцията Add-Migration AddCinemasToMovies

Отиваме на CinemaApp.Data -> CinemaMovieConfiguration и правим Seed method

private IEnumerable<CinemaMovie> SeedCinemasMovies()
		{
			IEnumerable<CinemaMovie> cinemasMovies = new List<CinemaMovie>()
			{
				new CinemaMovie()
				{
					CinemaId = Guid.Parse("0D1B6F8F-1A3E-401B-8636-2FC5E99CCF30"),
					MovieId = Guid.Parse("7402D10C-DD3F-4085-B092-16814F552B7C")
				}
			};

			return cinemasMovies;
		}

Добавяме и builder.HasData(SeedCinemasMovies());


Отиваме на CinemaApp.Web -> Controllers и създаваме нов контролер CinemaController
Правим DI

Правим нова папка Cinema във Views, правим ново View Index и вътре пействаме даденият код от ресурса на СофтУни
Оправяме using-a:
@using CinemaApp.Web.ViewModels.Cinema
@model IEnumerable<CinemaIndexViewModel>
Отиваме на Index и на <img src="~/images/cinema-default.jpg"
подменяме с 
"https://www.cinemacity.bg/xmedia/img/10106/cinema_card.jpg"
Може да се изтегли картинката, и в CinemaApp.Web -> wwwroot се прави папка images и се чете от там файла


Отиваме на CinemaApp.Web.ViewModels и правим нова папка Cinema, в която ще пазим View-моделите на Cinema
Правим нов клас в папка Cinema -> CinemaIndexViewModel
Правим пропъртитата. Отиваме на EntitityValidationMessages и добавяме статичен клас Cinema, в който ще сложим константите за CinemaInexViewModel. В CinemaIndexViewModel оправяме атрибутите.

Отиваме на CinemaController и правим нов IActionResult Create
Отиване на View/Cinema и създаваме ново View Create, копираме кода от документа на СофтУни и го пействаме в Create.cshtml. Оправяме using:

@using CinemaApp.Web.ViewModels.Cinema;
@model AddCinemaFormModel

@{
	ViewData["Title"] = "Add Cinema";
}

Отиваме на CinemaApp.Web.ViewModels/Cinema и създаваме нов View model AddCinemaFormModel
Попълваме му пропъртитата и добавяме атрибутите с константи

Връщаме се на CinemaController и създаваме нов метод, който ще поства ново кино след креате

[HttpPost]
		public IActionResult Create(AddCinemaFormModel model)
		{
			if (!this.ModelState.IsValid)
			{
				return View(model);
			}

			Cinema cinema = new Cinema()
			{	
				Name = model.Name,
				Location = model.Location
			};

			dbContext.Cinemas.Add(cinema);
			dbContext.SaveChanges();

			return RedirectToAction(nameof(Index));
		}

Оправяме методите на CinemaControleer да са Async + await , asyn Task<IActionResult>

Отиваме във Views/Shared/_Layout и ще добавим Cinemas в navbar-a
<li 
class="nav-item">
<a class="nav-link text-dark" asp-area="" asp-controller="Cinema" asp-action="Index">Cinemas<a>
</li>

Отиваме на CinemaController и правим нов 
[HttpGet] 
public async Task <IActionResult> Details(string id)

Създаваме ново View в Cinema/Details в което копираме кода от документа

Съзаваме нов клас в CinemaApp.Web.ViewModels -> CinemaDetailsViewModel
На View model-и не е нужно да се правят фалидации, за разлика от на форм модели?

public class CinemaDetailsViewModel
	{
		public string Name { get; set; } = null!;
        public string Location { get; set; } = null!;
        public IEnumerable<CinemaMovieViewModel> Movies { get; set; } = new HashSet<CinemaMovieViewModel>();
    }

Отиваме на CinemaApp.Web.ViewModels/Movie и добавяме още един модел, който ще е вложен в CinemaDetailsViewModel,а именно колекция от филми ->  CinemaMovieViewModel

public class CinemaMovieViewModel
	{
		public string Title { get; set; } = null!;
        public int Duration { get; set; }
    }

Връщаме се в CinemaController и имплементираме логиката на Details, после я местим в отделен метод

Отиваме на Controller папката, и създаваме нов конролер - BaseController в който местим метода от CinemaController 

protected bool IsGuidValid(string? id, ref Guid cinemaGuid)
		{
			//Non-existing parameter in the URL
			if (String.IsNullOrWhiteSpace(id))
			{
				return false;
			}

			bool isGuidValid = Guid.TryParse(id, out cinemaGuid);


			//Invalid parameter in the URL
			if (!isGuidValid)
			{
				return false;
			}

			return true;
		}

Променяме му името на IsGuidValid и го преизползваме с наследяването на CinemaController : BaseController
Отиваме на MovieController и пренаследяваме и него с : BaseController за да използваме метода isGuidValid

Отиваме на Views/Movie/Index.cshtml и добавяме още един бутон:

<a asp-action="AddToProgram" asp-route-id="@movie.Id" class="btn btn-info">Add to Program</a>

Отиваме на MovieControler и правим нов IActionResult
[HttpGet]
public async Task<IActionResult> AddToProgram(string? id)

Отиваме на CinemaApp.Web.ViewModels и правим нов клас AddMovieToCinemaInputModel
Създаваме проъртитата, и после създаваме още един клас, този път в папка Cinema:
CinemaCheckBoxItemInputModel. Създаваме и неговите пропъртита, и се връщаме и добавяме колеция от CinemaCheckBoxItemInputModel в AddMovieToCinemaInputModel като пропърти.
Добавяме им атрибутите.

Връщаме се на MovieController и довършваме метода 
[HttpGet]
public async Task<IActionResult> AddToProgram(string? id)

Отиваме на Views/Movie и създаваме ново View AddToProgram, копираме кода от документа на СофтУни и оправяме usings-ите и променяме for-loop на foreach, като редактираме малко имената на променливите

Отиваме на MovieController и създаваме нов метод 
[HttpPost]
public async Task<IActionResult> AddToProgram(AddMovieToCinemaInputModel model)
Валидирамe и довършваме метода.Оставяме засега имеплементацията на триене
Отиваме на CinemaApp.Data.Models CinemaMovie и добавяаме пропърти 
public bool IsDeleted { get; set; }

Добавяме нова миграция към базата и ъпдейтваме.

Добавяне на Identity:
Отиваме на CinemaWeb.App и инсталираме следните пакети
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 8.0.10
Microsoft.AspNetCore.Identity.UI 
Добавяме Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore -Version 8.0.10 и към CinemaApp.Data и CinemaApp.Data.Models
Отиваме на CinemaApp.Data.Models и правим нов модел ApplicationUser

public class ApplicationUser : IdentityUser<Guid>
    {
		public ApplicationUser() 
		{
			this.Id = Guid.NewGuid();
		}
    }

Отиваме на CinemaDbContext и поправяме наследяването на 
public class CinemaDbContext : IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>
На метода OnModelCreating добавяме 
base.OnModelCreating(modelBuilder);
Отиваме на Program.cs и добавяме нов Service


Добавяме app.UseAuthentication(); и app.MapRazorPages

Отиваме на CinemaApp.Web , даваме дясно копче и натискаме Add/New Scaffoldet Item/ Identity и добавяме Account/Login, Account/Logout, Account/Register

Отиваме на Views/Shared/_ViewStart.cshtml и оправяме Layout - a
Layout = "Views/Shared/_Layout.cshtml";

Отиваме на Areas/Identity/Account/Login и сменяме всички IdentityUser на ApplicationUser
Правим същото на  Areas/Identity/Account/Logout и на Areas/Identity/Account/Register

Връщаме се на Program.cs и чейнваме още няколко метода в builder.Services.AddDefaultIdentity<ApplicationUser>
Изтриваме една маса неща от Register.cshtml свързани със sender email, най-вече защото не се е включило user identity в началото на проекта

builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(cfg =>
				{

				})
				.AddEntityFrameworkStores<CinemaDbContext>()
				.AddRoles<IdentityRole<Guid>>()
				.AddSignInManager<SignInManager<ApplicationUser>>()
				.AddUserManager<UserManager<ApplicationUser>>();

Отиваме на Views/Shared/_LoginPartial и променяме IdentityUser na ApplicationUser

@inject - правим Depenency Injection във View-to
Добавяме <partial name="_LoginPartial"/> в _Layout

Отиваме на User secrets и там ще изнесем настройките за паролата

{
  "Identity": {
    "Password": {
      "RequireDigits": true,
      "RequireLowercase": false,
      "RequireUppercase": false,
      "RequireNonAlphanumerical": false,
      "RequiredLength": 3,
      "RequiredUniqueCharacters": 0
    },
    "SignIn": {
      "RequireConfirmedAccount": false,
      "RequireConfirmedEmail": false,
      "RequireConfirmedPhoneNumber": false
    },
    "User": {
      "RequireUniqueEmail": true
    } 
  }
}

Изнасяме пропъртитата в отделен метод:

private static void ConfigureIdentity(WebApplicationBuilder builder, IdentityOptions cfg)
		{

			//Password
			cfg.Password.RequireDigit = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigits");
			cfg.Password.RequireLowercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
			cfg.Password.RequireUppercase = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
			cfg.Password.RequireNonAlphanumeric = builder.Configuration.GetValue<bool>("Identity:Password:RequireNonAlphanumerical");
			cfg.Password.RequiredLength = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
			cfg.Password.RequiredUniqueChars = builder.Configuration.GetValue<int>("Identity:Password:RequiredUniqueCharacters");

			//Sign in
			cfg.SignIn.RequireConfirmedAccount = builder.Configuration.GetValue<bool>("Identity:SingIn:RequireConfirmedAccount");
			cfg.SignIn.RequireConfirmedEmail = builder.Configuration.GetValue<bool>("Identity:SingIn:RequireConfirmedEmail");
			cfg.SignIn.RequireConfirmedPhoneNumber = builder.Configuration.GetValue<bool>("Identity:SingIn:RequireConfirmedPhoneNumber");

			//User
			cfg.User.RequireUniqueEmail = builder.Configuration.GetValue<bool>("Identity:SingIn:RequireUniqueEmail");
		}

Отиваме на Register.cshtml и премахваме всичко под 
<div class="col-md-6 col-md-offset-2"> 

до
   
</section>
    </div>

Отиваме на Register.cshtml.cs и изтриваме External login и всички неща свързани с него

Отиваме на Login.cshtml и изтриваме от 

 <div class="col-md-6 col-md-offset-2">

до

     </section>
    </div>

Отиваме на Login.cshtml.cs и изтриваме от External Logins и вскички свързани неща

Отиваме на Register и добавяме още едно пропърти

[Required]
			[StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
			[Display(Name = "Username")]
			public string Username { get; set; }

Добавяме  в Register.cshtml

<div class="form-floating mb-3">
				<input asp-for="Input.Username" class="form-control" autocomplete="username" aria-required="true" placeholder="name@example.com" />
				<label asp-for="Input.Username">Username</label>
				<span asp-validation-for="Input.Username" class="text-danger"></span>
			</div>

Отиваме на Register.cshtml.cs  и добавяме Input.Username вместо Input.Email

await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);


Отиваме на CinemaApp.Data.Models и добавяме нов модел 
	public virtual ICollection<ApplicationUserMovie> MovieUsersWhishlist { get; set; } = new HashSet<ApplicationUserMovie>();

Връщаме са на ApplicationUser.cs и добавяме нови пропъртита към модела

public ICollection<ApplicationUserMovie> ApplicationUserMovies { get; set; } = new HashSet<ApplicationUserMovie>();

Отиваме на CinemaApp.Data/Configuration и добавяме нова конфигурация ApplicationUserMovieConfiguration

Добавяме нова миграция

Отиваме на Movie.cs и добавяме ново пропърти string ImageUrl
Теглим no-image изображение и го слагаме в CinemaApp.Web/wwwroot/images/no-image.jpg

Отиваме на CinemaApp.Common/ApplicationConstants.cs и добавяме нова константа - линк към празното изображение
public const string NoImageUrl = "~/images/no-image";
Отиваме на CinemaApp.Common/EntityValidationConstants.cs и добавяме константи за максимална и минимална дължина на URL
Отиваме на MovieConfiguration и добавяме Fluent API за ImageUrl

builder.Property(m => m.ImageUrl)
				   .IsRequired(false)
				   .HasMaxLength(ImageUrlMaxLength)
				   .HasDefaultValue(NoImageUrl);

Добавяаме нова миграция

Отиваме на CinmaApp.Web.ViewModels/Movie/AddMoveInputModel.cs и добавяме пропърти

		[MaxLength(ImageUrlMaxLength)]
		public string? ImageUrl { get; set; }

Отиваме на MovieController и добавяаме в IActionResult Create пропъртито ImageUrl
ImageUrl = inputModel.ImageUrl,

Отиваме на Views/Movie/Create.cshtml и добавяме ImageUrl

<div>
		<label asp-for="ImageUrl">Image URL</label>
		<input asp-for="ImageUrl" class="form-control"></input>
		<span asp-validation-for=ImageUrl class="text-danger"></span>
</div>

Отиваме на Controllers и създаваме нов WatchlistController
Отиваме на Views и създаваме нова папка Watchlist и добавяме ново View Index.cshtml в който копираме кода от документа
Оправяме @using
Отиваме на CinemaApp.Web.ViewModels и правим нова папка Watchlist и в нея добавяме ApplicationUserWatchListViewModel
Написаваме му пропъртитата и добавяме в Watchlist Controller

public async Task<IActionResult> Index()
		{
			string? userId = userManager.GetUserId(User);

			if (userId == null) 
			{
				return RedirectToPage("Login");
			}

			
			return View();
		}

Добавяме DbSet, който е трябвало да добавим по-рано

public virtual DbSet<ApplicationUserMovie> UserMovies { get; set; } = null!;

Правим миграция

Добавяме нова конфигурация в Program.cs
builder.Services.ConfigureApplicationCookie(cfg =>
			{
				cfg.LoginPath = "/Identity/Account/Login";
			});

Отиваме на Views/Movie/Index.cshtml и добавяме този код:

@if (User?.Identity?.IsAuthenticated ?? false)
                    {
                        <form asp-controller="Watchlist" asp-action="AddToWatchlist" method="post" class="d-inline">
                            <input type="hidden" name="movieId" value="@movie.Id" />
                            <button type="submit" class="btn btn-warning">+Watchlist</button>
                        </form>
                    }
Отиваме на Views/Shared/_Layout и обавяме този код:

@if(User?.Identity?.IsAuthenticated ?? false)
						{
							<li class="nav-item">
								<a class="nav-link text-dark" asp-area="" asp-controller="Watchlist" asp-action="Index">My Watchlist</a>
							</li>
						}

Отиваме на WatchlistController и добавяме нов екшън

[HttpPost]
		public async Task<IActionResult> AddToWatchList(string movieId)

Отиваме на Views/Watchlist/Index и добавяме кода от документа:
<form asp-controller="Watchlist" asp-action="RemoveFromWatchlist" method="post" class="d-inline">
						<input type="hidden" name="movieId" value="@movie.MovieId" />
						<button type="submit" class="btn btn-danger">Remove</button>
					</form>

Отиваме на WatchlistController и създаваме нов Action



 */