namespace FastDinner.Infrastructure.Persistence;

public class DatabaseConfig
{
    public static string SectionName => "DatabaseConfig";

    public string ConnectionString { get; set; }
}