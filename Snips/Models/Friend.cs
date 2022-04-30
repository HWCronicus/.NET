using System;
using System.Collections.Generic;

namespace Sabio.Models.Domain
{
    public class Friend
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Bio { get; set; }
        public string Summary { get; set; }
        public string Headline { get; set; }
        public string Slug { get; set; }
        public int StatusId { get; set; }
        public int PrimaryImageId { get; set; }
        public string Url { get; set; }
        public int TypeId { get; set; }
        public List<Skill> Skills { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        
    }
}
