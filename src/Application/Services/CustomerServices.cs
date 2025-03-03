using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Core.Interfaces.IServices;

namespace Hotel.src.Application.Services
{
    public class CustomerServices : IRegisterServices
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerServices(ICustomerRepository custumerRepository)
        {
            _customerRepository = custumerRepository;
        }

        public User RegisterCustomer(User user)
        {

            return _customerRepository.AddClient(user);

        }
    }
}
