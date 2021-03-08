using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EFCoreCosmosDbJsonExtensions
{
    public static class CosmosDbJsonBuilderExtensions
    {
        public static void ToJsonProperty<T>(this PropertyBuilder<T> builder) where T : class
        {
            builder.ToJsonProperty(FirstCharToLowerCase(builder.Metadata.Name));
        }

        public static void ToJsonProperties<T>(this EntityTypeBuilder<T> builder) where T : class
        {
            foreach (var property in GetProperties(builder))
            {
                builder.Property(property.Name).ToJsonProperty(FirstCharToLowerCase(property.Name));
            }
        }

        public static void ToJsonProperties<T, D>(this OwnedNavigationBuilder<T, D> builder)
            where T : class
            where D : class
        {
            builder.ToJsonProperty(FirstCharToLowerCase(builder.Metadata.GetNavigation(false).Name));
            foreach (var property in GetProperties(builder))
            {
                builder.Property(property.Name).ToJsonProperty(FirstCharToLowerCase(property.Name));
            }
        }

        private static IEnumerable<PropertyInfo> GetProperties<T>(EntityTypeBuilder<T> builder) where T : class
        {
            return typeof(T).GetProperties().Where(a => a.CanWrite && a.CanRead
                 && builder.Metadata.FindDeclaredNavigation(a.Name) == null);
        }

        private static IEnumerable<PropertyInfo> GetProperties<T, D>(OwnedNavigationBuilder<T, D> _)
            where T : class
            where D : class
        {
            return typeof(D).GetProperties().Where(a => a.CanWrite && a.CanRead);
        }

        private static string FirstCharToLowerCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            var firstChar = input.Substring(0, 1).ToLower();

            if (input.Length == 1)
            {
                return firstChar;
            }

            return firstChar + new string(input.Skip(1).ToArray());
        }
    }
}
