using Exoft.Gamification.Api.Services.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Exoft.Gamification.Api.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly GamificationDbContext _context;

        public UnitOfWork(GamificationDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
