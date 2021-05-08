using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsersApi.DataBases;

namespace UsersApi.Repository
{
    public class UsersHandler : IUsersHandler
    {
        private readonly UsersDbContext _context;

        public UsersHandler(UsersDbContext context)
        {
            _context = context;
        }

        public async Task<UserDbo> FindAsync(string name)
        {
            return await _context.Users
                .Where(b => b.Name == name)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(UserDbo userDbo)
        {
            await _context.AddAsync(userDbo);
            await _context.SaveChangesAsync();
        }
    }
}