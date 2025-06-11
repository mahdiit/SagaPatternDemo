using BookingApi.Saga.Messages;
using MassTransit;

namespace BookingApi.Handlers;

public class CancelFlightBookingConsumer : IConsumer<CancelFlightBooking>
{
    public Task Consume(ConsumeContext<CancelFlightBooking> context)
    {
        Console.WriteLine($"Booking flight cancel for traveler {context.Message.TravelerId}");
        return Task.CompletedTask;
    }
}