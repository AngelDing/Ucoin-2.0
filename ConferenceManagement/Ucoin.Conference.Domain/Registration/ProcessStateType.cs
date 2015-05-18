
namespace Ucoin.Conference.Domain
{
    public enum ProcessStateType
    {
        NotStarted = 0,
        AwaitingReservationConfirmation = 1,
        ReservationConfirmationReceived = 2,
        PaymentConfirmationReceived = 3,
    }
}
