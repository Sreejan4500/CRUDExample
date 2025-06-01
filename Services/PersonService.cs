using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class PersonService : IPersonService
    {
        public PersonResponse AddPerson(PersonAddRequest personAddRequest)
        {
            throw new NotImplementedException();
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            throw new NotImplementedException();
        }
    }
}
