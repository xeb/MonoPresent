using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

namespace Controllers
{
	[HandleError]
	public class HomeController : Controller
	{
		public ActionResult Index (string id)
		{
			ViewData ["Message"] = "You said: " + id;
			return View ();
		}
	}
}

