using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gallery_API.DataConnection;
using Gallery_API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly DbContextClass _context;

        public ImagesController(DbContextClass context)
        {
            _context = context;
        }

        // GET: api/Images
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ImageDto>>> GetImages()
        {
            return await _context.Images
                .Include(i => i.User)
                .Select(i => new ImageDto
                {
                    Id = i.Id,
                    User_Id = i.User_Id,
                    Filename = i.Filename,
                    Description = i.Description,
                    Created_At = i.Created_At,
                    Image_Data = i.Image_Data // Include image data in DTO
                })
                .ToListAsync();
        }

        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ImageDto>> GetImage(int id)
        {
            var image = await _context.Images
                .Include(i => i.User)
                .Select(i => new ImageDto
                {
                    Id = i.Id,
                    User_Id = i.User_Id,
                    Filename = i.Filename,
                    Description = i.Description,
                    Created_At = i.Created_At,
                    Image_Data = i.Image_Data // Include image data in DTO
                })
                .FirstOrDefaultAsync(i => i.Id == id);

            if (image == null)
            {
                return NotFound();
            }

            return image;
        }

        // PUT: api/Images/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImage(int id, ImageDto imageDto)
        {
            if (id != imageDto.Id)
            {
                return BadRequest();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            // Update properties from DTO
            image.User_Id = imageDto.User_Id;
            image.Filename = imageDto.Filename;
            image.Description = imageDto.Description;
            image.Created_At = imageDto.Created_At;
            image.Image_Data = imageDto.Image_Data; // Update image blob data

            _context.Entry(image).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Images
        [HttpPost]
        public async Task<ActionResult<ImageDto>> PostImage([FromForm] ImageDto imageDto)
        {
            if (imageDto == null || imageDto.ImageFile == null || imageDto.ImageFile.Length == 0)
            {
                return BadRequest("Image file is required.");
            }

            byte[] imageBytes;

            using (var ms = new MemoryStream())
            {
                await imageDto.ImageFile.CopyToAsync(ms);
                imageBytes = ms.ToArray();
            }

            var image = new Image
            {
                User_Id = imageDto.User_Id,
                Filename = imageDto.ImageFile.FileName, // Use filename from uploaded file
                Description = imageDto.Description,
                Created_At = DateTime.UtcNow,
                Image_Data = imageBytes // Store image blob data
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            imageDto.Id = image.Id;

            return CreatedAtAction(nameof(GetImage), new { id = imageDto.Id }, imageDto);
        }

        // DELETE: api/Images/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
