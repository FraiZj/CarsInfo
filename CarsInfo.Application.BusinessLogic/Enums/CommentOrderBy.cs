using CarsInfo.Application.Persistence.Enums;
using CarsInfo.Application.Persistence.Filters;

namespace CarsInfo.Application.BusinessLogic.Enums
{
    public static class CommentOrderBy
    {
        public const string PublishDateAsc = "PublishDateAsc";

        public const string PublishDateDesc = "PublishDateDesc";

        public static SortingField ConvertToSortingField(string orderBy)
        {
            return orderBy switch
            {
                PublishDateAsc => new SortingField("Comment.PublishDate"),
                PublishDateDesc => new SortingField("Comment.PublishDate", Order.Descending),
                _ => null
            };
        }
    }
}
