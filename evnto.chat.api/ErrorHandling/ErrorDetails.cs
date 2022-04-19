﻿using System.Text.Json;

namespace evnto.chat.api.ErrorHandling
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
