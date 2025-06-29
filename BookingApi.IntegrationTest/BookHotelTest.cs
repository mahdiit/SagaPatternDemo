using BookingApi.Saga.Messages;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.IntegrationTest
{
    public class BookHotelTest : BaseIntegrationTest
    {
        public BookHotelTest(IntegrationTestWebAppFactory factory) : base(factory)
        {

        }

        [Fact]
        public async Task BookHotel_WhenSuccess_MustHaveDataInDb()
        {
            var id = Guid.CreateVersion7() + "@email.com";
            var book = new BookHotel(id, "HotelName", "FlightCode", "CarPlateNumber");

            await Sender.Publish(book);
            await Task.Delay(5000);

            var traveler = await DbContext.Travelers.FirstOrDefaultAsync(x => x.Email == id);
            var sagaData = await DbContext.BookingSagaData.FirstOrDefaultAsync(x => x.Email == id);

            Assert.NotNull(traveler);
            Assert.NotNull(sagaData);
        }
    }
}
