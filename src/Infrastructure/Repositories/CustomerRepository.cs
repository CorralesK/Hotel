﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.src.Core.Entities;
using Hotel.src.Core.Interfaces.IRepository;
using Hotel.src.Infrastructure.Data;

namespace Hotel.src.Infrastructure.Repositories
{
    class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User AddCliente(User user)
        {
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }
    }
}
