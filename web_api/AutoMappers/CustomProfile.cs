using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using wg_core.Domain;
using wg_model.Accounts;
using wg_model.Products;

namespace web_api.AutoMappers
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<t1_user, UserModel>();
            CreateMap<UserModel, t1_user>();

            CreateMap<t2_product_type, ProductTypeModel>();
            CreateMap<ProductTypeModel, t2_product_type>();
        }
    }
}
