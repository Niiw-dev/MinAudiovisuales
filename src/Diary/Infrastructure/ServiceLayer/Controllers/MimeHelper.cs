namespace MinAudiovisual.Diary.Infrastructure.ServiceLayer.Controllers;

public static class MimeHelper
{
    public static string DetectMime(byte[] fileBytes)
    {
        if (fileBytes.Length < 4) return "unknown";

        if (fileBytes[0] == 0xFF && fileBytes[1] == 0xD8)
            return "image/jpeg";
        if (fileBytes[0] == 0x89 && fileBytes[1] == 0x50 &&
            fileBytes[2] == 0x4E && fileBytes[3] == 0x47)
            return "image/png";
        if (fileBytes[0] == 0x47 && fileBytes[1] == 0x49 &&
            fileBytes[2] == 0x46 && fileBytes[3] == 0x38)
            return "image/gif";
        if (fileBytes[0] == 0x42 && fileBytes[1] == 0x4D)
            return "image/bmp";
        if (fileBytes[0] == 0x52 && fileBytes[1] == 0x49 &&
            fileBytes[2] == 0x46 && fileBytes[3] == 0x46)
            return "image/webp";

        return "unknown";
    }
}
