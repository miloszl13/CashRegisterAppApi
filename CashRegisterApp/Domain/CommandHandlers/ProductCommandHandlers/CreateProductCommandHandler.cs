using Domain.Commands.ProductCommands;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommandHandlers.ProductCommandHandlers
{
    public class CreateProductCommandHandler:IRequestHandler<CreateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        public CreateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public Task<bool> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = new Product()
                {
                    Product_Id = request.Product_id,
                    Name = request.Name,
                    Cost = request.Cost,
                };
                _productRepository.Add(product);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }

        }
    }
}
