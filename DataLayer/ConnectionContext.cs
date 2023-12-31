﻿using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataLayer
{
    public class ConnectionContext : IDb<Connection, int>
    {
        private readonly TrainDbContext dbContext;

        public ConnectionContext(TrainDbContext dbContext)
            => this.dbContext = dbContext;

        public async Task CreateAsync(Connection item)
        {
            try
            {
                Location locationA = await dbContext.Locations.FindAsync(item.NodeAId);
                if(locationA != null) item.NodeA = locationA;

                Location locationB = dbContext.Locations.Find(item.NodeBId);
                if (locationB != null) item.NodeB = locationB;

                dbContext.Connections.Add(item);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task<Connection> ReadAsync(int key, bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Connection> query = dbContext.Connections;

                if (useNavigationalProperties)
                {
                    query = query.Include(p => p.NodeA)
                                .Include(p => p.NodeB);
                }

                if (isReadOnly)
                {
                    query = query.AsNoTrackingWithIdentityResolution();
                }

                return await query.FirstOrDefaultAsync(p => p.Id == key);
            }
            catch (Exception) { throw; }
        }

        public async Task<ICollection<Connection>> ReadAllAsync(bool useNavigationalProperties = false, bool isReadOnly = true)
        {
            try
            {
                IQueryable<Connection> query = dbContext.Connections;

                if (useNavigationalProperties)
                {
                    query = query.Include(p => p.NodeA)
                                .Include(p => p.NodeB);
                }

                if (isReadOnly)
                {
                    query = query.AsNoTrackingWithIdentityResolution();
                }

                return await query.ToListAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task UpdateAsync(Connection item, bool useNavigationalProperties = false)
        {
            try
            {
                Connection connectionFromDb = await dbContext.Connections.FindAsync(item.Id);

                if (connectionFromDb == null)
                {
                    await CreateAsync(item);
                    return;
                }

                dbContext.Entry(connectionFromDb).CurrentValues.SetValues(item);

                if (!useNavigationalProperties)
                {
                    await dbContext.SaveChangesAsync();
                    return;
                }
                
                Location locationA = await dbContext.Locations.FindAsync(item.NodeAId);
                if (locationA != null) connectionFromDb.NodeA = locationA;

                Location locationB = await dbContext.Locations.FindAsync(item.NodeBId);
                if (locationB != null) connectionFromDb.NodeB = locationB;

                await dbContext.SaveChangesAsync();
            }
            catch (Exception) { throw; }
        }

        public async Task DeleteAsync(int key)
        {
            try
            {
                Connection connectionFromDb = await ReadAsync(key, false, false)
                    ?? throw new InvalidOperationException("Connection with the given key does not exist!");

                dbContext.Connections.Remove(connectionFromDb);

                await dbContext.SaveChangesAsync();
            }
            catch (Exception) { throw; }
        }
    }
}
