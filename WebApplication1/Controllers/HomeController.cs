using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
  [ApiController]
  [Route("/")]
  public class HomeController : ControllerBase
  {
    private readonly ILogger<HomeController> _logger;

    private static Dictionary<string, Entity> _storage = new Dictionary<string, Entity>();

    public HomeController(ILogger<HomeController> logger)
    {
      _logger = logger;
    }

    // Solution 1.

    /// <summary>
    /// Adds entity to the storage.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns>Added entity.</returns>
    [HttpPost]
    public Entity Insert(Entity entity)
    {
      if (_storage.TryGetValue(entity.Id.ToString(), out var existingEntity))
      {
        // Entity with this id already exists;
        Response.StatusCode = (int) HttpStatusCode.Conflict;
        return existingEntity;
      }
      else
      {
        _storage.Add(entity.Id.ToString(), entity);
        return entity;
      }
    }

    /// <summary>
    /// Gets entity from the storage.
    /// </summary>
    /// <param name="get">Guid</param>
    /// <returns>Searched entity.</returns>
    [HttpGet]
    public Entity Get([Required, FromQuery] string get)
    {
      if (!Guid.TryParse(get, out _))
      {
        // Bad request;
        Response.StatusCode = (int) HttpStatusCode.BadRequest;
        return null;
      }

      if (!_storage.TryGetValue(get, out var entity))
      {
        // Entity not found;
        Response.StatusCode = (int) HttpStatusCode.NotFound;
        return null;
      }
      else
      {
        return entity;
      }
    }

    // Solution 2 (if need to retrieve a string and deserialize the object manually).

    //// In query, need to replace "+" with "%2b"
    //// http://localhost:37612?insert={"id":"cfaa0d3f-7fea-4423-9f69-ebff826e2f89","operationDate":"2019-04-02T13:10:20.0263632%2b03:00","amount":23.05}

    ///// <summary>
    ///// Adds entity to the storage.
    ///// </summary>
    ///// <param name="entity"></param>
    ///// <returns>Added entity.</returns>
    //[HttpPost]
    //public Entity Insert([Required, FromQuery] string insert)
    //{
    //  try
    //  {
    //    var entity = JsonSerializer.Deserialize<Entity>(
    //      insert,
    //      new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
    //    );

    //    if (_storage.TryGetValue(entity.Id.ToString(), out var existingEntity))
    //    {
    //      // Entity with this id already exists;
    //      Response.StatusCode = (int) HttpStatusCode.Conflict;
    //      return existingEntity;
    //    }
    //    else
    //    {
    //      _storage.Add(entity.Id.ToString(), entity);
    //      return entity;
    //    }
    //  }
    //  catch (JsonException)
    //  {
    //    Response.StatusCode = (int) HttpStatusCode.BadRequest;
    //    return null;
    //  }
    //}

    ///// <summary>
    ///// Gets entity from the storage
    ///// </summary>
    ///// <param name="get">Guid</param>
    ///// <returns>Searched entity.</returns>
    //[HttpGet]
    //public Entity Get([Required, FromQuery] string get)
    //{
    //  if (!Guid.TryParse(get, out _))
    //  {
    //    Response.StatusCode = (int) HttpStatusCode.BadRequest;
    //    return null;
    //  }

    //  if (!_storage.TryGetValue(get, out var entity))
    //  {
    //    Response.StatusCode = (int) HttpStatusCode.NotFound;
    //    return null;
    //  }
    //  else
    //  {
    //    return entity;
    //  }
    //}
  }
}
