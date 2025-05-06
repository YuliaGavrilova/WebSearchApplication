using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebSearch.DataAccess
{
    internal class DesignTimeDbContextFactory :  IDesignTimeDbContextFactory<WebSearchDbContext>
    {
        public WebSearchDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WebSearchDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost;Database=WebSearch;Integrated Security=true");
            return new WebSearchDbContext(optionsBuilder.Options);
        }
    }
    
}
