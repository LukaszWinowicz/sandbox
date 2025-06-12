using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ProxyPattern;

// Abstract 
public interface IProductRepository
{
    Product Get(int id);
}


// Proxy - logger
public class LoggerProductRepository : IProductRepository
{
    private readonly IProductRepository repository;

    public ConcurrentDictionary<Product, int> Stats = new ConcurrentDictionary<Product, int>();
    
    public LoggerProductRepository(IProductRepository repository)
    {
        this.repository = repository;
    }
    public Product Get(int id)
    {
        var product = repository.Get(id);

        Stats.AddOrUpdate(product, 1, (p, value) => ++value);

        return product;
    }
}

public class DbProductRepository : IProductRepository
{
    private readonly IDictionary<int, Product> products;

    public DbProductRepository()
    {
        products = new Dictionary<int, Product>()
        {
            { 1, new Product(1, "Product 1", 10) },
            { 2, new Product(2, "Product 2", 20) },
            { 3, new Product(3, "Product 3", 30) },
        };
    }

    public Product Get(int id)
    {
        if (products.TryGetValue(id, out Product product))
        {
            return product;
        }
        else
            return null;
    }
}


