namespace CarsInfo.Application.BusinessLogic.Dtos
{
    public class CommentEditorDto
    {
        public string Text { get; set; }
        
        public int CarId { get; set; }
        
        public int UserId { get; set; }
    }
}