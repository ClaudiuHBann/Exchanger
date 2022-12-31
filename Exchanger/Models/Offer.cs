﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Exchanger.Models
{
    public class Offer
    {
        public int Id { get; set; }
        [ForeignKey("Profile")]
        public int IdProfile { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } = null;
        public List<string> Images { get; set; }
    }
}