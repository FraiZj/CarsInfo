using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.Enums;
using CarsInfo.WebApi.Controllers.Base;
using CarsInfo.WebApi.EmailSender;
using CarsInfo.WebApi.EmailSender.Models;
using CarsInfo.WebApi.Extensions;
using CarsInfo.WebApi.Mappers;
using CarsInfo.WebApi.ViewModels.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SendGrid.Helpers.Mail;

namespace CarsInfo.WebApi.Controllers
{
    [Route("/cars/{carId:int}/comments")]
    public class CommentController : AppController
    {
        private readonly ICommentService _commentService;
        private readonly CommentControllerMapper _mapper;
        private readonly IEmailSender _emailSender;

        public CommentController(
            ICommentService commentService, 
            CommentControllerMapper mapper,
            IEmailSender emailSender)
        {
            _commentService = commentService;
            _mapper = mapper;
            _emailSender = emailSender;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] int carId, [FromQuery] CommentFilterDto filter)
        {
            var operation = await _commentService.GetByCarIdAsync(carId, filter);

            if (!operation.Success)
            {
                return BadRequest(operation.FailureMessage);
            }
            
            var comments = _mapper.MapToCommentViewModels(operation.Result);
            return Ok(comments);
        }
        
        [HttpPost, Authorize(Roles = Roles.User)]
        public async Task<IActionResult> Create([FromRoute] int carId, [FromBody] CommentEditorViewModel comment)
        {
            var userId = User.GetUserId();

            if (!userId.HasValue)
            {
                return BadRequest("Cannot get user id");
            }
            
            var commentDto = _mapper.MapToCommentEditorDto(comment);
            commentDto.CarId = carId;
            commentDto.UserId = userId.Value;
            var operation = await _commentService.AddAsync(commentDto);

            await _emailSender.SendEmailAsync(new EmailModel
            {
                Email = "dania.moom@gmail.com",
                Subject = "Test",
                Message = "Test"
            }, "Test");
            
            return operation.Success ?
                Created("/cars/{carId}/comments", comment) :
                BadRequest(operation.FailureMessage);
        }
    }
}