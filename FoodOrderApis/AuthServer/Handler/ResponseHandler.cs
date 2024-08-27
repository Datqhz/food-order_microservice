using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Handler;

public class ResponseHandler
{
      public static ObjectResult Ok(object data = null, string message = "")
        {
            return new ObjectResult(new
            {
                status = StatusCodes.Status200OK,
                statusText = string.IsNullOrEmpty(message) ? "Success" : message,
                data
            })
            { StatusCode = StatusCodes.Status200OK };
        }

        public static ObjectResult Created(object data = null, string message = "")
        {
            return new ObjectResult(new
            {
                status = StatusCodes.Status201Created,
                statusText = string.IsNullOrEmpty(message) ? "Created" : message,
                data
            })
            { StatusCode = StatusCodes.Status201Created };
        }

        public static ObjectResult NoContent(object data = null, string message = "")
        {
            return new ObjectResult(new
            {
                status = StatusCodes.Status204NoContent,
                statusText = string.IsNullOrEmpty(message) ? "No content" : message,
                data
            })
            { StatusCode = StatusCodes.Status204NoContent };
        }

        public static ObjectResult BadRequest(object data = null, string message = "", string messageCode = "")
        {
            message = string.IsNullOrEmpty(message) ? "Bad request" : message;
            return new ObjectResult(new
            {
                status = StatusCodes.Status400BadRequest,
                statusText = message,
                errorMessage = message,
                data
            })
            { StatusCode = StatusCodes.Status400BadRequest };
        }
        
        public static ObjectResult NotFound(object data = null, string message = "")
        {
            return new ObjectResult(new
            {
                status = StatusCodes.Status404NotFound,
                statusText = string.IsNullOrEmpty(message) ? "Not found" : message,
                data
            })
            { StatusCode = StatusCodes.Status404NotFound };
        }
        
        public static ObjectResult Error(object data = null, string message = "")
        {
            return new ObjectResult(new
            {
                status = StatusCodes.Status500InternalServerError,
                statusText = string.IsNullOrEmpty(message) ? "Internal server error" : message,
                data
            })
            { StatusCode = StatusCodes.Status500InternalServerError };
        }
}