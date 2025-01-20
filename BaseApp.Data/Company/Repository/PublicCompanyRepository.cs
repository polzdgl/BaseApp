using BaseApp.Data.Company.Interfaces;
using BaseApp.Data.Company.Models;
using BaseApp.Data.Context;
using BaseApp.Data.Repositories;

namespace BaseApp.Data.Company.Repository
{
    public class PublicCompanyRepository : GenericRepository<PublicCompany>, IPublicCompanyRepository
    {
        public PublicCompanyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
