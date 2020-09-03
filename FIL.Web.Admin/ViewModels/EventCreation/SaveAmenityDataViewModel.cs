using FIL.Contracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FIL.Web.Admin.ViewModels.EventCreation
{
    public class SaveAmenityDataViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Amenity { get; set; }
    } 
}
