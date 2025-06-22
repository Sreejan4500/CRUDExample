using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers
{
    public class PersonsController : Controller
    {
        private readonly IPersonService _personService;
        private readonly ICountryService _countryService;

        public PersonsController(IPersonService personService, ICountryService countryService)
        {
            _personService = personService ?? throw new ArgumentNullException(nameof(personService), "Person service cannot be null.");
            _countryService = countryService;
        }

        [Route("persons/index")]
        [Route("/")]
        public IActionResult Index(
            string searchBy, string? searchString, 
            string sortBy = nameof(PersonResponse.PersonName), SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            #region Code for Searching

            ViewBag.SearchFields = new Dictionary<string, string>
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
                { nameof(PersonResponse.Age), "Age" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryName), "Country" },
                { nameof(PersonResponse.Address), "Address" },
                { nameof(PersonResponse.ReceiveNewsLetters), "Receive News Letters" },
            };

            List<PersonResponse> persons = _personService.GetFilteredPersons(searchBy, searchString);

            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            #endregion

            #region Code for Sorting

            List<PersonResponse> sortedPersons = _personService.GetSortedPersons(persons, sortBy, sortOrder);

            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();

            #endregion


            return View(sortedPersons);
        }

        [Route("persons/create")]
        [HttpGet]
        public IActionResult Create()
        {            
            List<CountryResponse> countries = _countryService.GetAllCountries();
            ViewBag.Countries = countries;
            return View();
        }

        [Route("persons/create")]
        [HttpPost]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (ModelState.IsValid)
            {
                _personService.AddPerson(personAddRequest);
                return RedirectToAction("Index");
            }

            List<CountryResponse> countries = _countryService.GetAllCountries();
            ViewBag.Countries = countries;
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return View();
        }

    }
}
