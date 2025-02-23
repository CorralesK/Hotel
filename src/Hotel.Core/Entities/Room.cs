using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hotel.src.Hotel.Core.Enums;

namespace Hotel.src.Hotel.Core.Entities
{
    class Room
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int ID { get; }
        private RoomType TYPE { get; set; }
        private int ROOMNUMBER { get; set; }
        private float PRICE { get; set; }
        private RoomStatus STATUS { get; set; }
    }
}
