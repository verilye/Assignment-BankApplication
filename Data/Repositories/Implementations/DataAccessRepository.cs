using System;
using System.Net.Http;
using Newtonsoft.Json;
using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public class DataAccessRepository : IDataAccessRepository
{
    private readonly McbaDbContext _context;
    public DataAccessRepository(McbaDbContext context)
    {
        _context = context;
    }

    public void StoreJsonData(List<Customer> data)
    {
        _context.Customers.AddRange(data);
        _context.SaveChanges();
        return;
    }
}