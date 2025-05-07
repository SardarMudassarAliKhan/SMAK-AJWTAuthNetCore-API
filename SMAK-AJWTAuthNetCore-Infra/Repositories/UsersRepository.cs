using Microsoft.Extensions.Logging;
using SMAK_AJWTAuthNetCore_Core.Entities;
using SMAK_AJWTAuthNetCore_Core.Interfaces;
using SMAK_AJWTAuthNetCore_Infra.Data;

namespace SMAK_AJWTAuthNetCore_Infra.Repositories
{
    public class UsersRepository : IUsersRepository<ApplicationUser>, IDisposable
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger _logger;

        public UsersRepository(ILogger<ApplicationUser> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        // Create a new user
        public async Task<ApplicationUser> Create(ApplicationUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Add<ApplicationUser>(appuser);
                    await _appDbContext.SaveChangesAsync();
                    return obj.Entity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");
                throw;
            }
        }

        // Delete an existing user
        public void Delete(ApplicationUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Remove(appuser);
                    if (obj != null)
                    {
                        _appDbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user");
                throw;
            }
        }

        // Retrieve all users
        public IEnumerable<ApplicationUser> GetAll()
        {
            try
            {
                var obj = _appDbContext.ApplicationUsers.ToList();
                if (obj != null)
                    return obj;
                else
                    return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all users");
                throw;
            }
        }

        // Retrieve user by ID
        public ApplicationUser GetById(int Id)
        {
            try
            {
                if (Id != null)
                {
                    var Obj = _appDbContext.ApplicationUsers.FirstOrDefault(x => x.Id == Id);
                    if (Obj != null)
                        return Obj;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving user by Id");
                throw;
            }
        }

        // Update an existing user
        public void Update(ApplicationUser appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Update(appuser);
                    if (obj != null)
                        _appDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user");
                throw;
            }
        }

        public ApplicationUser GetByUserName(string userName)
        {
            try
            {
                if (userName!=null)
                {
                    var obj = _appDbContext.ApplicationUsers.FirstOrDefault(x => x.UserName == userName);
                    if (obj != null)
                        return obj;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user");
                throw;
            }
        }

        public ApplicationUser GetByEmail(string email)
        {
            try
            {
                if (email != null)
                {
                    var obj = _appDbContext.ApplicationUsers.FirstOrDefault(x => x.Email == email);
                    if (obj != null)
                        return obj;
                    else
                        return null;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user");
                throw;
            }
        }

        // Dispose the repository
        public void Dispose()
        {
            _appDbContext.Dispose();
        }

        
    }
}
