namespace CinemaApp.Common
{
	public static class EntityValidationConstants
	{
		public static class Movie
		{	
			public const int TitleMinLength = 2;
			public const int TitleMaxLength = 50;
			public const int GenreMinLength = 3;
			public const int GenreMaxLength = 20;
			public const int DurationMinValue = 1;
			public const int DurationMaxValue = 55_000;
			public const int DirectorNameMinLength = 10;
			public const int DirectorNameMaxLength = 80;
			public const int DescriptionNameMinLength = 50;
			public const int DescriptionNameMaxLength = 500;
		}
	}
}
