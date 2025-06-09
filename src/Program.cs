using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using FraudSys.Repositories;

DotNetEnv.Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// 1. Get the AWS options from the "AWS" section of appsettings.json
AWSOptions awsOptions = builder.Configuration.GetAWSOptions();

// 2. Set the default AWS options for the application
builder.Services.AddDefaultAWSOptions(awsOptions);

// 3. Register the DynamoDB client using the default options
builder.Services.AddAWSService<IAmazonDynamoDB>();

// 4. Register the DynamoDB context
builder.Services.AddScoped<IDynamoDBContext, DynamoDBContext>();

// 5. Register your custom repository
builder.Services.AddScoped<ILimiteRepository, LimiteRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();