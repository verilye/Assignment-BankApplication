using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using WebDevAss2.Models;

namespace WebDevAss2.Data;

public class McbaDbContext : DbContext
{

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Login> Logins { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Payee> Payees{get;set;}
    public DbSet<BillPay> BillPays { get; set;}

    //Code first approach
    // https://www.c-sharpcorner.com/article/using-entity-framework-core/

    public McbaDbContext(DbContextOptions<McbaDbContext> options): base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Transaction>()
            .HasOne(t=>t.Account)
            .WithMany(a=>a.Transactions)
            .HasForeignKey(t=>t.AccountNumber);

        builder.Entity<BillPay>()
            .HasOne(t=>t.Account)
            .WithMany(a=>a.BillPays)
            .HasForeignKey(t=>t.AccountNumber);

        builder.Entity<BillPay>()
            .HasOne(t=>t.Payee)
            .WithMany(a=>a.BillPays)
            .HasForeignKey(t=>t.PayeeId);

    }
};