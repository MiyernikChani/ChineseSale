﻿using System.ComponentModel.DataAnnotations;

namespace ChneseSaleApi.models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
