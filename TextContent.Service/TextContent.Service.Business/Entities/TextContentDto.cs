using System.Collections.Generic;

namespace TextContent.Service.Business.Entities
{
    public class TextContentDto
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Language { get; set; }
        public IEnumerable<TranslationDto> Dictionary { get; set; }
    }
}