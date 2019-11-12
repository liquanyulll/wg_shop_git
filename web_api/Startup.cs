using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using wg_core.Domain;
using wg_frame_work;
using wg_service;
using wg_service.Orders;
using wg_service.Products;
using wg_service.Users;

namespace web_api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    //options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});
            //关掉自动转小写
            //services.AddMvc().AddJsonOptions(options => { options.SerializerSettings.ContractResolver = new DefaultContractResolver(); });

            services.AddRazorPages();

            //跨域配置
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAllOrigins",
            //    builder =>
            //    {
            //        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            //    });
            //});

            services.AddDbContext<ShopContext>(options =>
              options.UseMySql(Configuration.GetConnectionString("constring")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            #region 服务注册
            services.AddTransient<HomeService>();
            services.AddTransient<AuthenticationSupport>();
            services.AddTransient<AccountService>();
            services.AddTransient<ProductService>();
            services.AddTransient<ProductTypeService>();
            services.AddTransient<MoneyKeyService>();
            services.AddTransient<InvitationService>();
            services.AddTransient<OrderService>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            #endregion

            services.AddAutoMapper(typeof(Startup));
            services.AddMvc(options =>
            {
                options.Filters.Add(new UserFilterAttribute(new AuthenticationSupport()));
                options.Filters.Add(typeof(GlobalExceptions));
            });

            services.AddMvc().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                WebConfig.SystemWebDomain = "http://localhost:9091/";

                //启用跨域
                app.UseCors(builder =>
                {
                    builder.WithOrigins("http://localhost:9090/").AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowAnyOrigin();
                });
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                WebConfig.SystemWebDomain = "http://139.9.203.54:9091/";

                //启用跨域
                app.UseCors(builder =>
                {
                    builder.WithOrigins("http://www.yiyaogou.top", "http://yiyaogou.top")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin();
                });
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");
            //    endpoints.MapRazorPages();
            //});

            //app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapDefaultControllerRoute();
            });

            //设置默认访问页面
            app.UseDefaultFiles();
            //开启静态资源（wwwroot）
            app.UseStaticFiles();
            //初始化DI解析器
            ServiceEngin.Initialize(app.ApplicationServices);
        }
    }
}
