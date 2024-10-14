﻿using Microsoft.AspNetCore.Mvc;

namespace CinemaApp.Web.Controllers
{
	public class BaseController: Controller
	{
		protected bool IsGuidValid(string? id, ref Guid parsedGuid)
		{
			//Non-existing parameter in the URL
			if (String.IsNullOrWhiteSpace(id))
			{
				return false;
			}

			bool isGuidValid = Guid.TryParse(id, out parsedGuid);

			//Invalid parameter in the URL
			if (!isGuidValid)
			{
				return false;
			}

			return true;
		}
	}
}
