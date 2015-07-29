using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SetSite.Models
{
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        //public int CreateUserId { get; set; }
        public double? TotalSeconds { get; set; }

        //[ForeignKey("CreateUserId")]
        //public ApplicationUser CreateUser { get; set; }
        public virtual List<PlayerGame> PlayerGames { get; set; }
    }
}