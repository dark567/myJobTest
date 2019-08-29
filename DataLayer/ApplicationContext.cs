using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayer
{
    public class ApplicationContext : DbContext
    {
        

        public ApplicationContext()
            : base("name=DbConnection")
        {
        }

        public DbSet<Sec_users> Sec_users { get; set; } //select login, pass from sec_users
    }
}
