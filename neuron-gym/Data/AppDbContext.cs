using Microsoft.EntityFrameworkCore;
using neuron_gym.Models;
using System.Collections.Generic;

namespace neuron_gym.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Score> Scores => Set<Score>();
    }
}
