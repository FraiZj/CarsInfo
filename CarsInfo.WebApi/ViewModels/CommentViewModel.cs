﻿using System;

namespace CarsInfo.WebApi.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTimeOffset PublishDate { get; set; }
        
        public string UserName { get; set; }
    }
}