using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sale.UpdateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings
{
    public class SalesRequestProfile : Profile
    {
        public SalesRequestProfile()
        {

            CreateMap<CreateSaleRequest, CreateSaleCommand>();
            CreateMap<CreateSaleRequest, CreateSaleCommand>();

            CreateMap<UpdateSaleRequest, UpdateSaleCommand>();
            CreateMap<UpdateSaleItemCommand, UpdateSaleItemResult>();
            CreateMap<UpdateSaleCommand, UpdateSaleResult>();
            CreateMap<UpdateSaleItemResult, UpdateSaleItemResponse>();
            CreateMap<UpdateSaleResult, UpdateSaleResponse>();


        }
    }
}
