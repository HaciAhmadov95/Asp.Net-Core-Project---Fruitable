namespace FRUITABLE.Services.Interface
{
    public interface ISettingsService
    {
        Task<Dictionary<string, string>> GetAllAsync();
    }
}
