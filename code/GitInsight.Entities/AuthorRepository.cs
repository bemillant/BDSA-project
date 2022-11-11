namespace GitInsight.Entities;

using GitInsight.Core;
using GitInsight;

public class AuthorRepository : IAuthorRepository
{

    private readonly CommitTreeContext context;

    public AuthorRepository(CommitTreeContext context)
    {
        this.context = context;
    }

    public void Create (AuthorCreateDTO author) {

        var entry = new Author(author.Name);
        context.Authors.Add(entry);
        context.SaveChanges();

        Console.WriteLine(author.Name + " has been created");
    }

    public IReadOnlyCollection<AuthorDTO> ReadAll(){
        return context.Authors.Select ( e => new AuthorDTO(e.Id, e.Name )).ToList().AsReadOnly();
    }

    public void Update(){
        // not implemented yet
        // context.Authors.delete
    }

}