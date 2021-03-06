using FairPos.Epylion.Logger;
using FairPos.Epylion.Logger.Contracts;
using FairPos.Epylion.Logger.Repositories;
using FairPos.Epylion.Service;
using FairPos.Epylion.Service.Operations;
using FairPos.Epylion.Service.Report;
using FairPos.Epylion.Service.Requisition;
using FairPos.Epylion.Service.Sales;
using FairPos.Epylion.Service.Setups;
using FairPos.Epylion.Service.Transfer;
using FairPos.Epylion.WebApi.Vs19.JWT;
using FairPos.Epyllion.Repository;
using FairPos.Epyllion.Repository.Operations;
using FairPos.Epyllion.Repository.Report;
using FairPos.Epyllion.Repository.Requisition;
using FairPos.Epyllion.Repository.Sales;
using FairPos.Epyllion.Repository.Setups;
using FairPos.Epyllion.Repository.Transfer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FairPos.Epylion.WebApi.Vs19.Extension
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    //FileName: ServiceExtension.cs
    //FileType: C# Source file
    //Author : SHUVO , RAHEE
    //Created On : 14/09/2021 9:56:39 AM
    //Last Modified On : 15/09/2021 04:00:00 PM
    //Copy Rights : MediaSoft Data System LTD
    //Description : Extension Function for Startup method
    ////////////////////////////////////////////////////////////////////////////////////////////////////////
    ///
    public static class ServiceExtension
    {
        public static void ConfigureRepositoryWrapper(this IServiceCollection services) //calling repositories
        {
            services.AddSingleton<IAuthorizationHandler, AccountHandler>();
            services.AddSingleton<ILog, LogNLog>();
            services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
            services.AddHostedService<JwtRefreshTokenCache>();
            services.AddScoped<IDBConnectionProvider, DBConnectionProvider>();

            services.AddScoped<IUsersWebService, UsersWebService>();
            services.AddScoped<IUsersWebRepository, UsersWebRepository>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<IShopListService, ShopListService>();
            services.AddScoped<IShopListRepository, ShopListRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddTransient<IMeasureUnitRepository, MeasureUnitRepository>();
            services.AddTransient<IMeasureUnitService, MeasureUnitService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IItemListService, ItemListService>();
            services.AddTransient<IItemListRepository, ItemListRepository>();
            services.AddTransient<IEmployeeProductRepository, EmployeeProductRepository>();
            services.AddTransient<IEmployeeProductService, EmployeeProductService>();
            services.AddTransient<IEmployeeImageService, EmployeeImageService>();
            services.AddTransient<IEmployeeImageRepository, EmployeeImageRepository>();
            services.AddTransient<IMemberCategoryService, MemberCategoryService>();
            services.AddTransient<IMemberCategoryRepository, MemberCategoryRepository>();
            services.AddTransient<IMemberCategoryProductService, MemberCategoryProductService>();
            services.AddTransient<IMemberCategoryProductRepository, MemberCategoryProductRepository>();
            services.AddTransient<IPurchaseOrderService, PurchaseOrderService>();
            services.AddTransient<IPurchaseOrderRepository, PurchaseOrderRepository>();
            services.AddTransient<ICircularPriceChangedService, CircularPriceChangedService>();
            services.AddTransient<ICircularPriceChangedRepositroy, CircularPriceChangedRepositroy>();
            services.AddTransient<IApprivalCheckRepository, ArrivalCheckRepository>();
            services.AddTransient<IApprivalCheckService, ArrivalCheckService>();
            services.AddTransient<IQualityControlService, QualityControlService>();
            services.AddTransient<IQualityControlRepository, QualityControlRepository>();
            services.AddTransient<IItemConversionRepository, ItemConversionRepository>();
            services.AddTransient<IItemConversionService, ItemConversionService>();
            services.AddTransient<IDamageLossService, DamageLossService>();
            services.AddTransient<IDamageLossRepository, DamageLossRepository>();
            services.AddTransient<IReturnService, ReturnService>();
            services.AddTransient<IReturnRepository, ReturnRepository>();
            services.AddTransient<IMainToStaffWorkerService, MainToStaffWorkerService>();
            services.AddTransient<IMainToStaffWorkerRepository, MainToStaffWorkerRepository>();
            services.AddTransient<IWorkerToMainStaffService, WorkerToMainStaffService>();
            services.AddTransient<IWorkerToMainStaffRepository, WorkerToMainStaffRepository>();
            services.AddTransient<IStaffToMainWorkerService, StaffToMainWorkerService>();
            services.AddTransient<IStaffToMainWorkerRepository, StaffToMainWorkerRepository>();
            services.AddTransient<ISalesOrderService, SalesOrderService>();
            services.AddTransient<ISalesOrderRepository, SalesOrderRepository>();
            services.AddTransient<ISalesWorkerService, SalesWorkerService>();
            services.AddTransient<ISalesWorkerRepository, SalesWorkerRepository>();
            services.AddTransient<ISalesOrderPrintService, SalesOrderPrintService>();
            services.AddTransient<ISalesOrderPrintRepository, SalesOrderPrintRepository>();
            services.AddTransient<ISalesStaffService, SalesStaffService>();
            services.AddTransient<ISalesStaffRepository, SalesStaffRepository>();
            services.AddTransient<IMonthlyBudgetService, MonthlyBudgetService>();
            services.AddTransient<IMonthlyBudgetRepository, MonthlyBudgetRepository>();
            services.AddTransient<IAutoRequisitionService, AutoRequisitionService>();
            services.AddTransient<IAutoRequisitionRepository, AutoRequisitionRepository>();            
            services.AddTransient<IRequisitionApprovalService, RequisitionApprovalService>();            
            services.AddTransient<IRequisitionApprovalRepository, RequisitionApprovalRepository>();            
            services.AddTransient<IRequisitionToPoService, RequisitionToPoService>();            
            services.AddTransient<IRequisitionToPoRepository, RequisitionToPoRepository>();            
            services.AddTransient<IArrivalEditService, ArrivalEditService>();            
            services.AddTransient<IArrivalEditRepository, ArrivalEditRepository>();            
            services.AddTransient<IInventoryService, InventoryService>();            
            services.AddTransient<IInventoryRepository, InventoryRepository>();            
            services.AddTransient<IShopToShopService, ShopToShopService>();            
            services.AddTransient<IShopToShopRepository, ShopToShopRepository>();            

            services.AddTransient<IUserCounterService, UserCounterService>();
            services.AddTransient<IUserCounterRepository, UserCounterRepository>();
            services.AddTransient<ILoginConferenceService, LoginConferenceService>();
            services.AddTransient<ILoginConferenceRepository, LoginConferenceRepository>();
            services.AddTransient<IEmailHelperRepository, EmailHelperRepository>();
            services.AddTransient<IEmailHelperService, EmailHelperService>();

            //services.AddTransient<ISalesManService, SalesManService>();
            //services.AddTransient<ISalesManRepository, SalesManRepository>();

            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IReportRepository, ReportRepository>();

        }
        public static void ConfigureDatabaseContext(this IServiceCollection services, IConfiguration config)//for passing database credentials
        {
            IDBConnectionProvider dBConnectionProvider = new DBConnectionProvider();
            dBConnectionProvider.SetDbConnectionString(config.GetConnectionString("DefaultConnection"));
        }
        public static void ConnfigControllers(this IServiceCollection services)
        {

            services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                option.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        }

        public static void AuthorizationWrapper(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                //options.AddPolicy(nameof(Policy.Account),
                //                  policy => policy.Requirements.Add(new AccountRequirement()));

                options.AddPolicy(nameof(Policy.Account), policy =>
                  policy.RequireRole(GlobalFunction.SystemRole
                  ).AddRequirements(new AccountRequirement()));
            });
        }
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FairPos.Epylion.WebApi.Vs19", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                    "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                    "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
                    "Example: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }
        public static void ConfigureAutoMapper(this IServiceCollection services) //tagging automapper profile
        {
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

        }
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILog logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        logger.Error($"Something went wrong: {contextFeature.Error}");

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error. Error generated by NLog!"
                        }.ToString());
                    }
                });
            });
        }
        public static void ConfigureJWTBearer(this IServiceCollection services, IConfiguration Configuration) // for jwt bearer
        {
            var jwtTokenConfig = Configuration.GetSection("jwtTokenConfig").Get<JwtTokenConfig>();

            services.AddSingleton(jwtTokenConfig);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtTokenConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                    ValidAudience = jwtTokenConfig.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });
        }
    }
}
