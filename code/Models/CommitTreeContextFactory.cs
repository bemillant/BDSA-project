namespace Models;

public class CommitTreeContextFactory : IDesignTimeDbContextFactory<CommitTreeContext>
{

    public CommitTreeContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddUserSecrets<Database>()
            .AddJsonFile("appsettings.json").Build();

        var connectionString = configuration.GetConnectionString("ConnectionString");

        String connectionStringHardcode = configuration.GetConnectionString(Database.ConnectionStringName)!;
        //"Server=localhost;Database=youthful_keller;User Id=sa;Password=Gruppen123;Trusted_Connection=False;Encrypt=False";
        
        var optionsBuilder = new DbContextOptionsBuilder<CommitTreeContext>();
        optionsBuilder.UseSqlServer(connectionStringHardcode);

        return new CommitTreeContext(optionsBuilder.Options);
    }

    
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}