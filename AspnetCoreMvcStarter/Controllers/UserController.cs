using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;

namespace AspnetCoreMvcStarter.Controllers;

[Route("api/users")]
[ApiController]
public class UserController : ControllerBase
{
  private static Dictionary<String, Object> cached = new Dictionary<string, object>();
  private static string cacheKey = "listUser";
  private readonly ILogger<UserController> _logger;

  public UserController(ILogger<UserController> logger)
  {
    _logger = logger;
  }

  private static List<User> _users = new List<User>()
  {
    new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
    new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" },
    new User { Id = 3, FirstName = "Peter", LastName = "Jones", Email = "peter.jones@example.com" },
    new User { Id = 4, FirstName = "Alice", LastName = "Brown", Email = "alice.brown@example.com" },
    new User { Id = 5, FirstName = "Bob", LastName = "Lee", Email = "bob.lee@example.com" },
    new User { Id = 6, FirstName = "Charlie", LastName = "Davis", Email = "charlie.davis@example.com" },
    new User { Id = 7, FirstName = "Diana", LastName = "Wilson", Email = "diana.wilson@example.com" },
    new User { Id = 8, FirstName = "Eric", LastName = "Moore", Email = "eric.moore@example.com" },
    new User { Id = 9, FirstName = "Fiona", LastName = "Taylor", Email = "fiona.taylor@example.com" },
    new User { Id = 10, FirstName = "George", LastName = "Anderson", Email = "george.anderson@example.com" },
    new User { Id = 11, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
    new User { Id = 12, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" },
    new User { Id = 13, FirstName = "Peter", LastName = "Jones", Email = "peter.jones@example.com" },
    new User { Id = 14, FirstName = "Alice", LastName = "Brown", Email = "alice.brown@example.com" },
    new User { Id = 15, FirstName = "Bob", LastName = "Lee", Email = "bob.lee@example.com" },
    new User { Id = 16, FirstName = "Charlie", LastName = "Davis", Email = "charlie.davis@example.com" },
    new User { Id = 17, FirstName = "Diana", LastName = "Wilson", Email = "diana.wilson@example.com" },
    new User { Id = 18, FirstName = "Eric", LastName = "Moore", Email = "eric.moore@example.com" },
    new User { Id = 19, FirstName = "Fiona", LastName = "Taylor", Email = "fiona.taylor@example.com" },
    new User { Id = 20, FirstName = "George", LastName = "Anderson", Email = "george.anderson@example.com" }
  };

  [HttpGet]
  public IActionResult GetUsers(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10,
    [FromQuery] string? search = null,
    [FromQuery] string? orderColumn = null,
    [FromQuery] string? orderDir = null)
  {
    if (!cached.ContainsKey("listUser"))
    {
      _logger.LogInformation("Get Fresh data");
      cached[cacheKey] = _users; // móc trong db.
    }
    else
    {
      _logger.LogInformation("Get Cache data");
    }
    _users = (List<User>) cached["listUser"];

    var query = _users.AsQueryable(); // móc từ db.
    if (!string.IsNullOrEmpty(search))
    {
      // search theo keyword
      query = query.Where(x => x.LastName.Contains(search, StringComparison.OrdinalIgnoreCase));
    }
    if (!string.IsNullOrEmpty(orderColumn) && !string.IsNullOrEmpty(orderDir))
    {
      // sort by field
      query = query.OrderBy($"{orderColumn} {orderDir}");
    }
    var totalCount = query.Count();
    var pagedProducts = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    var result = new
    {
      data = pagedProducts,
      recordsTotal = _users.Count,
      recordsFiltered = totalCount,
      page = page,
      pageSize = pageSize
    };
    return Ok(result);
  }

  [HttpPost]
  public IActionResult Save(User user)
  {
    // Save and add to cached
    // if (cached.ContainsKey("listUser"))
    // {
    //   var cachedUser = (List<User>) cached[cacheKey];
    //   cachedUser.Add(user);
    //   cached[cacheKey] = cachedUser;
    // }
    cached.Remove(cacheKey);
    return Ok(null);
  }

  // Model User
  public class User
  {
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
  }
}
