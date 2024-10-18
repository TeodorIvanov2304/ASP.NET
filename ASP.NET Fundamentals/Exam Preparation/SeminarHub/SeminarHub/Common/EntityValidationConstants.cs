namespace SeminarHub.Common
{
	public class EntityValidationConstants
	{
		//Seminar
		public const byte TopicMinLength = 3;
		public const byte TopicMaxLength = 100;
		public const byte LecturerMinLength = 5;
		public const byte LecturerMaxLength = 60;
		public const int DetailsMinLength = 10;
		public const int DetailsMaxLength = 500;
		public const string DateAndTimeFormat = "dd/MM/yyyy HH:mm";
		public const byte DurationMinValue = 30;
		public const byte DurationMaxValue = 180;

		//Category
		public const byte CategoryNameMinLength = 3;
		public const byte CategoryNameMaxLength = 50;
	}
}
