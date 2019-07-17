using System.Web.Mvc;
using DavidTielke.PersonManagementApp.Logic.PersonManagement.Contract;
using DavidTielke.PersonManagementApp.UI.AspWebClient.Models.PeopleModels;

namespace DavidTielke.PersonManagementApp.UI.AspWebClient.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IPersonManager _manager;

        public PeopleController(IPersonManager manager)
        {
            _manager = manager;
        }

        // GET: People
        public ActionResult Index()
        {
            var adults = _manager.GetAllAdults();
            var children = _manager.GetAllChildren();

            var model = new PeopleIndexModel
            {
                Adults = adults,
                Children = children
            };

            return View(model);
        }
    }
}