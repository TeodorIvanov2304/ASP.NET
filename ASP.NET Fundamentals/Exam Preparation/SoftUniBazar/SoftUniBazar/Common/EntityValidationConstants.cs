namespace SoftUniBazar.Common
{
	public static class EntityValidationConstants
	{
		//Ad
		public const byte AdNameMinLength = 5;
		public const byte AdNameMaxLength = 25;
		public const byte DescriptionMinLength = 15;
		public const byte DescriptionMaxLength = 250;
		public const string CreatedOnDateFormat = "yyyy-MM-dd H:mm";

		//Category
		public const byte CategoryNameMinLength = 3;
		public const byte CategoryNameMaxLength = 15;
	}
}
