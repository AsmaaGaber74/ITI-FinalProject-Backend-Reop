using AutoMapper;
using Jumia.Application.Contract;
using Jumia.Dtos;
using Jumia.Dtos.ResultView;
using Jumia.Dtos.ViewModel.category;
using Jumia.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jumia.Application.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IPaymentReposatory _paymentReposatory;
        private readonly IMapper _mapper;

        public PaymentServices(IPaymentReposatory paymentReposatory, IMapper mapper) 
        {
            _paymentReposatory = paymentReposatory;
            _mapper = mapper;
        }

        public async Task<ResultView<PaymentDto>> Create(int orderid)
        {
            var paymentDt = new Payment()
            {
                orderID = orderid,
                DatePaid = DateTime.Now,
                paymentMethod = "PayPal"
            };            
            var newPayment = await _paymentReposatory.Create(paymentDt);
            await _paymentReposatory.SaveChanges();

            var paymentDtoResult = _mapper.Map<PaymentDto>(newPayment);
            return new ResultView<PaymentDto> { Entity =  paymentDtoResult, IsSuccess = true, Message = "Created Successfully" };
        }
    }
}
