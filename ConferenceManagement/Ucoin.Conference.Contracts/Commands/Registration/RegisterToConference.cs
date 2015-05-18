
namespace Ucoin.Conference.Contracts.Commands.Registration
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Ucoin.Framework.Messaging;

    public class RegisterToConference : ICommand, IValidatableObject
    {
        public RegisterToConference()
        {
            this.Id = Guid.NewGuid();
            this.Seats = new Collection<SeatQuantity>();
        }

        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public Guid ConferenceId { get; set; }

        public ICollection<SeatQuantity> Seats { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Seats == null || !this.Seats.Any(x => x.Quantity > 0))
            {
                 return new[] { new ValidationResult("One or more items are required.", new[] { "Seats" }) };
            }
            else if (this.Seats.Any(x => x.Quantity < 0))
            {
                return new[] { new ValidationResult("Invalid registration.", new[] { "Seats" }) };
            }

            return Enumerable.Empty<ValidationResult>();
        }
    }
}
