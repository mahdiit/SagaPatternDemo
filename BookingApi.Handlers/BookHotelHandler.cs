﻿using BookingApi.Database;
using BookingApi.Saga.Messages;
using MassTransit;

namespace BookingApi.Handlers;

public class BookHotelHandler(BookingDbContext dbContext) : IConsumer<BookHotel>
{
    public async Task Consume(ConsumeContext<BookHotel> context)
    {
        Console.WriteLine($"Booking hotel {context.Message.HotelName} for traveler {context.Message.Email}");

        var traveler = new Traveler
        {
            Id = Guid.NewGuid(),
            Email = context.Message.Email,
            BookedOn = DateTime.Now
        };

        dbContext.Travelers.Add(traveler);

        await dbContext.SaveChangesAsync();

        await context.Publish(new HotelBooked
        {
            TravelerId = traveler.Id,
            HotelName = context.Message.HotelName,
            FlightCode = context.Message.FlightCode,
            CarPlateNumber = context.Message.CarPlateNumber,
            Email = context.Message.Email
        });
    }
}