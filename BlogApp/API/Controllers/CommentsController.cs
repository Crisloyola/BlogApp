using Microsoft.AspNetCore.Mvc;

using BlogApp.Application.DTOs.Comment;
using BlogApp.Application.Interfaces;
using BlogApp.Domain.Exceptions;

namespace BlogApp.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAll()
        {
            var comments = await _commentService.GetAllAsync();
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetById(int id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            return Ok(comment);
        }

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetByPostId(int postId)
        {
            try
            {
                var comments = await _commentService.GetByPostIdAsync(postId);
                return Ok(comments);

            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetByStatus(string status)
        {
            try
            {
                var comments = await _commentService.GetByStatusAsync(status);
                return Ok(comments);
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var comments = await _commentService.GetByDateRangeAsync(startDate, endDate);
                return Ok(comments);
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CommentDto>> Create([FromBody] CreateCommentDto dto)
        {
            try
            {
                var comment = await _commentService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = comment.Id }, comment);
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
        public async Task<ActionResult<CommentDto>> Update(int id, [FromBody] UpdateCommentDto dto)
        {
            try
            {
                var comment = await _commentService.UpdateAsync(id, dto);
                return Ok(comment);
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

        [HttpPatch("{id}/approve")]
        public async Task<ActionResult<CommentDto>> Approve(int id)
        {
            try
            {
                var comment = await _commentService.ApproveAsync(id);
                return Ok(comment);
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
                await _commentService.DeleteAsync(id);
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