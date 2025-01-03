using EntityPoste.Domain;
using EntityPoste.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EntityPoste.Repository;

public class UserRepository(AppDbContext ctx) : IUserRepository, IAsyncDisposable, IDisposable
{
    public async ValueTask DisposeAsync()
    {
        await ctx.DisposeAsync();
    }

    public void Insert(string name, string email)
    {
        ctx.Users.Add(new User(name, email));
    }

    public void Update(int id, string email)
    {
        var user = ctx.Users.Find(id);
        if (user == null) return;
        user.UpdateEmail(email);
    }

    public void Delete(int id)
    {
        var user = ctx.Users.Find(id);
        if (user == null) return;
        ctx.Users.Remove(user);
    }

    public IEnumerable<User> GetUsers() => ctx.Users.ToList();

    //public IEnumerable<User> GetUsersByProvider(string provider) => ctx.Users.Where(u => u.Email.Contains(provider));
    public IEnumerable<User> GetUsersByProvider(string provider)
    {
        return 
            from user in ctx.Users
            join address in ctx.Addresses on user.Id equals address.UserId
            where user.Email.Contains(provider)
            select user;
    }

    public IEnumerable<string> GetProviders() => ctx.Users.Select(u => u.Email.Substring(u.Email.IndexOf("@") + 1)).Distinct();

    public void Dispose() => ctx.Dispose();
    public int SaveChanges() => ctx.SaveChanges();
}