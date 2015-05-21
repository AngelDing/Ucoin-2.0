using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Ucoin.Conference.Entities.Properties;
using Ucoin.Framework.Entities;
using Ucoin.Framework.SqlDb.Entities;
using Ucoin.Framework.Utils;
using Ucoin.Framework.ValueObjects;

namespace Ucoin.Conference.Entities
{
    public class EditableConference : EfEntity<Guid>, IAggregateRoot<Guid>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        public string Tagline { get; set; }

        public string TwitterSearch { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "Start")]
        public DateTime StartDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        [Display(Name = "End")]
        public DateTime EndDate { get; set; }

        ///// <summary>
        ///// 可預訂時間區間
        ///// </summary>
        //public DateTimeRange BookableDateRange { get; set; }

        [Display(Name = "Is Published?")]
        public bool IsPublished { get; set; }
    }

    public class ConferenceInfo : EditableConference
    {
        public ConferenceInfo()
        {
            this.Id = GuidHelper.NewSequentialId();
            this.Seats = new ObservableCollection<SeatType>();
            this.AccessCode = HandleGenerator.Generate(6);
        }

        [StringLength(6, MinimumLength = 6)]
        public string AccessCode { get; set; }

        [Display(Name = "Owner")]
        [Required]
        public string OwnerName { get; set; }

        [Display(Name = "Email")]
        [Required]
        [RegularExpression(@"[\w-]+(\.?[\w-])*\@[\w-]+(\.[\w-]+)+", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "InvalidEmail")]
        public string OwnerEmail { get; set; }

        [Required]
        [RegularExpression(@"^\w+$", ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "InvalidSlug")]
        public string Slug { get; set; }

        public bool WasEverPublished { get; set; }

        public virtual ICollection<SeatType> Seats { get; set; }
    }
}
