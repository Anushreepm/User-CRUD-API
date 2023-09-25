using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersCRUDApp.Data;
using UsersCRUDApp.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserContext _context;

    public UsersController(UserContext context)
    {
        _context = context;
    }

    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> Get()
    {
        var users = await _context.Users.ToListAsync();
        return Ok(users);
    }

    // GET: api/Users/1
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return Ok(user);
    }

    // POST: api/Users
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] User user)
    {
        if (ModelState.IsValid)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }
        return BadRequest(ModelState);
    }

    // PUT: api/Users/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] User user)
    {
        if (id != user.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }
        return BadRequest(ModelState);
    }

    // DELETE: api/Users/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    private bool UserExists(int id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}
