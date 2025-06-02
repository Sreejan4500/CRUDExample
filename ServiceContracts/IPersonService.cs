using ServiceContracts.DTO;

namespace ServiceContracts
{
    public interface IPersonService
    {
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);
        List<PersonResponse> GetAllPersons();
        PersonResponse? GetPersonByPersonID(Guid? personID);
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString);
    }
}
