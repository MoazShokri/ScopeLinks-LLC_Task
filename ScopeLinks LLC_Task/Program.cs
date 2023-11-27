


using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ScopeLinks_LLC_Task.DataAccess.Repository.IRepository;
using ScopeLinks_LLC_Task.DataAccess.Repository;
using System.Text;
using ScopeLinks_LLC_Task.Utility;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(MappingConfig));


#region JWT Authentication & Identity
builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(Roles.Admin, policy =>
	{
		// Require users to be in the 'Admin' role to access the resource
		policy.RequireRole(Roles.Admin);
	});
});
builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(Roles.User, policy =>
	{
		// Require users to be in the 'Admin' role to access the resource
		policy.RequireRole(Roles.User);
	});
});
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
			 .AddJwtBearer(o =>
			 {
				 o.RequireHttpsMetadata = false;
				 o.SaveToken = false;
				 o.TokenValidationParameters = new TokenValidationParameters
				 {
					 ValidateIssuerSigningKey = true,
					 ValidateIssuer = true,
					 ValidateAudience = true,
					 ValidateLifetime = true,
					 ValidIssuer = builder.Configuration["JWT:Issuer"],
					 ValidAudience = builder.Configuration["JWT:Audience"],
					 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
				 };
			 });
#endregion

#region Dependency Injection

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuthService, AuthService>();
#endregion

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description =
				  "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
				  "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\n" +
				  "Example: \"Bearer 12345abcdef\"",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Scheme = "Bearer"
	});
	options.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
							  In = ParameterLocation.Header
				   },

					  new List<string>()
				}
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
