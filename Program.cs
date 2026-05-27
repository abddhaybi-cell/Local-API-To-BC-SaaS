using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.IO.Enumeration;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var sourceFolder = @"C:\SrcFolder\";
var processedFolder = @"C:\SrcFolder\Processed\";

/* if (!Directory.Exists(processedFolder))
    Directory.CreateDirectory(ProcessedFolder); */

app.MapGet("/readfile", (HttpRequest request, string filename) => 
{
    var apiKey = request.Headers["x-api-key"];
    if (apiKey != "MySuperSecretKey")
        return Results.Unauthorized();

    var fullPath = Path.Combine(sourceFolder, filename);
    if (!File.Exists(fullPath)) 
        return Results.NotFound("File not found");

    var content = File.ReadAllText(fullPath);
        return Results.Ok(content);
});


app.MapPost("/movefile",(HttpRequest request, string filename) =>
{
    var apiKey = request.Headers["x-api-key"];
    if (apiKey != "MySuperSecretKey")
        return Results.Unauthorized();
    var fullPath = Path.Combine(sourceFolder, filename);
    if (!File.Exists(fullPath)) 
        return Results.NotFound("File not found");

    var destPath = Path.Combine(processedFolder, filename);

    if(File.Exists(destPath))
        File.Delete(destPath);
    
    File.Move(fullPath, destPath);

    return Results.Ok($"File '{filename}' moved to processed folder.");
});

app.Run();
