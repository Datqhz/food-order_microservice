using CustomerService.Data.Context;
using CustomerService.Data.Models;
using CustomerService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Repositories.Implements;

public class UserInfoRepository : GenericRepository<UserInfo> ,IUserInfoRepository
{
    public UserInfoRepository(CustomerDbContext context) : base(context){}
    
}