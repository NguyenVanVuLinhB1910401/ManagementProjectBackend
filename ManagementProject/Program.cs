using ManagementProject.Interfaces;
using ManagementProject.Models;
using ManagementProject.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
//Add Auto Mapper
builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr")));
builder.Services.AddTransient<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IProjectMemberRepository, ProjectMemberRepository>();
builder.Services.AddTransient<IWorkRepository, WorkRepository>();
builder.Services.AddTransient<ICommentRepository, CommentRepository>();
builder.Services.AddTransient<IAttachmentFileRepository, AttachmentFileRepository>();
builder.Services.AddTransient<IQuyTrinhRepository, QuyTrinhRepository>();
builder.Services.AddTransient<IBuocThucHienRepository, BuocThucHienRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

// For Identity  
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// Adding Authentication  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    //Lanh dao cong ty
    //options.AddPolicy(Permissions.QuanLyDuAn, policy => policy.RequireRole(UserRoles.LanhDaoCongTy));
    //options.AddPolicy(Permissions.QuanLyPhongBan, policy => policy.RequireRole(UserRoles.LanhDaoCongTy));
    //options.AddPolicy(Permissions.QuanLyNhanSu, policy => policy.RequireRole(UserRoles.LanhDaoCongTy));
});
        
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
    // C?u hình các tu? ch?n JSON khác (n?u c?n)
    //Xóa các tr??ng có value null khi convert sang Json
    options.JsonSerializerOptions.IgnoreNullValues = true;
});


builder.Services.Configure<FormOptions>(options =>
{
    //options.ValueLengthLimit = int.MaxValue; // Không giới hạn kích thước cho mỗi giá trị form
    options.MultipartBodyLengthLimit = 2028 * 1024 * 1024; // Không giới hạn kích thước cho toàn bộ yêu cầu
    //options.MemoryBufferThreshold = int.MaxValue; // Không giới hạn bộ nhớ đệm
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Quản trị dự án đầu tư xây dựng", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
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
    }
);
builder.Services.AddCors(option =>
{
    option.AddPolicy("MyPolicy", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
