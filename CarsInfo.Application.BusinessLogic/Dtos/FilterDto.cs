﻿using System.Collections.Generic;

namespace CarsInfo.Application.BusinessLogic.Dtos
{
    public class FilterDto
    {
        public IList<string> Brands { get; set; } = new List<string>();

        public string Model { get; set; }

        public int Skip { get; set; } = 0;

        public int Take { get; set; } = 3;
    }
}