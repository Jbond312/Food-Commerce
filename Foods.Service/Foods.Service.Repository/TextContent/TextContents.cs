using System.Collections.Generic;
using Common.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Foods.Service.Repository.TextContent
{
    [ModelMetadataType(typeof(TextContents))]
    public class TextContents : BaseEntity
    {
        public string Key { get; set; }
        public string Language { get; set; }
        public IEnumerable<Translation> Dictionary { get; set; }
    }
}