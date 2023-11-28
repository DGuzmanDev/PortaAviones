using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSession();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/Home/ErrorHandler");

//Esta condicion es necesaria para poder ejecutar la solucion publicada directamente desde un Mac OS
//Ya que el folder wwwroot no se encuentra en el mismo directorio en este sistema operativo, por lo que los archivos
//estaticos no son encontrado al ejecutar la aplicacion en este perfil (publish)
if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
    app.UseStaticFiles(new StaticFileOptions
    { FileProvider = new PhysicalFileProvider(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "wwwroot")), RequestPath = "" });
else
    //Este caso se utiliza para ejecutarse en plataformas Windows
    app.UseStaticFiles();
    
app.UseAuthorization();
app.UseSession();

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();