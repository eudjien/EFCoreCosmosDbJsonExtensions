# EFCoreCosmosDbJsonExtensions
EntityFrameworkCore Extensions which allows to map all properties to JSON format when targeting CosmosDb in Fluent configuration.

> [NuGet package here](https://www.nuget.org/packages/EntityFrameworkCore.CosmosDbJsonExtensions)

Example:
```C#
public void Configure(EntityTypeBuilder<Post> builder)
{
    builder.ToContainer("Posts");
    builder.HasNoDiscriminator();

    builder.HasKey(a => a.Id);
    
    // ...

    builder.ToJsonProperties();
    
    builder.OwnsMany(a => a.Tags, builder => builder.ToJsonProperties());
}
```
