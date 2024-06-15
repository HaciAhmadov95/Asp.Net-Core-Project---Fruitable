namespace FRUITABLE.Models
{
    public class Review : BaseEntity
    {
        public string Message { get; set; }
        public DateTime CreateDate { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }


    }
}
