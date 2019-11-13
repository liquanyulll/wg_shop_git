﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wg_core.Domain;
using wg_model.Accounts;
using wg_model.Orders;
using wg_model.Products;

namespace web_api.AutoMappers
{
    public class CustomProfile : Profile
    {
        public CustomProfile()
        {
            CreateMap<t1_user, UserModel>();
            CreateMap<UserModel, t1_user>();

            CreateMap<t2_product_type, ProductTypeModel>().BeforeMap((source, dto) =>
            {
                //可以较为精确的控制输出数据格式
                dto.Childrens = source.InversePt_Parent.Select(item => new ProductTypeModel
                {
                    Pt_Id = item.Pt_Id,
                    Pt_Name = item.Pt_Name,
                    Sort = item.Sort,
                    Enabled = item.Enabled,
                    Deleted = item.Deleted
                }).ToList();
            });
            CreateMap<ProductTypeModel, t2_product_type>();

            CreateMap<ProductListModel, t2_product>();
            CreateMap<t2_product, ProductListModel>();

            CreateMap<ProductModel, t2_product>();
            CreateMap<t2_product, ProductModel>();

            CreateMap<InvitationModel, t3_user_product_invitation>();
            CreateMap<t3_user_product_invitation, InvitationModel>();

            CreateMap<OrderModel, t4_order>();
            CreateMap<t4_order, OrderModel>().BeforeMap((source, dto) =>
            {
                dto.ProductName = source.Product.ProductName;
            });

            CreateMap<t2_product_detail_Img, ProductDetailImgModel>();
            CreateMap<ProductDetailImgModel, t2_product_detail_Img>();

            CreateMap<t1_user_login_history, UserLoginHistoryModel>();
            CreateMap<t1_user_moneykey, MoneyKeyModel>();
        }
    }
}
