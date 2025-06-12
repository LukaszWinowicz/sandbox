using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProxyPattern;

// Proxy (Pośrednik) - cache
public class CacheProductRepository : IProductRepository
{
    // Real Subject
    private readonly IProductRepository repository;

    private IDictionary<int, Product> products;

    public CacheProductRepository(IProductRepository repository)
    {
        products = new Dictionary<int, Product>();
        this.repository = repository;
    }

    public void Add(Product product)
    {
        products.Add(product.Id, product);
    }

    public Product Get(int id)
    {
        if (products.TryGetValue(id, out Product product))
        {
            product.CacheHit++;

            return product;
        }
        else
        {
           product = repository.Get(id);   // Real Subject

            if (product != null)
            {
                products.Add(product.Id, product); // Dodaje do cache'a
            }
        }

        return product;
    }

}
