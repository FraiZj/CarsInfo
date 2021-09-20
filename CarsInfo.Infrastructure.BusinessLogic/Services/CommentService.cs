using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.OperationResult;
using CarsInfo.Application.Persistence.Contracts;
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
        private readonly IFilterService _filterService;

        public CommentService(
            ICarsRepository carsRepository,
            IGenericRepository<Comment> commentRepository, 
            CommentServiceMapper mapper,
            ILogger<CommentService> logger,
            IFilterService filterService)
        {
            _carsRepository = carsRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _logger = logger;
            _filterService = filterService;
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
                _logger.LogError(e, $"An error occurred while adding comment");
                return OperationResult.ExceptionResult();
            }
        }

        public async Task<OperationResult<IEnumerable<CommentDto>>> GetByCarIdAsync(int carId, CommentFilterDto filterDto)
        {
            try
            {
                var filter = _filterService.ConfigureCommentFilter(carId, filterDto);
                var comments = await _commentRepository.GetAllAsync(filter);

                return OperationResult<IEnumerable<CommentDto>>.SuccessResult(_mapper.MapToCommentsDtos(comments));
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while fetching comments with carId={carId}");
                return OperationResult<IEnumerable<CommentDto>>.ExceptionResult();
            }
        }
    }
}
