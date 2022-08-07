﻿using Portal.Domain.Interfaces;

namespace Portal.Domain.Models
{
    public class BookMaterial : Material
    {
        public string Authors { get; set; }

        public string Title { get; set; }

        public int CountPages { get; set; }

        public string Format { get; set; }

        public DateTime DatePublication { get; set; }

        public override string ToString()
        {
            return $"{Authors} - {Title}, {CountPages} ({DatePublication.ToString("d")}).{Format}";
        }

        public override bool Equals(object? obj)
        {
            if (obj is BookMaterial other)
            {
                return GetHashCode() == other.GetHashCode();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Authors.GetHashCode() + Title.GetHashCode() + CountPages.GetHashCode() + Format.GetHashCode() + DatePublication.GetHashCode();
        }
    }
}
