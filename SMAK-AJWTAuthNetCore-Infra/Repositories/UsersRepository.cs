using Microsoft.Extensions.Logging;
using SMAK_AJWTAuthNetCore_Core.Entities;
using SMAK_AJWTAuthNetCore_Core.Interfaces;
using SMAK_AJWTAuthNetCore_Infra.Data;

namespace SMAK_AJWTAuthNetCore_Infra.Repositories
{
    public class UsersRepository : IUsersRepository<RegisterRequestModel>, IDisposable
    {
        private readonly AppDbContext _appDbContext;
        private readonly ILogger _logger;

        public UsersRepository(ILogger<RegisterRequestModel> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext ?? throw new ArgumentNullException(nameof(appDbContext));
        }

        // Create a new user
        public async Task<RegisterRequestModel> Create(RegisterRequestModel appuser)
        {
            try
            {
                if (appuser != null)
                {
                    var obj = _appDbContext.Add<RegisterRequestModel>(appuser);
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
        public void Delete(RegisterRequestModel appuser)
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
        public IEnumerable<RegisterRequestModel> GetAll()
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
        public RegisterRequestModel GetById(string Id)
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
        public void Update(RegisterRequestModel appuser)
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

        // Dispose the repository
        public void Dispose()
        {
            _appDbContext.Dispose();
        }
    }
}
