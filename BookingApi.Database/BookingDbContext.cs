using BookingApi.Saga;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Database;

public class BookingDbContext(DbContextOptions<BookingDbContext> options) : SagaDbContext(options)
{
    public DbSet<Traveler> Travelers { get; set; }

    public DbSet<BookingSagaData> BookingSagaData { get; set; }

    protected override IEnumerable<ISagaClassMap> Configurations => [new BookingSagaMap()];
}