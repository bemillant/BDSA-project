namespace Models;
public partial class Database
{
    internal const string ConnectionStringName = "GitInsight";

    // Creates and seeds the database if it does not already exist.
    public static void SetupDatabase(CommitTreeContext context)
    {
        Console.WriteLine("Database.cs line 9, SetupDatabase()");
        if (context.Database.GetService<IRelationalDatabaseCreator>().Exists())
        {   // If the database exists, just apply migrations.
            context.Database.Migrate();
        }
        else
        {   // Otherwise, create and seed the database.
            context.Database.EnsureDeleted();
            // Clean migrations.
            var migrator = context.GetService<IMigrator>();
            migrator.Migrate("0");
            context.Database.Migrate();
        }
    }

    // Gets the dafault options which should be used when not testing.
    public static DbContextOptionsBuilder<CommitTreeContext> GetOptionsBuilder()
    {
        var optionsBuilder = new DbContextOptionsBuilder<CommitTreeContext>()
            .UseSqlServer(GetConnectionString());
        return optionsBuilder;
    }

    // Gets the connection string named 'GitInsight' in *LitExplore.Repository*
    public static string GetConnectionString()
    {
        var configuration = LoadConfiguration();
        var connectionString = configuration.GetConnectionString(ConnectionStringName);
        return connectionString!;
    }

    public static IConfiguration LoadConfiguration()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Database>().Build();

        return configuration;
    }
}