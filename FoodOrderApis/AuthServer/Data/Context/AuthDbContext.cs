﻿using AuthServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data.Context;

public class AuthDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientGrantTypes> ClientGrantTypes { get; set; }
    public DbSet<ClientSecrets> ClientSecrets { get; set; }
    public DbSet<ClientScopes> ClientScopes { get; set; }
    public DbSet<ApiResources> ApiResources { get; set; }
    public DbSet<ApiResourceScope> ApiResourceScope { get; set; }
    public AuthDbContext(){}
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options){}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("auth");
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
    }
}
