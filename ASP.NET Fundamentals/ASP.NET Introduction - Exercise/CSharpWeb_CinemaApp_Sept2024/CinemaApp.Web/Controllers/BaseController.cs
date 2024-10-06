using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
	public class BaseController: Controller
	{
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
	}
}
