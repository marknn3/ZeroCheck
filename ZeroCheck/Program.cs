namespace ZeroCheck
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path = args.Length > 0 ? args[0] : Environment.CurrentDirectory;

            Console.WriteLine($"Checking for files with all zero content in path: {path}");

            int zeroFileCount = 0;
            foreach (var file in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    using (var fs = File.OpenRead(file))
                    {
                        var b = fs.ReadByte();
                        if (b == 0)
                        {
                            var bytes = File.ReadAllBytes(file);
                            var allzero = bytes.All(b => b == 0);
                            if (allzero)
                            {
                                zeroFileCount++;
                                Console.WriteLine($"All zero content: {file}");
                            }
                        }
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (PathTooLongException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (IOException ex)
                {
                    if (ex.Message.Contains("another process", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"File in use by another process: {file}");
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            if (zeroFileCount==0)
            {
                Console.WriteLine("No files found with all zero content");
            }
            else
            {
                Console.WriteLine($"{zeroFileCount} files found with all zero content");
            }
        }
    }
}