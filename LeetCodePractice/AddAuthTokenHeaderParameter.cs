using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GISWaterSupplyAndSewageServer
{
    public class AddAuthTokenHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }
            else
            {
                if (operation.Summary == "获取其他台账")
                {

                }
                foreach (var parameter in operation.Parameters)
                {
                    var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                    bool hasDefaultValue = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerParameterDescriptor)description.ParameterDescriptor).ParameterInfo.HasDefaultValue;
                    if (hasDefaultValue == true || description.Type.Name == "Nullable`1")
                    {
                        parameter.Required = false;
                    }
                    else
                    {
                        parameter.Required = true;
                    }
                }
            }
            //operation.Parameters.Add(new OpenApiParameter()
            //{
            //    Name = "Token",
            //    Description = "Token",
            //    Required = false,
            //    In = ParameterLocation.Header,//query header body path formData
            //    Schema = new OpenApiSchema()
            //    {
            //        Type = "string",
            //    },
            //});
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "access_token",
                Description = "Web端获取UniWater用户信息所需",
                Required = false,
                In = ParameterLocation.Header,//query header body path formData
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                },
            });
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "app",
                Description = "App端获取UniWater用户信息所需",
                Required = false,
                In = ParameterLocation.Header,//query header body path formData
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                },
            });
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "token",
                Description = "App端获取UniWater用户信息所需",
                Required = false,
                In = ParameterLocation.Header,//query header body path formData
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                },
            });
            operation.Parameters.Add(new OpenApiParameter()
            {
                Name = "uniwater_url",
                Description = "动态UniWater地址",
                Required = false,
                In = ParameterLocation.Header,//query header body path formData
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                },
            });
        }
    }
}
