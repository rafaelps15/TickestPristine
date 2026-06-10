using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Tickest.Persistence.Data;

internal static class ModelBuilderSnakeCaseExtensions
{
    public static void UseSnakeCaseNames(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();

            if (!string.IsNullOrWhiteSpace(tableName))
            {
                entityType.SetTableName(ToSnakeCase(tableName));
            }

            foreach (var property in entityType.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.GetColumnName()));
            }

            foreach (var key in entityType.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName() ?? string.Empty));
            }

            foreach (var foreignKey in entityType.GetForeignKeys())
            {
                foreignKey.SetConstraintName(ToSnakeCase(foreignKey.GetConstraintName() ?? string.Empty));
            }

            foreach (var index in entityType.GetIndexes())
            {
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName() ?? string.Empty));
            }
        }
    }

    private static string ToSnakeCase(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var builder = new StringBuilder(value.Length + 10);

        for (var i = 0; i < value.Length; i++)
        {
            var current = value[i];

            if (current is '-' or ' ')
            {
                AppendSeparator(builder);
                continue;
            }

            if (current == '_')
            {
                AppendSeparator(builder);
                continue;
            }

            if (char.IsUpper(current))
            {
                var hasPrevious = i > 0;
                var nextIsLower = i + 1 < value.Length && char.IsLower(value[i + 1]);
                var previousIsLowerOrDigit = hasPrevious && (char.IsLower(value[i - 1]) || char.IsDigit(value[i - 1]));
                var previousIsUpper = hasPrevious && char.IsUpper(value[i - 1]);

                if (hasPrevious && (previousIsLowerOrDigit || previousIsUpper && nextIsLower))
                {
                    AppendSeparator(builder);
                }

                builder.Append(char.ToLowerInvariant(current));
                continue;
            }

            builder.Append(char.ToLowerInvariant(current));
        }

        return builder.ToString();
    }

    private static void AppendSeparator(StringBuilder builder)
    {
        if (builder.Length > 0 && builder[^1] != '_')
        {
            builder.Append('_');
        }
    }
}
