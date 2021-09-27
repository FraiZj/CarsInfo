using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.BusinessLogic.OperationResult;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICarsRepository _carsRepository;
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly CommentServiceMapper _mapper;
        private readonly IFilterService _filterService;

        public CommentService(
            ICarsRepository carsRepository,
            IGenericRepository<Comment> commentRepository, 
            CommentServiceMapper mapper,
            IFilterService filterService)
        {
            _carsRepository = carsRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
            _filterService = filterService;
        }

        public async Task<OperationResult> AddAsync(CommentEditorDto commentDto)
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

        public async Task<OperationResult<IEnumerable<CommentDto>>> GetByCarIdAsync(int carId, CommentFilterDto filterDto)
        {
            var filter = _filterService.ConfigureCommentFilter(carId, filterDto);
            var comments = await _commentRepository.GetAllAsync(filter);

            return OperationResult<IEnumerable<CommentDto>>.SuccessResult(_mapper.MapToCommentsDtos(comments));
        }
    }
}
