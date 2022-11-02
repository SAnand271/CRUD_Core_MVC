using CRUD.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Data
{

    public partial class EmployeeContext : DbContext
    {
        public EmployeeContext()
        {
        }

        public EmployeeContext(DbContextOptions<EmployeeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Employees> Employees { get; set; }


    }
}



