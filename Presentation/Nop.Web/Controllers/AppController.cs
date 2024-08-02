using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Nop.Core;
using Nop.Core.Infrastructure;
using Nop.Services.Common;
using System;
using System.IO;
using System.Linq;

namespace Nop.Web.Controllers;

[AutoValidateAntiforgeryToken]

#nullable enable
public class AppController : Controller
{
    #region Fields

    private readonly INopFileProvider _fileProvider;

    #endregion

    #region Ctor

    public AppController(INopFileProvider fileProvider)
    {
        _fileProvider = fileProvider;
    }

    #endregion

    public IActionResult Index()
    {
        try
        {
            // Ruta del archivo que se desea enviar al cliente
            string path = _fileProvider.MapPath("~/wwwroot/app-builds/production");
            string[] files = _fileProvider.GetFiles(path, "brink-movil-*.apk", true);
            string? file = GetHigherVersion(files);

            // Verificar si el archivo existe
            if (file != null)
            {

                // Leer el contenido del archivo
                byte[] fileBytes = System.IO.File.ReadAllBytes(file);

                // Nombre del archivo que se enviará al cliente
                string fileName = Path.GetFileName(file);

                // Tipo de contenido MIME para el archivo
                string contentType = "application/vnd.android.package-archive";

                // Enviar el archivo al cliente como una descarga
                return File(fileBytes, contentType, fileName);

            }
            else
            {
                return View();
            }
        }
        catch
        {
            return View();
        }
    }

    private string? GetHigherVersion(string[] files)
    {
        // Extraer el número de versión de cada archivo y obtener el archivo con la versión más alta
        return files
             .OrderByDescending(x =>
             {
                 var nameWithoutExtension = Path.GetFileNameWithoutExtension(x);
                 var version = nameWithoutExtension?.Split('-').LastOrDefault();
                 return version == null ? Version.Parse("0.0.0") : Version.Parse(version);
             })
             .FirstOrDefault(); // Obtener la versión más alta
    }
}
