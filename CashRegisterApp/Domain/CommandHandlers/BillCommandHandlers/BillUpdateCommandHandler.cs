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
    public class BillUpdateCommandHandler : IRequestHandler<UpdateBillCommand, bool>
    {
        private readonly IBillRepository _billRepository;

        public BillUpdateCommandHandler(IBillRepository billRepository)
        {
            _billRepository = billRepository;
        }
        public Task<bool> Handle(UpdateBillCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var bill = _billRepository.GetBills().First(x => x.Bill_number == request.Bill_number);
                if (bill != null)
                {
                    bill.Bill_number = request.Bill_number;
                    bill.Total_cost = request.Total_cost;
                    bill.Credit_card = request.Credit_card;
                    _billRepository.Update(bill, bill.Bill_number);
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
