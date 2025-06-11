namespace BookingApi.Saga.Messages;

public record HotelBooked
{
    public Guid TravelerId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string HotelName { get; set; } = string.Empty;
    public string FlightCode { get; set; } = string.Empty;
    public string CarPlateNumber { get; set; } = string.Empty;
}

public record FlightBooked
{
    public Guid TravelerId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FlightCode { get; set; } = string.Empty;
    public string CarPlateNumber { get; set; } = string.Empty;
}

public record FlightBookingFailed
{
    public Guid TravelerId { get; set; }
    public string Reason { get; set; }
};

public record CarRented
{
    public Guid TravelerId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string CarPlateNumber { get; set; } = string.Empty;
}

public record CarRentingFailed
{
    public Guid TravelerId { get; set; }
    public string Reason { get; set; }
};

public record BookingCompleted
{
    public Guid TravelerId { get; set; }
    public string Email { get; set; } = string.Empty;
}

