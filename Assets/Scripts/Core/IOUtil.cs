using System;
using System.IO;

public static class IOUtil
{
    public static FileStream TruncateOpen( string path , FileMode mode )
    {
        if( mode == FileMode.Truncate && !File.Exists(path) )
        {
            return new FileStream(path, FileMode.OpenOrCreate);
        }
        return new FileStream(path, mode);        
    }
}
