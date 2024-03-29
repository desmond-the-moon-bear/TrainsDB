﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class TrainCar
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Train Car Type")]
        public TrainCarType TrainCarType { get; set; }

        public double Weight { get; set; }

        #region Navigation

        [Required]
        public Location Location { get; set; }
        
        [ForeignKey("Location")]
        [DisplayName("Location")]
        public int LocationId { get; set; }

        [DisplayName("Train Composition")]
        public TrainComposition? TrainComposition { get; set; }
        
        [ForeignKey("TrainComposition")]
        [DisplayName("Train Composition")]
        public int? TrainCompositionId { get; set; }

        #endregion

        public TrainCar() { }
        
        public TrainCar(
            TrainCarType trainCarType,
            double weight,
            Location location,
            TrainComposition? trainComposition = null)
        {
            TrainCarType = trainCarType;
            Location = location;
            Weight = weight;
            TrainComposition = trainComposition;
        }
    }
}
