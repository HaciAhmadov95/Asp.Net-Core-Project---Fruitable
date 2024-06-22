using FRUITABLE.Data;
using FRUITABLE.Models;
using FRUITABLE.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace FRUITABLE.Services
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;

        public CommentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comments>> GetAllAsync()
        {
            return await _context.Comments.Include(m => m.AppUser).ToListAsync();
        }

        public async Task<Comments> GetById(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task AddAsync(Comments comment)
        {
            await _context.AddAsync(comment);
        }

        public void Delete(Comments comment)
        {
            _context.Comments.Remove(comment);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
