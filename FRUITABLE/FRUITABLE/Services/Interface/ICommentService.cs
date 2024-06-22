using FRUITABLE.Models;

namespace FRUITABLE.Services.Interface
{
    public interface ICommentService
    {
        Task<IEnumerable<Comments>> GetAllAsync();
        Task<Comments> GetById(int id);
        Task AddAsync(Comments comment);
        void Delete(Comments comment);
        Task SaveAsync();
    }
}
