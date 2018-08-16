using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pizza_Demo.Models;

namespace Pizza_Demo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Order> Order { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
