namespace ServerApp.DTOs
{
    public class ImagesForDetailsDTO
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsProfile { get; set; }
    }
}