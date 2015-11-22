﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITConferences.Domain.Entities
{
    public class Conference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ConferenceID { get; set; }

        [Required]
        [Display(Name = "Name of the conference")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "When?")]
        public DateTime Date { get; set; }

        [Required]
        [Url(ErrorMessage = "Invalid URL!")]
        [Display(Name = "Website")]
        public string Url { get; set; }

        [Display(Name = "Is it paid?")]
        public bool IsPaid { get; set; }

        public virtual City TargetCity { get; set; }
        public virtual Country TargetCountry { get; set; }
        public virtual ICollection<Attendee> Attendees { get; set; }
        public virtual ICollection<Speaker> Speakers { get; set; }
        public virtual ICollection<Organizer> Organizers { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Inspiration> Media { get; set; }

        [ForeignKey("TargetCity")]
        public int TargetCityId { get; set; }
        [ForeignKey("TargetCountry")]
        public int TargetCountryId { get; set; }
    }
}
