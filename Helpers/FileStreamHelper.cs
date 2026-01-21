namespace Imaginator.Helpers;

public static class FileStreamHelper
{
    public static FileStream OpenRead(string source) => 
        new(
            source, 
            FileMode.Open, 
            FileAccess.Read, 
            FileShare.Read, 
            Constants.DefaultBufferSize, 
            true
        );
}
