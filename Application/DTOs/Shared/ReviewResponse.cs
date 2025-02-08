namespace Application.DTOs.Shared
{
    public class ReviewResponse
    {
        public Guid Id { get; init; }
        public string Content { get; init; }
        public int Rating { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public string GuestName { get; init; }
    }
}
