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
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IProductRepository _productRepository;
        public UpdateProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var product = _productRepository.GetProducts().FirstOrDefault(x => x.Product_Id == request.Product_id);
                if (product != null)
                {
                    product.Product_Id = request.Product_id;
                    product.Name = request.Name;
                    product.Cost = request.Cost;
                    _productRepository.Update(product);
                    return Task.FromResult(true);
                }
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }


        }
    }
}
