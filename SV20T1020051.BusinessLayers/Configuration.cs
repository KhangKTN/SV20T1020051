namespace SV20T1020051.BusinessLayers;

public class Configuration
{
    public static string ConnectionString { get; private set; } = "";

    public static void Initialize(string connectionString)
    {
        Configuration.ConnectionString = connectionString;
    }

}

