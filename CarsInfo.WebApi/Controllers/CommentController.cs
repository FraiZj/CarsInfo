using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.WebApi.Extensions;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels;
using CarsInfo.WebApi.ViewModels.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarsInfo.WebApi.Controllers
{
    [Route("/cars/{carId}/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly CommentControllerMapper _mapper;

        public CommentController(
            ICommentService commentService, 
            CommentControllerMapper mapper)
        {
            _commentService = commentService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(int carId)
        {
            var operation = await _commentService.GetByCarIdAsync(carId);

            if (!operation.Success)
            {
                return operation.IsException ? 
                    BadRequest("An error occured") :
                    BadRequest(operation.FailureMessage);
            }
            
            var comments = _mapper.MapToCommentViewModels(operation.Result);
            return Ok(comments);
        }
        
        [HttpPost, Authorize(Roles = Roles.User)]
        public async Task<IActionResult> Create(int carId, CommentEditorViewModel comment)
        {
            var userId = User.GetUserId();

            if (!userId.HasValue)
            {
                return BadRequest("Cannot get user id");
            }
            
            var commentDto = _mapper.MapToCommentEditorDto(comment);
            commentDto.CarId = carId;
            commentDto.UserId = User.GetUserId()!.Value;
            var operation = await _commentService.AddAsync(commentDto);

            if (operation.IsException)
            {
                return BadRequest("An error occured");
            }
            
            return operation.Success ?
                Ok("Comment added") :
                BadRequest(operation.FailureMessage);
        }
    }
}