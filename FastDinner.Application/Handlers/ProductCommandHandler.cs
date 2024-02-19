using FastDinner.Application.Commands.Product;
using FastDinner.Application.Common.Interfaces.Repositories;
using FastDinner.Contracts.Product;
using FastDinner.Domain.Model;
using MediatR;

namespace FastDinner.Application.Handlers;

public class ProductCommandHandler :
    IRequestHandler<CreateProductCommand, ProductResponse>,
    IRequestHandler<UpdateProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;
    //private readonly IUnitOfWork _unitOfWork;

    public ProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
        //_unitOfWork = unitOfWork;
    }

    public async Task<ProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.CreateAsync(new Product(command.Name));

        return new ProductResponse(product.Id, product.Name);
    }

    public async Task<ProductResponse> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id);

        if (product is null)
            throw new ApplicationException("Product not found with id " + command.Id);

        product.Update(command.Name);

        return new ProductResponse(product.Id, product.Name);
    }
}