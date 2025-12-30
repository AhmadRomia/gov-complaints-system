using Domain.Common;
using Domain.Enums;
using Domain.ValueObjects;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public class Complaint : BaseAuditableEntity
    {

        public required string Title { get; set; }
        public string? Description { get; set; }
        public int Severity { get; set; }
        public ComplaintStatus Status { get; set; } = ComplaintStatus.New;

        public Guid CitizenId { get; set; }

        public ApplicationUser? Citizen { get; set; } 
        public string? ReferenceNumber { get; set; }
        public ComplaintType Type { get; set; }
        public decimal LocationLong { get; set; }

        public decimal LocationLat { get; set; }
        public List<Attachment> Attachments { get; set; } = new();

        public Guid? GovernmentEntityId { get; set; }
        public GovernmentEntity? GovernmentEntity { get; set; }

        // Notes added by agency staff while processing the complaint
        public string? AgencyNotes { get; set; }
        // When agency requests more info from citizen, can store a short message
        public string? AdditionalInfoRequest { get; set; }


        public SyrianGovernorate Governorate { get; set; }

        public Guid? LockedBy { get; set;  }

        // Actions log
        public List<ComplaintAction> Actions { get; set; } = new();
    }
}
