using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Product;
using MediatR;

namespace FastDinner.Application.Handlers;

// ReSharper disable once UnusedMember.Global
public class ProductQueryHandler : 
    IRequestHandler<ProductQuery, IEnumerable<ProductResponse>>
{
    private readonly IProductRepository _productRepository;
    
    public ProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductResponse>> Handle(ProductQuery _, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetAllAsync();

        return products.Select(x => new ProductResponse(x.Id, x.Name));
    }
}