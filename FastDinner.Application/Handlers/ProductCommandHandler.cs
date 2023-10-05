using FastDinner.Application.Commands;
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
    private readonly IUnitOfWork _unitOfWork;

    public ProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
         _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.CreateAsync(new Product(command.Name));

        await _unitOfWork.CommitAsync();

        return new ProductResponse(product.Id, product.Name);
    }

    public async Task<ProductResponse> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(command.Id);

        // Throw new NotFoundException(nameof(Menu), command.Id);
        product.Update(command.Name);

        await _productRepository.UpdateAsync(product);

        await _unitOfWork.CommitAsync();

        return new ProductResponse(product.Id, product.Name);
    }
}