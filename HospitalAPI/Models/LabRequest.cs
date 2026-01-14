using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HospitalAPI.Models
{
    public class LabRequest
    {
        [Key]
        public int Id { get; set; } // Primary key for EF

        public string? Address { get; set; }
        public DateTime DateRequestReceived { get; set; }
        public string? Diagnosis { get; set; }
        public bool IsUrgent { get; set; }
        public string? LabRequestId { get; set; } // For external API / JSON
        public string? Location { get; set; }
        public int PatientAge { get; set; }
        public string? PatientBedNumber { get; set; }
        public DateTime PatientBirthDate { get; set; }
        public string? PatientFirstName { get; set; }
        public string? PatientGender { get; set; }
        public string? PatientId { get; set; }
        public string? PatientOtherName { get; set; }
        public string? PPatientPhone { get; set; }
        public string? PatientStage { get; set; }
        public string? PatientSurname { get; set; }
        public string? PatientWard { get; set; }
        public string? ReceiptNumber { get; set; }
        public string? ReceiptType { get; set; }
        public string? RequestedByName { get; set; }
        public string? OpenMrsId { get; set; }

        
        public List<LabTest>? LabTests { get; set; }
    }

    public class LabTest
    {
        [Key]
        public int Id { get; set; } // Primary key for EF

        public string? LabTestId { get; set; }
        public string? LabTestName { get; set; }

        // Foreign key to LabRequest
        [JsonIgnore]
        public int LabRequestId { get; set; } 

        [ForeignKey("LabRequestId")]
        [JsonIgnore]
        public LabRequest? LabRequest { get; set; } 
    }
}
