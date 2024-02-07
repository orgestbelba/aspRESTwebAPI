using AutoMapper;
using aspRESTwebAPI.Dto;
using aspRESTwebAPI.Models;

namespace aspRESTwebAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //Created the WriteDtos because the id is handled from EF

            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
            CreateMap<WriteCustomerDto, Customer>();

            CreateMap<Order, OrderDto>();
            CreateMap<OrderDto, Order>();
            CreateMap<WriteOrderDto, Order>();

            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<WriteProductDto, Product>();
        }
    }
}
