using LaserAPI.Interfaces.Dal;
using LaserAPI.Models.Dto.RegisteredLaser;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace LaserAPI.Dal
{
    public class RegisteredLaserDal(DataContext _context) : IRegisteredLaserDal
    {
        public async Task Add(RegisteredLaserDto registeredLaser)
        {
            await _context.RegisteredLaser.AddAsync(registeredLaser);
            await _context.SaveChangesAsync();
        }

        public async Task<RegisteredLaserDto> Find(string laserId)
        {
            return await _context.RegisteredLaser.FirstOrDefaultAsync(rl => rl.LaserId == laserId);
        }

        public async Task Remove(Guid uuid)
        {
            RegisteredLaserDto registeredLaserDto = await _context.RegisteredLaser.SingleAsync(rl => rl.Uuid == uuid);
            _context.RegisteredLaser.Remove(registeredLaserDto);
            await _context.SaveChangesAsync();
        }

        public async Task Update(RegisteredLaserDto registeredLaser)
        {
            await Remove(registeredLaser.Uuid);
            await Add(registeredLaser);
        }
    }
}
