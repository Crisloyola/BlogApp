using Microsoft.AspNetCore.Mvc;
using BlogApp.Application.DTOs.Author;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Exceptions;

namespace BlogApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll()
    {
        var authors = await _authorService.GetAllAsync();
        return Ok(authors);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorDto>> GetById(int id)
    {
        var author = await _authorService.GetByIdAsync(id);
        return Ok(author);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<AuthorDto>> GetByEmail(string email)
    {
        var author = await _authorService.GetByEmailAsync(email);
        if (author == null)
            return NotFound(new { message = $"Author with email {email} not found." });

        return Ok(author);
    }

    [HttpGet("search/{name}")]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> SearchByName(string name)
    {
        var authors = await _authorService.SearchByNameAsync(name);
        return Ok(authors);
    }

    [HttpPost]
    public async Task<ActionResult<AuthorDto>> Create([FromBody] CreateAuthorDto dto)
    {
        try
        {
            var author = await _authorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = author.Id }, author);
        }
        catch (DuplicateEntityException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AuthorDto>> Update(int id, [FromBody] UpdateAuthorDto dto)
    {
        var author = await _authorService.UpdateAsync(id, dto);
        return Ok(author);
    }
        

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _authorService.DeleteAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (BusinessRuleException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}