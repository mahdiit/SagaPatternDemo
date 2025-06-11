using BookingApi.Saga;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingApi.Database
{
    public class BookingSagaMap : SagaClassMap<BookingSagaData>
    {
        protected override void Configure(EntityTypeBuilder<BookingSagaData> entity, ModelBuilder model)
        {
            entity.HasKey(x => x.CorrelationId);

            entity.Property(x => x.CurrentState).HasMaxLength(100);
            entity.Property(x => x.TravelerId);
            entity.Property(x => x.Email).HasMaxLength(200);
            entity.Property(x => x.HotelName).HasMaxLength(200);
            entity.Property(x => x.FlightCode).HasMaxLength(200);
            entity.Property(x => x.CarPlateNumber).HasMaxLength(200);
            entity.Property(x => x.HotelBooked);
            entity.Property(x => x.FlightBooked);
            entity.Property(x => x.CarRented);
            entity.Property(x => x.BookingFinished);

            entity.Property(x => x.Version).IsRowVersion();
        }
    }

}
