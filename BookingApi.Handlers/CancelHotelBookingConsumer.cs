using BookingApi.Saga.Messages;
using MassTransit;

namespace BookingApi.Handlers;

public class CancelHotelBookingConsumer : IConsumer<CancelHotelBooking>
{
    public Task Consume(ConsumeContext<CancelHotelBooking> context)
    {
        Console.WriteLine($"Booking hotel cancel for traveler {context.Message.TravelerId}");
        return Task.CompletedTask;
    }
}