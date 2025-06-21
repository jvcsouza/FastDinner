using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Application.Queries;
using FastDinner.Contracts.Product;
using MediatR;

namespace FastDinner.Application.Handlers;

// ReSharper disable once UnusedMember.Global
public class ProductQueryHandler : 
    IRequestHandler<ProductQuery, IEnumerable<ProductResponse>>,
    IRequestHandler<ProductQueryById, ProductResponse>
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

    public async Task<ProductResponse> Handle(ProductQueryById request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);
        return new ProductResponse(product.Id, product.Name);
    }
}