using Ucoin.Framework.ValueObjects;
namespace Ucoin.Conference.Entities.MongoDb
{

    public class OrderSeat
    {
        public OrderSeat()
        {
            this.Attendee = new PersonalInfo();
        }

        public OrderSeat(int position, string seatName)
        {
            this.Position = position;
            this.SeatName = seatName;
            this.Attendee = new PersonalInfo();
        }

        public int Position { get; set; }

        public string SeatName { get; set; }

        public PersonalInfo Attendee { get; set; }
    }
}
