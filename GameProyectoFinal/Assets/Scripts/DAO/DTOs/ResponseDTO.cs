using System.Collections.Generic;

namespace Assets.Scripts.DAO.DTOs
{
    public class ResponseDTO
    {
        public List<HighscoreEntryDTO> Data { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
