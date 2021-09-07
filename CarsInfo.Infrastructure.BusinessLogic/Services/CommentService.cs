using System.Collections.Generic;
using System.Threading.Tasks;
using CarsInfo.Application.BusinessLogic.Contracts;
using CarsInfo.Application.BusinessLogic.Dtos;
using CarsInfo.Application.Persistence.Contracts;
using CarsInfo.Application.Persistence.Filters;
using CarsInfo.Domain.Entities;
using CarsInfo.Infrastructure.BusinessLogic.Mappers;

namespace CarsInfo.Infrastructure.BusinessLogic.Services
{
    public class CommentService : ICommentService
    {
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly CommentServiceMapper _mapper;

        public CommentService(
            IGenericRepository<Comment> commentRepository, 
            CommentServiceMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task AddAsync(CommentDto commentDto)
        {
            var comment = _mapper.MapToComment(commentDto);
            await _commentRepository.AddAsync(comment);
        }

        public async Task GetByCarIdAsync(int carId)
        {
            var filter = new FilterModel()
            {
                Filters = new List<FiltrationField>
                {
                    new("Comment.CarId", carId)
                }
            };
            await _commentRepository.GetAllAsync(filter);
        }
    }
}
