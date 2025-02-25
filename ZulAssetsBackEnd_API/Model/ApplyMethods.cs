﻿
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ZulAssetsBackEnd_API.Model
{
    public class RemoveVersionFromParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters.Count > 0)
            {
                var aa = operation.Parameters[0];
                var versionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "version");
                //var versionParameter = operation.Parameters
                //.FirstOrDefault(p => p.Name == "version" && p.In == ParameterLocation.Query);
                operation.Parameters.Remove(versionParameter);
            }
        }
    }

    public class ReplaceVersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths;
            swaggerDoc.Paths = new OpenApiPaths();

            foreach (var path in paths)
            {
                var key = path.Key.Replace("v{version}", swaggerDoc.Info.Version);
                var value = path.Value;
                swaggerDoc.Paths.Add(key, value);
            }
        }
    }

}
