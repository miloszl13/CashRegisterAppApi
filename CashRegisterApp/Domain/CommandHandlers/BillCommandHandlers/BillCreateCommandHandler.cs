using Domain.Commands.BillCommands;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CommandHandlers.BillCommandHandlers
{
    public class BillCreateCommandHandler:IRequestHandler<CreateBillCommand,bool>
    {
        private readonly IBillRepository _billRepository;
        public BillCreateCommandHandler(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }
        public Task<bool> Handle(CreateBillCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = new Bill()
                {
                    Bill_number = request.Bill_number,
                    Total_cost = request.Total_cost,
                    Credit_card = request.Credit_card,
                };
                _billRepository.Add(bill);
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }
    }
}

