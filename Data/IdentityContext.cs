﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Data
{
    public class IdentityContext:IdentityDbContext<User, Role, int>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options):base(options)
        {

        }
}
}
