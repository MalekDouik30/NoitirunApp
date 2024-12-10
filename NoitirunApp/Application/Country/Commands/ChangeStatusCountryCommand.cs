namespace NoitirunApp.Application.Country.Commands
{
    public class ChangeStatusCountryCommand : IRequest<bool>
    {
        public Guid idCountry { get; set; }
    }
}
