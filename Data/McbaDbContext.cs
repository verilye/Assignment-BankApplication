using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using WebDevAss2.Models;

namespace WebDevAss2.Data;

public class McbaDbContext : DbContext
{

    //Code first approach
    // https://www.c-sharpcorner.com/article/using-entity-framework-core/

    public McbaDbContext(DbContextOptions<McbaDbContext> options): base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Using Fluent API here will override everything else
        // Try to mostly use data annotations
        // Look at documentation for how to add separate configurations
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

   
};