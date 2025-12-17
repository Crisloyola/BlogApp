using Microsoft.AspNetCore.Mvc;
using BlogApp.Application.DTOs.Post;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Exceptions;

namespace BlogApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetAll()
        {
            var posts = await _postService.GetAllAsync();
            return Ok(posts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PostDto>> GetById(int id)
        {
            var post = await _postService.GetByIdAsync(id);
            return Ok(post);
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetByAuthorId(int authorId)
        {
            try
            {
                var posts = await _postService.GetByAuthorIdAsync(authorId);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("category/{category}")]
        public async Task<ActionResult<IEnumerable<PostDto>>> GetByCategory(string category)
        {
            var posts = await _postService.GetByCategoryAsync(category);
            return Ok(posts);
        }

        [HttpPost]
        public async Task<ActionResult<PostDto>> Create([FromBody] CreatePostDto dto)
        {
            try
            {
                var post = await _postService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = post.Id }, post);
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

        [HttpPut("{id}")]
        public async Task<ActionResult<PostDto>> Update(int id, [FromBody] UpdatePostDto dto)
        {
            try
            {
                var post = await _postService.UpdateAsync(id, dto);
                return Ok(post);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _postService.DeleteAsync(id);
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
}