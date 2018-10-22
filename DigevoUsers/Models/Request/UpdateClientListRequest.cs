namespace DigevoUsers.Models.Request
{

    public class UpdateClientListRequest
    {
        public int? idClient { get; set; } = null;  
        public int? idList { get; set; } = null;    // idLista
        public int? idGuide { get; set; } = null;   // idPauta

    }
}