using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.OperationResult;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;
using Microsoft.Extensions.Logging;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICarsRepository _carsRepository;
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly CommentServiceMapper _mapper;
        private readonly ILogger<CommentService> _logger;

        public CommentService(
            ICarsRepository carsRepository,
            IGenericRepository<Comment> commentRepository, 
            CommentServiceMapper mapper,
            ILogger<CommentService> logger)
        {
            _carsRepository = carsRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<OperationResult> AddAsync(CommentEditorDto commentDto)
        {
            try
            {
                var car = await _carsRepository.GetAsync(commentDto.CarId);
                if (car is null)
                {
                    return OperationResult.FailureResult($"Car with id={commentDto.CarId} does not exist");
                }
                
                var comment = _mapper.MapToComment(commentDto);
                comment.PublishDate = DateTimeOffset.Now;
                await _commentRepository.AddAsync(comment);
                
                return OperationResult.SuccessResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occured while adding comment");
                return OperationResult.ExceptionResult(e);
            }
        }

        public async Task<OperationResult<IEnumerable<CommentDto>>> GetByCarIdAsync(int carId)
        {
            try
            {
                var filter = new FilterModel
                {
                    Filters = new List<FiltrationField>
                    {
                        new("Comment.CarId", carId)
                    }
                };
                var comments = await _commentRepository.GetAllAsync(filter);

                return OperationResult<IEnumerable<CommentDto>>.SuccessResult(_mapper.MapToCommentsDtos(comments));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occured while fetching comments with CarId={carId}");
                return OperationResult<IEnumerable<CommentDto>>.ExceptionResult(e);
            }
        }
    }
}
