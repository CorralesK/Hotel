using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.Core.Entities;
using Hotel.src.Core.Enums;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Core.Interfaces.IServices;
using Microsoft.EntityFrameworkCore.Internal;

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

            return _customerRepository.AddCliente(user);

        }
    }
}
