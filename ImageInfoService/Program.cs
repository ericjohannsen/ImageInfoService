using ImageInfoService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient(); // This registers IHttpClientFactory
builder.Services.AddScoped<ImageService>(); // This adds your ImageService to the DI container

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/imageinfo", async (string url, ImageService imageService) =>
{
    // Use the ImageService to compute image info
    var imageInfo = await imageService.ComputeImageInfo(url);
    return imageInfo;
})
.WithName("GetImageInfo"); // This is an optional developer friendly name for the route

app.Run();


