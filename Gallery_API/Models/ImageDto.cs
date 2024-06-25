using Microsoft.AspNetCore.Http;

namespace Gallery_API.Models
{
    public class ImageDto
    {
        public int Id { get; set; }
        public int User_Id { get; set; }
        public string Filename { get; set; }
        public string Description { get; set; }
        public DateTime Created_At { get; set; }
        public byte[] Image_Data { get; set; } // Byte array for image data

        public IFormFile ImageFile { get; set; } // IFormFile property for file upload
    }
}
