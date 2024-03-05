﻿using System.ComponentModel.DataAnnotations;

namespace Jumia.Dtos
{
    public class AddressDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Street { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string State { get; set; }

        //[Required]
        //[MaxLength(100)]
        //public string Country { get; set; }

        [MaxLength(20)]
        public string ZipCode { get; set; }

        public int UserID { get; set; }
        public virtual ApplicationUserDto User { get; set; }
    }
}