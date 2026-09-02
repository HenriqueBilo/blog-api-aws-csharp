namespace BlogApi.Services;

public interface IImageService
{
    Task<string> UploadAsync(IFormFile file, string postId);
}
