using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tickest.Persistence.Interceptorr;

internal class SetCreateDateOnInsert : SaveChangesInterceptor
{
    public override int SaveChanges(SaveChangesContext context)
    {
        SetAuditData(context);
        return base.SaveChanges(context);
    }

    public override Task<int> SaveChangesAsync(SaveChangesContext context, CancellationToken cancellationToken = default)
    {
        SetAuditData(context);
        return base.SaveChangesAsync(context, cancellationToken);
    }

    private void SetAuditData(SaveChangesContext context)
    {
        // Obter todas as entidades rastreadas pelo contexto
        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            // Verifica se a entidade possui a propriedade DataAlteracao (Data de alteração)
            if (entry.Entity is IBaseEntity baseEntity)
            {
                // Se a entidade foi adicionada, define a data de criação
                if (entry.State == EntityState.Added)
                {
                    baseEntity.DataCriacao = DateTime.UtcNow;
                }
                // Se a entidade foi modificada, define a data de alteração
                else if (entry.State == EntityState.Modified)
                {
                    baseEntity.DataAlteracao = DateTime.UtcNow;
                }
            }
        }
    }
}
